using E_Agendamento.Domain.Entities.Common;

namespace E_Agendamento.Domain.Entities
{
    public class Schedule : Entity
    {
        public string ItemId { get; set; }
        public Item Item { get; set; }

        public string Status { get; set; }
        public string Observation { get; set; }

        public string RequestedById { get; set; }
        public ApplicationUser RequestedBy { get; set; }

        public string ConfirmedById { get; set; }
        public ApplicationUser ConfirmedBy { get; set; }

        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public Company Company { get; set; }
        public string CompanyId { get; set; }

        public Schedule()
        {
            StartAt = DateTime.Now;
            EndAt = DateTime.MinValue;
            ConfirmedById = null;
        }
    }
}