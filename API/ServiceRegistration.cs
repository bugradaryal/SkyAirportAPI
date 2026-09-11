using Business.Features;
using Business.Hangfire.Jobs;
using Business.Hangfire.Manager;
using Business.Redis;
using DataAccess.Abstract;
using DataAccess.Concrete.Generic;
using Utilitys.Logging;
using Utilitys.Logging.Serilog;
using Utilitys.Mapper;

namespace API
{
    public static class ServiceRegistration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IMapper, Mapper>();
            services.AddScoped<ISerilogServices, SerilogLogger>();
            services.AddScoped<ILoggerServices, LoggerManager>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            // DataAccess: Repository sınıflarının otomatik taranması (XRepository : IXRepository kalıbı)
            services.Scan(scan => scan
                .FromAssemblyOf<DataAccess.AssemblyInfo>()
                .AddClasses(c => c.Where(type =>
                    type.GetInterfaces().Any(i => i.Name.EndsWith("Repository"))))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            // Business: interface'i "Service"/"Services" ile biten class'lar
            services.Scan(scan => scan
                .FromAssemblyOf<Business.AssemblyInfo>()
                .AddClasses(c => c.Where(type =>
                    type.GetInterfaces().Any(i => i.Name.EndsWith("Services"))))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            // İsim kalıbına uymayan / tekil kayıtlar
            services.AddTransient<GetForex>();
            services.AddScoped<BackGroundSchedule>();
            services.AddScoped<IRedisServices, RedisServices>();
            services.AddMediatRApplication();

        }
    }
}
