using System.ComponentModel.DataAnnotations.Schema;
using E_Agendamento.Domain.Entities.Common;

namespace E_Agendamento.Domain.Entities
{
    public class UsersCompanies : AuditableEntity
    {
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        public string CompanyId { get; set; }

        public static UsersCompanies Create(string userId, string companyId)
        {
            return new()
            {
                UserId = userId,
                CompanyId = companyId
            };
        }
    }
}