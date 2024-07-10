using E_Agendamento.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace E_Agendamento.WebAPI.Helpers
{
    public static class MigrationsHelper 
    {
        public static async Task MigrateAsync(this IApplicationBuilder app) 
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateScope();
            await serviceScope.ServiceProvider.GetService<ApplicationDbContext>().Database.MigrateAsync();
        } 
    }
}