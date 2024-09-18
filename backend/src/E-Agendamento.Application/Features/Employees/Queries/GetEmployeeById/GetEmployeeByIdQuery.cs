using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Application.Features.Employees.Common;
using E_Agendamento.Application.Wrappers;
using E_Agendamento.Domain.Entities;
using MediatR;

namespace E_Agendamento.Application.Features.Employees.Queries.GetEmployeeById
{
    public class GetEmployeeByIdQuery : IRequest<Response<DetailEmployeeViewModel>>
    {
        public string Id { get; set; }
    }

    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, Response<DetailEmployeeViewModel>>
    {
        private readonly IUserRepository _userRepository;

        public GetEmployeeByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Response<DetailEmployeeViewModel>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            ApplicationUser user = await _userRepository.GetWithRolesAndCompaniesAsync(request.Id);
            if (user is null)
            {
                throw new KeyNotFoundException("Usuário não encontrado");
            }

            List<string> roles = user.UsersRoles.Select(x => x.RoleId).ToList();
            List<string> companies = user.UsersCompanies.Select(x => x.CompanyId).ToList();

            DetailEmployeeViewModel viewModel = DetailEmployeeViewModel.Map(user, roles, companies);

            return new("Usuário recuperado com sucesso", viewModel);
        }
    }
}
