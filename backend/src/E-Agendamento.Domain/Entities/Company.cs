using E_Agendamento.Domain.Entities.Common;

namespace E_Agendamento.Domain.Entities
{
    public class Company : Entity
    {
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public ICollection<ApplicationUser> Users { get; set; } = [];
        public ICollection<Item> Items { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
    }
}