using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Application.Features.Items.Common;
using E_Agendamento.Application.Features.Schedules.Queries.Common;
using E_Agendamento.Application.Wrappers;
using E_Agendamento.Domain.Entities;
using MediatR;

namespace E_Agendamento.Application.Features.Schedules.Queries.GetSchedulesByUser
{
    public class GetSchedulesByUserQuery : IRequest<Response<IEnumerable<ScheduleViewModel>>>
    {
        public string RequestedBy { get; set; }
        public string Status { get; set; } = "";
    }

    public class GetSchedulesByUserQueryHandler : IRequestHandler<GetSchedulesByUserQuery, Response<IEnumerable<ScheduleViewModel>>>
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IUserRepository _userRepository;

        public GetSchedulesByUserQueryHandler(IScheduleRepository scheduleRepository, IUserRepository userRepository)
        {
            _scheduleRepository = scheduleRepository;
            _userRepository = userRepository;
        }

        public async Task<Response<IEnumerable<ScheduleViewModel>>> Handle(GetSchedulesByUserQuery request, CancellationToken cancellationToken)
        {
            // todo => adicionar validação de usuário existente
            IEnumerable<Schedule> schedules;

            if (request.Status.ToUpper() == "ALL")
            {
                schedules = await _scheduleRepository.GetByUserAsync(request.RequestedBy);
            }
            else
            {
                schedules = await _scheduleRepository.GetByUserWithStatusAsync(request.RequestedBy, request.Status);
            }

            var response = ScheduleViewModel.Map(schedules);

            return new("Agendamentos recuperados com sucesso", response);
        }
    }
}
