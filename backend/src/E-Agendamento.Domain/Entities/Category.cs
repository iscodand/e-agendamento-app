using System.ComponentModel.DataAnnotations.Schema;
using E_Agendamento.Domain.Entities.Common;

namespace E_Agendamento.Domain.Entities
{
    public class Category : AuditableEntity
    {
        public string Description { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        public string CompanyId { get; set; }

        public ICollection<Item> Items { get; set; }
    }
}