using E_Agendamento.Application.DTOs.Account;
using E_Agendamento.Domain.Entities;
using E_Agendamento.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Agendamento.Infrastructure.Identity.Seeds
{
    public static class DefaultUsers
    {
        public async static Task SeedAsync(UserManager<ApplicationUser> userManager, CancellationToken cancellationToken)
        {
            RegisterRequest superAdmin = new()
            {
                FullName = "Isco D'Andrade",
                UserName = "iscodandrade",
                Email = "iscodand@outlook.com",
                Roles = [nameof(Roles.SuperAdmin)],
                Password = "12345678",
                ConfirmPassword = "12345678"
            };

            RegisterRequest basic = new()
            {
                FullName = "Jose Carlos",
                UserName = "jose",
                Email = "jose@email.com",
                Roles = [nameof(Roles.Basic)],
                Password = "12345678",
                ConfirmPassword = "12345678"
            };

            bool usersAlreadyRegistered = await userManager.Users.AsNoTracking()
                .AnyAsync(x => x.UserName.Contains(basic.UserName) &&
                               x.UserName.Contains(superAdmin.UserName)
                               , cancellationToken)
                .ConfigureAwait(false);
            if (usersAlreadyRegistered == false)
            {
                ApplicationUser newSuperAdmin = RegisterRequest.Map(superAdmin);
                ApplicationUser newBasicUser = RegisterRequest.Map(basic);

                await userManager.CreateAsync(newSuperAdmin, superAdmin.Password);
                await userManager.CreateAsync(newBasicUser, basic.Password);

                await userManager.AddToRolesAsync(newSuperAdmin, superAdmin.Roles);
                await userManager.AddToRolesAsync(newBasicUser, basic.Roles);
            }
        }
    }
}