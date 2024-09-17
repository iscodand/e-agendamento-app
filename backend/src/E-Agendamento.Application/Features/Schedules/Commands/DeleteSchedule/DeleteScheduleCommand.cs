using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Application.Exceptions;
using E_Agendamento.Application.Features.Schedules.Queries.Common;
using E_Agendamento.Application.Wrappers;
using E_Agendamento.Domain.Entities;
using MediatR;

namespace E_Agendamento.Application.Features.Schedules.Commands.DeleteSchedule
{
    public class DeleteScheduleCommand : IRequest<Response<string>>
    {
        public string ScheduleId { get; set; }
        public string CompanyId { get; set; }
    }

    public class DeleteScheduleCommandHandler : IRequestHandler<DeleteScheduleCommand, Response<string>>
    {
        private readonly IScheduleRepository _scheduleRepository;

        public DeleteScheduleCommandHandler(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public async Task<Response<string>> Handle(DeleteScheduleCommand request, CancellationToken cancellationToken)
        {
            Schedule schedule = await _scheduleRepository.GetByIdAsync(request.ScheduleId);
            if (schedule is null)
            {
                throw new ValidationException([new("ScheduleId", "Agendamento não encontrado.")]);
            }

            if (request.CompanyId != schedule.CompanyId)
            {
                throw new ValidationException([new("Schedule", "Você não tem permissão para deletar esse agendamento.")]);
            }

            await _scheduleRepository.DeleteAsync(schedule);

            return new("Agendamento deletado com sucesso.", "");
        }
    }
}