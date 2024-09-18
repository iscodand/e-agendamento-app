using E_Agendamento.Application.Contracts.Repositories.Common;
using E_Agendamento.Application.DTOs.Account;
using E_Agendamento.Domain.Entities;

namespace E_Agendamento.Application.Contracts.Repositories
{
    public interface IUserRepository : IGenericRepository<ApplicationUser>
    {
        public Task<ApplicationUser> GetWithCompaniesAsync(string userId);
        public Task<bool> UserInCompanyAsync(string userId, string companyId);
        public Task<IEnumerable<ApplicationUser>> SearchByTermAsync(string searchTerm);
        public Task<ApplicationUser> GetWithRolesAndCompaniesAsync(string userId);
        public Task<IEnumerable<RetrieveUserResponse>> GetUsersByCompanyAsync(string companyId);
    }
}