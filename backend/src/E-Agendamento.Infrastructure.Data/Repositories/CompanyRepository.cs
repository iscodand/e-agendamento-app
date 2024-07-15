using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Domain.Entities;
using E_Agendamento.Infrastructure.Data.Context;
using E_Agendamento.Infrastructure.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace E_Agendamento.Infrastructure.Data.Repositories
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        private readonly DbSet<Company> _companies;

        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
            _companies = context.Companies;
        }

        public async Task<bool> AddUserToCompanyAsync(Company company, ApplicationUser user)
        {
            company.Users.Add(user);
            await UpdateAsync(company);
            return true;
        }

        public async Task<bool> ExistsByCNPJAsync(string cnpj)
        {
            return await _companies.AsNoTracking()
                                .Where(x => x.CNPJ.ToUpper().Trim() == cnpj.ToUpper().Trim())
                                .AnyAsync()
                                .ConfigureAwait(false);
        }
    }
}