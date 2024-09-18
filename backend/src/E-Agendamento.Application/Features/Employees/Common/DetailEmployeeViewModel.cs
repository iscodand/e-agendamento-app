using E_Agendamento.Domain.Entities;

namespace E_Agendamento.Application.Features.Employees.Common
{
    public class DetailEmployeeViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public List<string> Roles { get; set; }
        public List<string> Companies { get; set; }

        public static DetailEmployeeViewModel Map(ApplicationUser employee, List<string> roles, List<string> companies)
        {
            return new()
            {
                Id = employee.Id,
                UserName = employee.UserName,
                FullName = employee.FullName,
                Roles = roles,
                Companies = companies
            };
        }
    }
}