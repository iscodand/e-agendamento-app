using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Application.Exceptions;
using E_Agendamento.Application.Wrappers;
using E_Agendamento.Domain.Entities;
using MediatR;

namespace E_Agendamento.Application.Features.Items.Commands.UpdateItemById
{
    public class UpdateItemCommand : IRequest<Response<UpdateItemCommand>>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public int TotalQuantity { get; set; }
        public bool IsAvailable { get; set; }
        public int QuantityAvailable { get; set; }
        public string CompanyId { get; set; }

        public static Item Map(Item item, UpdateItemCommand command)
        {
            item.Name = command.Name;
            item.Description = command.Description;
            item.TotalQuantity = command.TotalQuantity;
            item.CategoryId = command.CategoryId;
            item.QuantityAvailable = command.QuantityAvailable;
            item.IsAvailable = command.IsAvailable;
            return item;
        }
    }

    public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, Response<UpdateItemCommand>>
    {
        private readonly IItemRepository _itemRepository;
        private readonly ICategoryRepository _categoryRepository;

        public UpdateItemCommandHandler(IItemRepository itemRepository, ICategoryRepository categoryRepository)
        {
            _itemRepository = itemRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<Response<UpdateItemCommand>> Handle(UpdateItemCommand command, CancellationToken cancellationToken)
        {
            Item item = await _itemRepository.GetByIdAsync(command.Id);
            if (item is null)
            {
                throw new KeyNotFoundException();
            }

            bool itemAlreadyRegistered = await _itemRepository.IsUniqueAsync(command.Name, command.CompanyId, cancellationToken, command.Id);
            if (itemAlreadyRegistered)
            {
                throw new ValidationException([new("Name", "Um item com esse nome já foi cadastrado. Verifique e tente novamente.")]);
            }

            bool categoryExists = await _categoryRepository.ExistsByIdAsync(command.CategoryId, command.CompanyId, cancellationToken);
            if (categoryExists == false)
            {
                throw new ValidationException([new("CategoryId", "Categoria não encontrada.")]);
            }

            item = UpdateItemCommand.Map(item, command);
            await _itemRepository.UpdateAsync(item);

            return new Response<UpdateItemCommand>("Item atualizado com sucesso.", command);
        }
    }
}