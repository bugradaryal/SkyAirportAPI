using DataAccess;
using Entities;
using Entities.Moderation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Threading.RateLimiting;
using Utilitys.Configuration;
using Serilog;
using Serilog.Sinks.PostgreSQL;
using DataAccess.LogManager;
using Serilog.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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
            /*
            builder.Services.AddScoped<DatabaseLogSink>(); // DatabaseLogSink'i DI container'a kaydediyoruz.
            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilog(new LoggerConfiguration()
                    .WriteTo.Console()  // Konsola log yazma (isteğe bağlı)
                    .WriteTo.Sink(new DatabaseLogSink(builder.Services.BuildServiceProvider().GetRequiredService<DataDbContext>()))  // Burada lambda kullanmak yerine direkt olarak kullanıyoruz
                    .CreateLogger());
            });
            */
            builder.Services.AddControllers();

            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

            builder.Services.Configure<JwtBearer>(builder.Configuration.GetSection("JwtBearer"));

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


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
            });

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

            var allowedOrigins = builder.Configuration["CORS:Origin"].ToString();
            app.UseCors(builder =>
                builder.WithOrigins(allowedOrigins)
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithExposedHeaders("Authorization", "RefreshToken"));

            app.MapControllers();

            app.Run();
        }
    }
}
