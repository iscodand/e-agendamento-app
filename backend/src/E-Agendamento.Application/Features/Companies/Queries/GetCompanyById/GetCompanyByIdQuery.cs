using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Application.Wrappers;
using E_Agendamento.Domain.Entities;
using MediatR;

namespace E_Agendamento.Application.Features.Companies.Queries.GetCompanyById
{
    public class GetCompanyByIdQuery : IRequest<Response<GetCompanyViewModel>>
    {
        public string Id { get; set; }
    }

    public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, Response<GetCompanyViewModel>>
    {
        private readonly ICompanyRepository _companyRepository;

        public GetCompanyByIdQueryHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<Response<GetCompanyViewModel>> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
        {
            Company company = await _companyRepository.GetByIdAsync(request.Id);

            if (company is null)
            {
                throw new KeyNotFoundException();
            }

            GetCompanyViewModel viewModel = GetCompanyViewModel.Map(company);
            return new("Empresa recuperada com sucesso", viewModel);
        }
    }
}