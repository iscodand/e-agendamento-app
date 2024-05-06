using E_Agendamento.Application.Contracts.Repositories.Common;
using E_Agendamento.Domain.Entities;

namespace E_Agendamento.Application.Contracts.Repositories
{
    public interface ICompanyRepository : IGenericRepository<Company>
    {
        public Task<bool> AddUserToCompanyAsync(Company company, ApplicationUser user);
    }
}