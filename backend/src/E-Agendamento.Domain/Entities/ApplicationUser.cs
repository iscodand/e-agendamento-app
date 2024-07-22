using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Agendamento.Domain.Entities
{
    public class ApplicationUser : IdentityUser<string>
    {
        public string FullName { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Company> Companies { get; set; } = [];
        public ICollection<ApplicationRole> Roles { get; set; } = [];
        // public ICollection<Schedule> Schedules { get; set; }

        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString();
            IsActive = true;
        }
    }
}
