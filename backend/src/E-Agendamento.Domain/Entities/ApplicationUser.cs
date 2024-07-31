using Microsoft.AspNetCore.Identity;

namespace E_Agendamento.Domain.Entities
{
    public class ApplicationUser : IdentityUser<string>
    {
        public string FullName { get; set; }
        public bool IsActive { get; set; }

        // TODO => refresh token implementation
        // public string RefreshToken { get; set; }
        // public DateTime RefreshTokenValidityTime { get; set; }

        public ICollection<UsersRoles> UsersRoles { get; set; }
        public ICollection<UsersCompanies> UsersCompanies { get; set; }
        // public ICollection<ApplicationRole> Roles { get; set; } = [];
        // public ICollection<Schedule> Schedules { get; set; }

        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString();
            IsActive = true;
        }
    }
}
