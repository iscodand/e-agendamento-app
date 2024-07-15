using System.Security.Cryptography.X509Certificates;
using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Application.Exceptions;
using E_Agendamento.Application.Features.Companies.Queries.GetAllCompanies;
using E_Agendamento.Application.Features.Companies.Queries.GetCompanyById;
using E_Agendamento.Application.Wrappers;
using E_Agendamento.Domain.Entities;
using MediatR;

namespace E_Agendamento.Application.Features.Companies.Commands.CreateCompany
{
    public class CreateCompanyCommand : IRequest<Response<GetCompanyViewModel>>
    {
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public string Description { get; set; }

        public static Company Map(CreateCompanyCommand command)
        {
            return new()
            {
                Name = command.Name,
                CNPJ = command.CNPJ,
                Description = command.Description
            };
        }
    }

    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, Response<GetCompanyViewModel>>
    {
        private readonly ICompanyRepository _companyRepository;

        public CreateCompanyCommandHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<Response<GetCompanyViewModel>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            bool companyAlreadyExists = await _companyRepository.ExistsByCNPJAsync(request.CNPJ);
            if (companyAlreadyExists)
            {
                throw new ValidationException([new("CNPJ", "Uma empresa já está cadastrada com esse CNPJ.")]);
            }

            Company company = CreateCompanyCommand.Map(request);
            Company newCompany = await _companyRepository.CreateAsync(company);

            GetCompanyViewModel response = GetCompanyViewModel.Map(newCompany);

            return new("Empresa cadastrada com sucesso.", response);
        }
    }
}