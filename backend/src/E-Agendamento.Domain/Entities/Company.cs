using System.ComponentModel.DataAnnotations.Schema;
using E_Agendamento.Domain.Entities.Common;

namespace E_Agendamento.Domain.Entities
{
    public class Company : Entity
    {
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        // TODO => implement auditable entity

        public ICollection<UsersCompanies> UsersCompanies { get; set; }
        public ICollection<Item> Items { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Schedule> Schedules { get; set; }

        public Company()
        {
            IsActive = true;
        }
    }
}