using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Application.Wrappers;
using E_Agendamento.Domain.Entities;
using MediatR;

namespace E_Agendamento.Application.Features.Items.Queries.GetAllItems
{
    public class GetAllItemsQuery : IRequest<PagedResponse<IEnumerable<GetAllItemsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetAllItemsQueryHandler : IRequestHandler<GetAllItemsQuery, PagedResponse<IEnumerable<GetAllItemsViewModel>>>
    {
        private readonly IItemRepository _itemRepository;

        public GetAllItemsQueryHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<PagedResponse<IEnumerable<GetAllItemsViewModel>>> Handle(GetAllItemsQuery request, CancellationToken cancellationToken)
        {
            // TODO: verificar se esse mapeamento está correto
            GetAllItemsParameter validFilter = GetAllItemsParameter.Map(new Parameters.RequestParameter(
                request.PageSize,
                request.PageNumber
            ));

            // TODO:
            //    1. fazer uma query com filtro por empresa (CompanyId)
            //    2. fazer uma query com filtro por usuario (CreatedBy)
            IEnumerable<Item> items = await _itemRepository.GetPagedResponseAsync(validFilter.PageNumber, validFilter.PageSize);
            IEnumerable<GetAllItemsViewModel> viewModel = GetAllItemsViewModel.Map(items);

            return new(viewModel, validFilter.PageNumber, validFilter.PageSize);
        }
    }
}