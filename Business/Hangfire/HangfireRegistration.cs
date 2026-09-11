using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Hangfire
{
    public static class HangfireRegistration
    {
        public static void AddHangfireApplication(IServiceCollection services, string connectionString)
        {
            // PostgreSQL bağlantı dizesi ile Hangfire yapılandırması
            services.AddHangfire(config =>
                config.UsePostgreSqlStorage(opt =>
                {
                    opt.UseNpgsqlConnection(connectionString);
                }));
            // Hangfire Server'ı ekliyoruz
            services.AddHangfireServer();
        }
    }
}
