using E_Agendamento.Domain.Entities;

namespace E_Agendamento.Application.Features.Companies.Queries.GetCompanyById
{
    public class GetCompanyViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public static GetCompanyViewModel Map(Company company)
        {
            return new()
            {
                Id = company.Id,
                Name = company.Name,
                CNPJ = company.CNPJ,
                Description = company.Description,
                IsActive = company.IsActive
            };
        }
    }
}