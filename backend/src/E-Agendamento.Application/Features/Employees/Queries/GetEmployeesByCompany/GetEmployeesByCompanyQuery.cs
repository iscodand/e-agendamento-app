using System.Text.Json.Serialization;
using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Application.DTOs.Account;
using E_Agendamento.Application.Exceptions;
using E_Agendamento.Application.Wrappers;
using E_Agendamento.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace E_Agendamento.Application.Features.Employees.Queries.GetEmployeesByCompany
{
    public class GetEmployeesByCompanyQuery : IRequest<Response<IEnumerable<GetEmployeesByQueryViewModel>>>
    {
        [JsonIgnore]
        public string CompanyId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetEmployeesByCompanyQueryHandler : IRequestHandler<GetEmployeesByCompanyQuery, Response<IEnumerable<GetEmployeesByQueryViewModel>>>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public GetEmployeesByCompanyQueryHandler(IUserRepository userRepository, ICompanyRepository companyRepository)
        {
            _userRepository = userRepository;
            _companyRepository = companyRepository;
        }

        public async Task<Response<IEnumerable<GetEmployeesByQueryViewModel>>> Handle(GetEmployeesByCompanyQuery request, CancellationToken cancellationToken)
        {
            Company company = await _companyRepository.GetByIdAsync(request.CompanyId);
            if (company is null)
            {
                throw new ValidationException([new("CompanyId", "Empresa não encontrada. Verifique e tente novamente.")]);
            }

            IEnumerable<RetrieveUserResponse> users = await _userRepository.GetUsersByCompanyAsync(request.CompanyId);

            IEnumerable<GetEmployeesByQueryViewModel> response = GetEmployeesByQueryViewModel.Map(users);

            return new("Usuários recuperados com sucesso.", response);
        }
    }
}