using E_Agendamento.Application.DTOs.Account;

namespace E_Agendamento.Application.Features.Employees.Queries.GetEmployeesByCompany
{
    public class GetEmployeesByQueryViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public IEnumerable<string> Companies { get; set; }

        public static IEnumerable<GetEmployeesByQueryViewModel> Map(IEnumerable<RetrieveUserResponse> users)
        {
            return users.Select(x => new GetEmployeesByQueryViewModel
            {
                Id = x.Id,
                FullName = x.FullName,
                UserName = x.UserName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                IsActive = x.IsActive,
                Roles = x.Roles,
                Companies = x.Companies
            }).ToList();
        }
    }
}