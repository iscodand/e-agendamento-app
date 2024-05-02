using E_Agendamento.Domain.Entities.Common;

namespace E_Agendamento.Domain.Entities
{
    public class Category : AuditableEntity
    {
        public string Description { get; set; }
        public Company Company { get; set; }

        public ICollection<Item> Items { get; set; }
    }
}