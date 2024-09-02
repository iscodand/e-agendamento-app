using E_Agendamento.Domain.Entities;

namespace E_Agendamento.Application.Features.Employees.Queries.SearchByEmployee
{
    public class SearchByEmployeeViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool AlreadyInCompany { get; set; }

        public static IEnumerable<SearchByEmployeeViewModel> Map(IEnumerable<ApplicationUser> users, string companyId)
        {
            return users.Select(x => new SearchByEmployeeViewModel
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                IsActive = x.IsActive,
                AlreadyInCompany = x.UsersCompanies.Select(x => x.CompanyId).Contains(companyId)
            }).ToList();
        }
    }
}