using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Application.Exceptions;
using E_Agendamento.Application.Features.Items.Queries.Common;
using E_Agendamento.Application.Wrappers;
using E_Agendamento.Domain.Entities;
using MediatR;

namespace E_Agendamento.Application.Features.Items.Queries.SearchByItem
{
    public class SearchByItemQuery : IRequest<Response<IEnumerable<GetItemViewModel>>>
    {
        public string CompanyId { get; set; }
        public string SearchTerm { get; set; }
    }

    public class SearchByItemQueryHandler : IRequestHandler<SearchByItemQuery, Response<IEnumerable<GetItemViewModel>>>
    {
        private readonly IItemRepository _itemRepository;
        private readonly ICompanyRepository _companyRepository;

        public SearchByItemQueryHandler(IItemRepository itemRepository, ICompanyRepository companyRepository)
        {
            _itemRepository = itemRepository;
            _companyRepository = companyRepository;
        }

        public async Task<Response<IEnumerable<GetItemViewModel>>> Handle(SearchByItemQuery request, CancellationToken cancellationToken)
        {
            Company company = await _companyRepository.GetByIdAsync(request.CompanyId);
            if (company is null)
            {
                throw new ValidationException([new("CompanyId", "Empresa não encontrada.")]);
            }

            IEnumerable<Item> items = await _itemRepository.SearchByItemAsync(request.SearchTerm, request.CompanyId);
            var itemsMapped = GetItemViewModel.Map(items);

            return new("Itens encontrados com sucesso.", itemsMapped);
        }
    }
}