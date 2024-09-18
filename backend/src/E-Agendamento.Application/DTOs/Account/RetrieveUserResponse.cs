using E_Agendamento.Domain.Entities;

namespace E_Agendamento.Application.DTOs.Account
{
    public class RetrieveUserResponse
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public ICollection<string> Roles { get; set; }
        public ICollection<string> Companies { get; set; }

        public static RetrieveUserResponse Map(ApplicationUser user)
        {
            return new()
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                Roles = [],
                Companies = []
            };
        }
    }
}