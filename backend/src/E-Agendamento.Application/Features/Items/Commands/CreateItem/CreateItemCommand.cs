using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Application.Wrappers;
using E_Agendamento.Domain.Entities;
using MediatR;

namespace E_Agendamento.Application.Features.Items.Commands
{
    public partial class CreateItemCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public int TotalQuantity { get; set; }
        public int QuantityAvailable { get; set; }
        public string CompanyId { get; set; }

        public static Item Map(CreateItemCommand command)
        {
            return new()
            {
                Name = command.Name,
                Description = command.Description,
                CategoryId = command.CategoryId,
                TotalQuantity = command.TotalQuantity,
                QuantityAvailable = command.QuantityAvailable,
                CompanyId = command.CompanyId
            };
        }
    }

    public class CreateItemHandler : IRequestHandler<CreateItemCommand, Response<int>>
    {
        private readonly IItemRepository _itemRepository;

        public CreateItemHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<Response<int>> Handle(CreateItemCommand command, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}