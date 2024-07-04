using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Application.Wrappers;
using E_Agendamento.Domain.Entities;
using MediatR;

namespace E_Agendamento.Application.Features.Items.Commands.DeleteItemById
{
    public class DeleteItemCommand : IRequest<Response<string>>
    {
        public string Id { get; set; }
    }

    public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, Response<string>>
    {
        private readonly IItemRepository _itemRepository;

        public DeleteItemCommandHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<Response<string>> Handle(DeleteItemCommand command, CancellationToken cancellationToken)
        {
            Item item = await _itemRepository.GetByIdAsync(command.Id);
            if (item is null)
            {
                throw new KeyNotFoundException();
            }

            await _itemRepository.DeleteAsync(item);

            return new Response<string>("Item deletado com sucesso.", item.Id);
        }
    }
}