using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Application.DTOs.Account;
using E_Agendamento.Domain.Entities;
using E_Agendamento.Infrastructure.Data.Context;
using E_Agendamento.Infrastructure.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace E_Agendamento.Infrastructure.Data.Repositories
{
    public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
    {
        private readonly DbSet<ApplicationUser> _users;
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _users = context.Users;
            _context = context;
        }

        public async Task<IEnumerable<RetrieveUserResponse>> GetUsersByCompanyAsync(string companyId)
        {
            var users = await (
                from user in _context.Users.AsNoTracking()
                join userRole in _context.UsersRoles on user.Id equals userRole.UserId into userRoleJoin
                from ur in userRoleJoin.DefaultIfEmpty()
                join userCompany in _context.UsersCompanies on user.Id equals userCompany.UserId
                where userCompany.CompanyId == companyId
                select new
                {
                    user.Id,
                    user.FullName,
                    user.UserName,
                    user.Email,
                    user.IsActive,
                    Roles = (from ur in _context.UsersRoles
                             join r in _context.Roles on ur.RoleId equals r.Id
                             where ur.UserId == user.Id
                             select r.Description).ToList(),
                    Companies = (from uc in _context.UsersCompanies
                                 join c in _context.Companies on uc.CompanyId equals c.Id
                                 where uc.UserId == user.Id
                                 select c.Name).ToList()
                }
            ).ToListAsync().ConfigureAwait(false);

            var response = users.Select(u => new RetrieveUserResponse
            {
                Id = u.Id.ToString(),
                FullName = u.FullName,
                UserName = u.UserName,
                IsActive = u.IsActive,
                Email = u.Email,
                Roles = u.Roles,
                Companies = u.Companies
            });

            return response;
        }

        public async Task<ApplicationUser> GetWithCompaniesAsync(string userId)
        {
            return await _users.AsNoTracking()
                            .Include(x => x.UsersCompanies)
                            .ThenInclude(x => x.Company)
                            .Where(x => x.Id == userId)
                            .FirstOrDefaultAsync()
                            .ConfigureAwait(false);
        }

        public async Task<IEnumerable<ApplicationUser>> SearchByTermAsync(string searchTerm)
        {
            return await _context.Users
                                .AsNoTracking()
                                .Include(x => x.UsersCompanies)
                                .Where(x => x.FullName.Contains(searchTerm) ||
                                            x.UserName.Contains(searchTerm) ||
                                            x.Email.Contains(searchTerm))
                                .ToListAsync()
                                .ConfigureAwait(false);
        }

        public async Task<bool> UserInCompanyAsync(string userId, string companyId)
        {
            return await _users.AsNoTracking()
                            .Include(x => x.UsersCompanies)
                            .ThenInclude(x => x.Company)
                            .AnyAsync()
                            .ConfigureAwait(false);
        }
    }
}