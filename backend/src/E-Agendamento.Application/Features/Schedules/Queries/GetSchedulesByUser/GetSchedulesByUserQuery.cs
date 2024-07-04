namespace E_Agendamento.Application.Features.Schedules.Queries.GetSchedulesByUser
{
    public class GetSchedulesByUserQuery
    {
        public string Id { get; set; }
        public string ItemId { get; set; }
        public string Observation { get; set; }
        public string Status { get; set; }
        public string RequestedBy { get; set; }
        public string ConfirmedBy { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime EndAt { get; set; }
        public string CompanyId { get; set; }
    }
}
