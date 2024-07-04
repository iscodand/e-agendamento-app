using E_Agendamento.Application.Contracts.Repositories.Common;
using E_Agendamento.Domain.Entities;

namespace E_Agendamento.Application.Contracts.Repositories
{
    public interface IUserRepository : IGenericRepository<ApplicationUser>
    {
        public Task<ApplicationUser> GetWithCompaniesAsync(string userId);
    }
}