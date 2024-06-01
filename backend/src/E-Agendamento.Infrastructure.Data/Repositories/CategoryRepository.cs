using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Domain.Entities;
using E_Agendamento.Infrastructure.Data.Context;
using E_Agendamento.Infrastructure.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace E_Agendamento.Infrastructure.Data.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly DbSet<Category> _categories;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _categories = context.Categories;
        }

        public async Task<bool> AlreadyRegisteredByDescriptionAsync(string description, string companyId, CancellationToken cancellationToken)
        {
            return await _categories.AsNoTracking()
                                    .Where(x => x.Description.ToUpper().Trim() == description.ToUpper().Trim())
                                    .AnyAsync()
                                    .ConfigureAwait(false);
        }

        public async Task<bool> ExistsByIdAsync(string categoryId, string companyId, CancellationToken cancellationToken)
        {
            return await _categories.AsNoTracking()
                                    .Where(x => x.Id == categoryId && x.CompanyId == companyId)
                                    .AnyAsync(cancellationToken: cancellationToken)
                                    .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Category>> GetByCompanyAsync(string companyId, CancellationToken cancellationToken)
        {
            return await _categories.AsNoTracking()
                                    .Where(x => x.CompanyId == companyId)
                                    .ToListAsync(cancellationToken)
                                    .ConfigureAwait(false);
        }
    }
}