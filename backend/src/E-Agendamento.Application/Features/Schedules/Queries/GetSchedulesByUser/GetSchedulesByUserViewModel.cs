using E_Agendamento.Domain.Entities;

namespace E_Agendamento.Application.Features.Schedules.Queries.GetSchedulesByUser
{
    public class GetSchedulesByUserViewModel
    {
        public string Id { get; set; }
        public string ItemId { get; set; }
        public string Observation { get; set; }
        public string Status { get; set; }

        // must be UserResponse
        public string RequestedBy { get; set; }

        public string ConfirmedBy { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime EndAt { get; set; }
        public string CompanyId { get; set; }

        public static IEnumerable<GetSchedulesByUserViewModel> Map(IEnumerable<Schedule> schedules)
        {
            return schedules.Select(x => new GetSchedulesByUserViewModel
            {
                Id = x.Id,
                ItemId = x.ItemId,
                Observation = x.Observation,
                Status = x.Status,
                RequestedBy = x.RequestedById,
                ConfirmedBy = x.ConfirmedById,
                StartedAt = x.StartAt,
                EndAt = x.EndAt,
                CompanyId = x.CompanyId
            });
        }
    }
}