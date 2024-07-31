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
        private readonly ApplicationDbContext _context;

        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
            _companies = context.Companies;
            _context = context;
        }

        public async Task<bool> AddUserToCompanyAsync(Company company, ApplicationUser user)
        {
            UsersCompanies userCompany = UsersCompanies.Create(user.Id, company.Id);
            await _context.UsersCompanies.AddAsync(userCompany);

            // TODO => melhorar esse lixo
            return true;
        }

        public async Task<bool> ExistsByCNPJAsync(string cnpj, string companyToUpdateId = "")
        {
            return await _companies.AsNoTracking()
                                .Where(x => x.CNPJ.ToUpper().Trim() == cnpj.ToUpper().Trim())
                                .Where(x => x.Id != companyToUpdateId)
                                .AnyAsync()
                                .ConfigureAwait(false);
        }
    }
}