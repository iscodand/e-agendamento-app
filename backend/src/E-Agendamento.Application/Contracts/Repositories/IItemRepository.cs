using E_Agendamento.Application.Contracts.Repositories.Common;
using E_Agendamento.Domain.Entities;

namespace E_Agendamento.Application.Contracts.Repositories
{
    public interface IItemRepository : IGenericRepository<Item>
    {
        public Task<bool> IsUniqueAsync(string name, string companyId, CancellationToken cancellationToken, string itemIdToComparison = "");
        public Task<IEnumerable<Item>> GetByCompanyPagedAsync(string companyId, int pageNumber, int pageSize, CancellationToken cancellationToken);
    }
}