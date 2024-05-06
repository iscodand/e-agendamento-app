using E_Agendamento.Application.Contracts.Services;
using E_Agendamento.Infrastructure.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace E_Agendamento.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDateTimeService, DateTimeService>();
            services.AddScoped<IEmailService, EmailService>();
        }
    }
}