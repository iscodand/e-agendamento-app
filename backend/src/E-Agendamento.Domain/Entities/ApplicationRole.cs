using Microsoft.AspNetCore.Identity;

namespace E_Agendamento.Domain.Entities
{
    public class ApplicationRole : IdentityRole<string>
    {
        public string Description { get; set; }

        public ApplicationRole()
        {
            Id = Guid.NewGuid().ToString();
        }

        public static ApplicationRole Create(string name, string description)
        {
            return new()
            {
                Name = name,
                NormalizedName = name.Trim().ToUpper(),
                Description = description,
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
        }
    }
}
