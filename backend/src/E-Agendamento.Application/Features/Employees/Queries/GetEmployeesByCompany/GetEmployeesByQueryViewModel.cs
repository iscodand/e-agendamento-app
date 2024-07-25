using E_Agendamento.Domain.Entities;

namespace E_Agendamento.Application.Features.Employees.Queries.GetEmployeesByCompany
{
    public class GetEmployeesByQueryViewModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        // public string Roles { get; set; }

        public static IEnumerable<GetEmployeesByQueryViewModel> Map(IEnumerable<ApplicationUser> users)
        {
            return users.Select(x => new GetEmployeesByQueryViewModel
            {
                FullName = x.FullName,
                Email = x.Email,
                IsActive = x.IsActive
            }).ToList();
        }
    }
}