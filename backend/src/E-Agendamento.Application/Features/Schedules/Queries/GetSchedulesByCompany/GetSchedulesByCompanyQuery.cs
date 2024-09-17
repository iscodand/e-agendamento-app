using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Application.Exceptions;
using E_Agendamento.Application.Features.Schedules.Queries.Common;
using E_Agendamento.Application.Wrappers;
using E_Agendamento.Domain.Entities;
using MediatR;

namespace E_Agendamento.Application.Features.Schedules.Queries.GetSchedulesByCompany
{
    public class GetSchedulesByCompanyQuery : IRequest<Response<IEnumerable<ScheduleViewModel>>>
    {
        public string CompanyId { get; set; }
    }

    public class GetSchedulesByCompanyQueryHandler : IRequestHandler<GetSchedulesByCompanyQuery, Response<IEnumerable<ScheduleViewModel>>>
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly ICompanyRepository _companyRepository;

        public GetSchedulesByCompanyQueryHandler(IScheduleRepository scheduleRepository, ICompanyRepository companyRepository)
        {
            _scheduleRepository = scheduleRepository;
            _companyRepository = companyRepository;
        }

        public async Task<Response<IEnumerable<ScheduleViewModel>>> Handle(GetSchedulesByCompanyQuery request, CancellationToken cancellationToken)
        {
            Company company = await _companyRepository.GetByIdAsync(request.CompanyId);
            if (company is null)
            {
                throw new ValidationException([new("CompanyId", "Empresa não encontrada. Verifique e tente novamente")]);
            }

            IEnumerable<Schedule> schedules = await _scheduleRepository.GetByCompanyAsync(request.CompanyId);
            IEnumerable<ScheduleViewModel> viewModel = ScheduleViewModel.Map(schedules);

            return new("Agendamentos recuperados com sucesso.", viewModel);
        }
    }
}