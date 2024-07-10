using E_Agendamento.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace E_Agendamento.WebAPI.Helpers
{
    public static class SeedHelper
    {
        public static async Task SeedDatabaseAsync(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            {
                try
                {
                    UserManager<ApplicationUser> userManager = (UserManager<ApplicationUser>)scope
                        .ServiceProvider
                        .GetService(typeof(UserManager<ApplicationUser>));

                    // ICompanyService companyService = (ICompanyService)scope
                    //     .ServiceProvider
                    //     .GetService(typeof(ICompanyService));

                    RoleManager<ApplicationRole> roleManager = (RoleManager<ApplicationRole>)scope
                        .ServiceProvider
                        .GetService(typeof(RoleManager<ApplicationRole>));

                    await Infrastructure.Identity.Seeds.DefaultRoles.SeedAsync(roleManager, new CancellationToken());
                    await Infrastructure.Identity.Seeds.DefaultUsers.SeedAsync(userManager, new CancellationToken());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ops! An Error ocurred while seeding database: {ex}");
                }
            }
        }
    }
}