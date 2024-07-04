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
                Password = "P@$$w0rd",
                ConfirmPassword = "P@$$w0rd"
            };

            RegisterRequest basic = new()
            {
                FullName = "José Carlos",
                UserName = "jose",
                Email = "jose@email.com",
                Roles = [nameof(Roles.Basic)],
                Password = "12345678",
                ConfirmPassword = "12345678"
            };

            bool basicUserAlreadyRegistered = await userManager.Users.AnyAsync(x => x.UserName == basic.UserName, cancellationToken);
            if (basicUserAlreadyRegistered == false)
            {
                ApplicationUser newSuperAdmin = RegisterRequest.Map(superAdmin);
                ApplicationUser newBasicUser = RegisterRequest.Map(basic);

                await userManager.CreateAsync(newSuperAdmin, superAdmin.Password);
                await userManager.CreateAsync(newBasicUser, basic.Password);
            }
        }
    }
}