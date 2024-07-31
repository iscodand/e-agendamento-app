using E_Agendamento.Domain.Entities;
using E_Agendamento.Infrastructure.Data.Context;
using E_Agendamento.Infrastructure.Data.Repositories.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Agendamento.Infrastructure.Data.Repositories
{
    public class UserRoleRepository : GenericRepository<UsersRoles>, IUserRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRoleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ICollection<ApplicationRole>> GetUserRolesByUserIdAsync(string userId)
        {
            var usersRoles = await _context.UserRoles.AsNoTracking()
                                    .Where(x => x.UserId == userId)
                                    .ToListAsync()
                                    .ConfigureAwait(false);

            List<ApplicationRole> roles = [];

            foreach (UsersRoles userRole in usersRoles)
            {
                ApplicationRole role = await _context.Roles.AsNoTracking().Where(x => x.Id == userRole.RoleId).FirstOrDefaultAsync();
                roles.Add(role);
            }

            return roles;
        }
    }
}