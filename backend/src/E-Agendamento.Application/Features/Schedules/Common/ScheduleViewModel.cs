using E_Agendamento.Application.DTOs.Account;
using E_Agendamento.Application.Features.Items.Common;
using E_Agendamento.Domain.Entities;

namespace E_Agendamento.Application.Features.Schedules.Queries.Common
{
    public class ScheduleViewModel
    {
        public string Id { get; set; }
        public ItemViewModel Item { get; set; }
        public string ItemId { get; set; }
        public string Observation { get; set; }
        public string Status { get; set; }
        public RetrieveUserResponse RequestedBy { get; set; }
        public string ConfirmedBy { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime EndAt { get; set; }
        public string CompanyId { get; set; }

        public static IEnumerable<ScheduleViewModel> Map(IEnumerable<Schedule> schedules)
        {
            return schedules.Select(x => new ScheduleViewModel
            {
                Id = x.Id,
                ItemId = x.ItemId,
                Item = ItemViewModel.Map(x.Item),
                Observation = x.Observation,
                Status = x.Status,
                RequestedBy = RetrieveUserResponse.Map(x.RequestedBy),
                ConfirmedBy = x.ConfirmedById,
                StartedAt = x.StartAt,
                EndAt = x.EndAt,
                CompanyId = x.CompanyId
            });
        }

        public static ScheduleViewModel Map(Schedule schedule)
        {
            return new()
            {
                Id = schedule.Id,
                ItemId = schedule.ItemId,
                Item = null,
                Observation = schedule.Observation,
                Status = schedule.Status,
                RequestedBy = schedule.RequestedBy is not null ? RetrieveUserResponse.Map(schedule.RequestedBy) : null,
                ConfirmedBy = schedule.ConfirmedById,
                StartedAt = schedule.StartAt,
                EndAt = schedule.EndAt,
                CompanyId = schedule.CompanyId
            };
        }
    }
}