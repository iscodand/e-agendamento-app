using E_Agendamento.Domain.Entities;
using E_Agendamento.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Agendamento.Infrastructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<ApplicationRole> roleManager, CancellationToken cancellationToken)
        {
            await roleManager.CreateAsync(ApplicationRole.Create(nameof(Roles.SuperAdmin), "Super Administrador"));
            await roleManager.CreateAsync(ApplicationRole.Create(nameof(Roles.Admin), "Administrador"));
            await roleManager.CreateAsync(ApplicationRole.Create(nameof(Roles.Basic), "Basico"));
        }
    }
}