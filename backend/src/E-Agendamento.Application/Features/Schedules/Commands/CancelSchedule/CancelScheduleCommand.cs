using System.Text.Json.Serialization;
using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Application.Features.Schedules.Queries.Common;
using E_Agendamento.Application.Wrappers;
using E_Agendamento.Domain.Entities;
using MediatR;

namespace E_Agendamento.Application.Features.Schedules.Commands.CancelSchedule
{
    public class CancelScheduleCommand : IRequest<Response<ScheduleViewModel>>
    {
        public string Id { get; set; }

        [JsonIgnore]
        public string CompanyId { get; set; }

        [JsonIgnore]
        public string CanceledBy { get; set; }
    }

    public class CancelScheduleCommandHandler : IRequestHandler<CancelScheduleCommand, Response<ScheduleViewModel>>
    {
        private readonly IScheduleRepository _scheduleRepository;

        public CancelScheduleCommandHandler(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public async Task<Response<ScheduleViewModel>> Handle(CancelScheduleCommand request, CancellationToken cancellationToken)
        {
            Schedule schedule = await _scheduleRepository.GetByIdAsync(request.Id);
            if (schedule is null)
            {
                throw new KeyNotFoundException("Agendamento não encontrado.");
            }

            schedule = schedule.CancelSchedule();
            await _scheduleRepository.UpdateAsync(schedule);
            ScheduleViewModel viewModel = ScheduleViewModel.Map(schedule);

            return new("Agendamento cancelado com sucesso.", viewModel);
        }
    }
}