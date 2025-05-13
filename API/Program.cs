using DataAccess;
using Entities;
using Entities.Moderation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Threading.RateLimiting;
using Serilog;
using Serilog.Sinks.PostgreSQL;
using Serilog.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Entities.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Business;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Utilitys.Logger;
using Serilog.Sinks.Elasticsearch;
using Serilog.Exceptions;
using Business.FluentValidation;
using FluentValidation.AspNetCore;
using System.Diagnostics;
using Business.Abstract;
using Business.Concrete;
using Microsoft.AspNetCore.Mvc;
using Utilitys;
using Utilitys.Mapper;
using Hangfire;
using Business.Hangfire;
using Business.Hangfire.Manager;
using Business.Hangfire.Jobs;
using Business.Redis;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<DataDbContext>();


            builder.WebHost.UseIISIntegration();
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(7257, listenOptions =>
                {
                    listenOptions.UseHttps(); // HTTPS
                });
            });

            // Serilog'u ayarlıyoruz
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            // 🔹 Uygulama logger'ı olarak tanıt
            builder.Host.UseSerilog();
            builder.Services.AddSingleton<ISerilogServices, SerilogLogger>();
            builder.Services.AddScoped<ILoggerServices, LoggerManager>();

            builder.Services.AddValidationApplication();

            builder.Services.Configure<JwtBearer>(builder.Configuration.GetSection("JwtBearer"));
            builder.Services.Configure<EmailSender>(builder.Configuration.GetSection("EmailSender"));
            builder.Services.Configure<CallBackURL>(builder.Configuration.GetSection("CallBackURL"));
            builder.Services.Configure<Entities.Configuration.SecurityKey>(builder.Configuration.GetSection("SecurityKey"));

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedPhoneNumber = true;
                options.SignIn.RequireConfirmedEmail = true;

                options.Password.RequireDigit = true;  // Sayı zorunlu
                options.Password.RequireLowercase = true;  // Küçük harf zorunlu
                options.Password.RequireNonAlphanumeric = false;  // Alfasayısal olmayan karakter zorunlu
                options.Password.RequireUppercase = true;  // Büyük harf zorunlu
                options.Password.RequiredLength = 8;  // Minimum şifre uzunluğu
                options.Password.RequiredUniqueChars = 0;  // Benzersiz karakter sayısı zorunlu

                // Lockout ayarları
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);  // Hesap 5 dakika kilitlensin
                options.Lockout.MaxFailedAccessAttempts = 5;  // Başarısız giriş denemesi sayısı
                options.Lockout.AllowedForNewUsers = true;  // Yeni kullanıcılar için kilitleme uygulansın
                
                // Kullanıcı adı ayarları
                options.User.RequireUniqueEmail = true;  // Her kullanıcı adı benzersiz olmalı
                options.User.AllowedUserNameCharacters ="abcdefghýijklmnoöpqrsþtuüvwxyzABCDEFGHIÝJKLMNOÖPQRSTUÜVWXYZ0123456789-_";
            }).AddEntityFrameworkStores<DataDbContext>().AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = builder.Configuration["JwtBearer:Issuer"],
                    ValidAudience = builder.Configuration["JwtBearer:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtBearer:Key"]))
                };
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("IsUserSuspended", policy =>
                    policy.RequireClaim("IsSuspended", "false"));
            });
            builder.Services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                {
                    var userIp = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                    if (httpContext.User.IsInRole("Admin"))
                    {
                        // Admin'lere loglama yapılmaz
                        return RateLimitPartition.GetNoLimiter("AdminUser");
                    }

                    var userId = httpContext.User.Identity?.Name ?? userIp;

                    // Loglama yapalım
                    Serilog.Log.Information("User {UserId} is making a request from IP {UserIp}", userId, userIp);

                    return RateLimitPartition.GetFixedWindowLimiter(
                        userId,
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 10,
                            Window = TimeSpan.FromSeconds(30),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 2
                        });
                });
            });
            // FluentValidation için gerekli konfigürasyonları ekleyin
            builder.Services.AddFluentValidationAutoValidation()  // Sunucu tarafı doğrulama
                .AddFluentValidationClientsideAdapters();
            builder.Services.AddControllers()
                    .AddNewtonsoftJson(options => {

                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

                        options.SerializerSettings.Formatting = Formatting.Indented;

                        options.SerializerSettings.Error = (sender, args) =>
                        {
                            args.ErrorContext.Handled = true;
                        };
                    });
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            HangfireRegistration.AddHangfireApplication(builder.Services, connectionString);
            builder.Services.AddTransient<GetForex>();
            builder.Services.AddScoped<BackGroundSchedule>();
            builder.Services.AddMediatRApplication();
            builder.Services.AddMapperApplication();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSingleton<IRedisServices, RedisServices>();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });

                // JWT Bearer Authentication
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                // API Key Authentication
                opt.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "API Key needed to access the endpoints. Example: 'X-API-KEY: your_api_key'",
                    Name = "X-API-KEY",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "ApiKeyScheme"
                });

                // Security Requirements - JWT ve API Key birlikte
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            In = ParameterLocation.Header
                        },
                        new string[] {}
                    },
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "ApiKey"
                            },
                            In = ParameterLocation.Header
                        },
                        new string[] {}
                    }
                });
            });


            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ISerilogServices>();

                    // Hata loglama: ModelState hatalarını logla
                    logger.Info(new DTO.LogDTO { Message = "Validation hatası: " + context.ModelState , Action_type = Entities.Enums.Action_Type.APIResponse, loglevel_id = 3, AdditionalData= context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()});

                    // Hata mesajını özelleştirme
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Geçersiz veri gönderildi", // İstediğiniz başlık
                    };

                    // Response'u döndür
                    return new BadRequestObjectResult(problemDetails);
                };
            });

            //builder.Services.AddSignalR();
            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRateLimiter();
            app.UseRouting(); 
            app.UseAuthentication();  
            app.UseAuthorization();
            app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), appBuilder =>
            {
                appBuilder.UseMiddleware<ApiKeyMiddleware>();
            });
            var allowedOrigins = builder.Configuration["CORS:Origin"].ToString();
            app.UseCors(builder =>
                builder.WithOrigins(allowedOrigins)
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithExposedHeaders("Authorization", "RefreshToken"));

            app.UseHangfireDashboard("/hangfire");  // Dashboard '/hangfire' üzerinden erişilebilir olacak

            // Hangfire server'ı başlatıyoruz
            app.UseHangfireServer();
            using (var scope = app.Services.CreateScope())
            {
                var jobs = scope.ServiceProvider.GetRequiredService<BackGroundSchedule>();
                jobs.ScheduleRecurringJobs();

                // İlk çalıştırmak için:
                var backgroundJobClient = scope.ServiceProvider.GetRequiredService<IBackgroundJobClient>();
                backgroundJobClient.Enqueue<GetForex>(job => job.Run());
            }
            app.MapControllers();
            //app.MapHub<SignalRHub>("/signalrhub");
            app.Run();
        }
    }
}
