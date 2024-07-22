using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Application.Wrappers;
using E_Agendamento.Domain.Entities;
using MediatR;

namespace E_Agendamento.Application.Features.Companies.Queries.GetAllCompanies
{
    public class GetAllCompaniesQuery : IRequest<PagedResponse<IEnumerable<GetAllCompaniesViewModel>>>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }

    public class GetAllCompaniesQueryHandler : IRequestHandler<GetAllCompaniesQuery, PagedResponse<IEnumerable<GetAllCompaniesViewModel>>>
    {
        private readonly ICompanyRepository _companyRepository;

        public GetAllCompaniesQueryHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<PagedResponse<IEnumerable<GetAllCompaniesViewModel>>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
        {
            GetAllCompaniesParameter validFilter = GetAllCompaniesParameter.Map(new Parameters.RequestParameter(
                request.PageSize,
                request.PageNumber
            ));

            IEnumerable<Company> companies = await _companyRepository.GetPagedResponseAsync(validFilter.PageNumber, validFilter.PageSize);
            int totalCompaniesCount = await _companyRepository.GetTotalItemsCountAsync();
            IEnumerable<GetAllCompaniesViewModel> viewModel = GetAllCompaniesViewModel.Map(companies);

            return new(viewModel, validFilter.PageNumber, validFilter.PageSize, totalCompaniesCount);
        }
    }
}