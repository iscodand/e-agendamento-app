using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace E_Agendamento.Domain.Entities
{
    public class UsersRoles : IdentityUserRole<string>
    {
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        [ForeignKey(nameof(RoleId))]
        public ApplicationRole Role { get; set; }
    }
}