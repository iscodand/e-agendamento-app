using System.Text.Json.Serialization;
using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Application.Exceptions;
using E_Agendamento.Application.Wrappers;
using E_Agendamento.Domain.Entities;
using MediatR;

namespace E_Agendamento.Application.Features.Companies.Commands.AddUserToCompany
{
    public class AddUserToCompanyCommand : IRequest<Response<string>>
    {
        public string UserId { get; set; }

        [JsonIgnore]
        public string CompanyId { get; set; }
    }

    public class AddUserToCompanyCommandHandler : IRequestHandler<AddUserToCompanyCommand, Response<string>>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;

        public AddUserToCompanyCommandHandler(ICompanyRepository companyRepository, IUserRepository userRepository)
        {
            _companyRepository = companyRepository;
            _userRepository = userRepository;
        }

        public async Task<Response<string>> Handle(AddUserToCompanyCommand request, CancellationToken cancellationToken)
        {
            ApplicationUser user = await _userRepository.GetWithCompaniesAsync(request.UserId);
            if (user is null)
            {
                throw new ValidationException([new("UserId", "Usuário não encontrado.")]);
            }

            Company company = await _companyRepository.GetByIdAsync(request.CompanyId);
            if (company is null)
            {
                throw new ValidationException([new("CompanyId", "Empresa não encontrada.")]);
            }

            if (!user.IsActive)
            {
                throw new ValidationException([new("UserId", "O usuário está inativo, portanto não pode ser adicionado à uma empresa.")]);
            }

            if (!company.IsActive)
            {
                throw new ValidationException([new("CompanyId", "Você não pode inserir um usuário em uma empresa inativa no sistema.")]);
            }

            // bool userAlreadyInCompany = user.Companies.Select(x => x.Id).Contains(request.CompanyId);
            // if (userAlreadyInCompany)
            // {
            //     throw new ValidationException([new("CompanyId", "O usuário já pertence a essa empresa.")]);
            // }

            await _companyRepository.AddUserToCompanyAsync(company, user);

            return new("Usuário adicionado com sucesso.");
        }
    }
}