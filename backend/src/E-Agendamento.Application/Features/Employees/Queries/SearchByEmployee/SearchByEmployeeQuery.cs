using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Application.Wrappers;
using E_Agendamento.Domain.Entities;
using MediatR;

namespace E_Agendamento.Application.Features.Employees.Queries.SearchByEmployee
{
    public class SearchByEmployeeQuery : IRequest<Response<IEnumerable<SearchByEmployeeViewModel>>>
    {
        public string CompanyId { get; set; }
        public string SearchTerm { get; set; }
    }

    public class SearchByEmployeeQueryHandler : IRequestHandler<SearchByEmployeeQuery, Response<IEnumerable<SearchByEmployeeViewModel>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;

        public SearchByEmployeeQueryHandler(IUserRepository userRepository, ICompanyRepository companyRepository)
        {
            _userRepository = userRepository;
            _companyRepository = companyRepository;
        }

        public async Task<Response<IEnumerable<SearchByEmployeeViewModel>>> Handle(SearchByEmployeeQuery request, CancellationToken cancellationToken)
        {
            // Company company = await _companyRepository.GetByIdAsync(request.CompanyId);
            // if (company is null)
            // {
            //     throw new KeyNotFoundException();
            // }

            IEnumerable<ApplicationUser> users = await _userRepository.SearchByTermAsync(request.SearchTerm);
            IEnumerable<SearchByEmployeeViewModel> response = SearchByEmployeeViewModel.Map(users, request.CompanyId);

            return new("Usuários recuperados com sucesso.", response);
        }
    }
}