using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Application.Wrappers;
using E_Agendamento.Domain.Entities;
using MediatR;

namespace E_Agendamento.Application.Features.Schedules.Queries.GetSchedulesByUser
{
    public class GetSchedulesByUserQuery : IRequest<Response<IEnumerable<GetSchedulesByUserViewModel>>>
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

    public class GetSchedulesByUserQueryHandler : IRequestHandler<GetSchedulesByUserQuery, Response<IEnumerable<GetSchedulesByUserViewModel>>>
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IUserRepository _userRepository;

        public GetSchedulesByUserQueryHandler(IScheduleRepository scheduleRepository, IUserRepository userRepository)
        {
            _scheduleRepository = scheduleRepository;
            _userRepository = userRepository;
        }

        public async Task<Response<IEnumerable<GetSchedulesByUserViewModel>>> Handle(GetSchedulesByUserQuery request, CancellationToken cancellationToken)
        {
            // todo => adicionar validação de usuário existente

            Console.WriteLine(request.RequestedBy);

            IEnumerable<Schedule> schedules = await _scheduleRepository.GetByUserAsync(request.RequestedBy);
            var response = GetSchedulesByUserViewModel.Map(schedules);
            return new("Agendamentos recuperados com sucesso", response);
        }
    }
}
