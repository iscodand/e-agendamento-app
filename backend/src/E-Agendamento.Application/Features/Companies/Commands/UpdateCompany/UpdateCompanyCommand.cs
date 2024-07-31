using System.Text.Json.Serialization;
using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Application.Exceptions;
using E_Agendamento.Application.Features.Companies.Queries.GetCompanyById;
using E_Agendamento.Application.Wrappers;
using E_Agendamento.Domain.Entities;
using MediatR;

namespace E_Agendamento.Application.Features.Companies.Commands.UpdateCompany
{
    public class UpdateCompanyCommand : IRequest<Response<GetCompanyViewModel>>
    {
        [JsonIgnore]
        public string Id { get; set; }
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public static Company Map(Company company, UpdateCompanyCommand command)
        {
            company.Name = command.Name;
            company.CNPJ = command.CNPJ;
            company.Description = command.Description;
            company.IsActive = command.IsActive;
            return company;
        }
    }

    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, Response<GetCompanyViewModel>>
    {
        private readonly ICompanyRepository _companyRepository;

        public UpdateCompanyCommandHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<Response<GetCompanyViewModel>> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            Company company = await _companyRepository.GetByIdAsync(request.Id);
            if (company is null)
            {
                throw new KeyNotFoundException();
            }

            bool cnpjAlreadyRegistered = await _companyRepository.ExistsByCNPJAsync(request.CNPJ, request.Id);
            if (cnpjAlreadyRegistered)
            {
                throw new ValidationException([new("CNPJ", "Esse CNPJ já foi cadastrado. Verifique e tente novamente.")]);
            }

            company = UpdateCompanyCommand.Map(company, request);
            await _companyRepository.UpdateAsync(company);

            GetCompanyViewModel viewModel = GetCompanyViewModel.Map(company);

            return new("Empresa atualizada com sucesso", viewModel);
        }
    }
}