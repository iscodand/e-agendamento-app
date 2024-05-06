using E_Agendamento.Application.DTOs.Account;
using E_Agendamento.Domain.Entities;
using E_Agendamento.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Agendamento.Infrastructure.Identity.Seeds
{
    public static class DefaultSuperAdmin
    {
        public async static Task SeedAsync(UserManager<ApplicationUser> userManager, CancellationToken cancellationToken)
        {
            RegisterRequest request = new()
            {
                FullName = "Isco D'Andrade",
                UserName = "iscodandrade",
                Email = "iscodand@outlook.com",
                Roles = [nameof(Roles.SuperAdmin)],
                Password = "P@$$w0rd",
                ConfirmPassword = "P@$$w0rd"
            };

            bool userAlreadyRegistered = await userManager.Users.AnyAsync(x => x.UserName == request.UserName, cancellationToken);

            if (userAlreadyRegistered == false)
            {
                ApplicationUser newUser = RegisterRequest.Map(request);
                await userManager.CreateAsync(newUser, request.Password);
            }
        }
    }
}