using E_Agendamento.Application.Contracts.Repositories.Common;
using E_Agendamento.Domain.Entities;

namespace E_Agendamento.Application.Contracts.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        public Task<bool> ExistsByIdAsync(string categoryId, string companyId, CancellationToken cancellationToken);
    }
}