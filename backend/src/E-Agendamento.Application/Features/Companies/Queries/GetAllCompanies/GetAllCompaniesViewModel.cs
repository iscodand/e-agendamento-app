using E_Agendamento.Domain.Entities;

namespace E_Agendamento.Application.Features.Companies.Queries.GetAllCompanies
{
    public class GetAllCompaniesViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CNPJ { get; set; }
        public bool IsActive { get; set; }

        public static IEnumerable<GetAllCompaniesViewModel> Map(IEnumerable<Company> companies)
        {
            return companies.Select(x => new GetAllCompaniesViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                CNPJ = x.CNPJ,
                IsActive = x.IsActive
            }).ToList();
        }
    }
}