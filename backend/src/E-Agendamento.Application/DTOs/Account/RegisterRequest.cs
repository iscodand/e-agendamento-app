using E_Agendamento.Domain.Entities;

namespace E_Agendamento.Application.DTOs.Account
{
    public class RegisterRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public List<string> Roles { get; set; }
        public string CompanyId { get; set; }

        public static ApplicationUser Map(RegisterRequest request)
        {
            return new()
            {
                FullName = request.FullName,
                Email = request.Email,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber
            };
        }
    }
}