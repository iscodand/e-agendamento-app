using System.Security.Cryptography.X509Certificates;
using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Domain.Entities;
using E_Agendamento.Infrastructure.Data.Context;
using E_Agendamento.Infrastructure.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace E_Agendamento.Infrastructure.Data.Repositories
{
    public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
    {
        private readonly DbSet<ApplicationUser> _users;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _users = context.Users;
        }

        public async Task<ApplicationUser> GetWithCompaniesAsync(string userId)
        {
            return await _users.AsNoTracking()
                            .Include(x => x.Companies)
                            .Where(x => x.Id == userId)
                            .FirstOrDefaultAsync()
                            .ConfigureAwait(false);
        }

        public async Task<bool> UserInCompanyAsync(string userId, string companyId)
        {
            return await _users.AsNoTracking()
                            .Include(x => x.Companies)
                            .AnyAsync(x => x.Companies.Select(x => x.Id).Contains(companyId))
                            .ConfigureAwait(false);
        }
    }
}