using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Domain.Entities;
using E_Agendamento.Infrastructure.Data.Context;
using E_Agendamento.Infrastructure.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace E_Agendamento.Infrastructure.Data.Repositories
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        private readonly DbSet<Item> _items;

        public ItemRepository(ApplicationDbContext context) : base(context)
        {
            _items = context.Items;
        }

        public async Task<IEnumerable<Item>> GetByCompanyPagedAsync(string companyId, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            return await _items.AsNoTracking()
                            .Include(x => x.Category)
                            .Where(x => x.CompanyId == companyId)
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .AsNoTracking()
                            .ToListAsync(cancellationToken: cancellationToken)
                            .ConfigureAwait(false);
        }

        public async Task<bool> IsUniqueAsync(string name, string companyId, CancellationToken cancellationToken, string itemIdToComparison = "")
        {
            return await _items.AsNoTracking()
                            .Where(x => x.Name.ToUpper().Trim() == name.ToUpper().Trim())
                            .Where(x => x.CompanyId == companyId)
                            .Where(x => x.Id != itemIdToComparison)
                            .AnyAsync(cancellationToken)
                            .ConfigureAwait(false);
        }
    }
}