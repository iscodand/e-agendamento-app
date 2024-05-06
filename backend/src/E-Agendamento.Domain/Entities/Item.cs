using System.ComponentModel.DataAnnotations.Schema;
using E_Agendamento.Domain.Entities.Common;

namespace E_Agendamento.Domain.Entities
{
    public class Item : AuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }
        public string CategoryId { get; set; }

        public int TotalQuantity { get; set; }
        public int QuantityAvailable { get; set; }
        public bool IsAvailable { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        public string CompanyId { get; set; }
    }
}