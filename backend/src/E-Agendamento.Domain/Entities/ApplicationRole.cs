using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Agendamento.Domain.Entities
{
    public class ApplicationRole : IdentityRole<string>
    {
        public string Description { get; set; }
    }
}
