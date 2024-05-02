using E_Agendamento.Domain.Entities.Common;

namespace E_Agendamento.Domain.Entities
{
    public class Schedule : Entity
    {
        public Item Item { get; set; }
        public string Status { get; set; }
        public string Observation { get; set; }
        public ApplicationUser RequestedBy { get; set; }
        public ApplicationUser ConfirmedBy { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public Company Company { get; set; }
    }
}