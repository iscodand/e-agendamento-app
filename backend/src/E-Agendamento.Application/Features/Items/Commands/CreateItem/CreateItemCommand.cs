using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Application.Exceptions;
using E_Agendamento.Application.Wrappers;
using E_Agendamento.Domain.Entities;
using MediatR;

namespace E_Agendamento.Application.Features.Items.Commands
{
    public partial class CreateItemCommand : IRequest<Response<CreateItemCommand>>
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

    public class CreateItemHandler : IRequestHandler<CreateItemCommand, Response<CreateItemCommand>>
    {
        private readonly IItemRepository _itemRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CreateItemHandler(IItemRepository itemRepository, ICategoryRepository categoryRepository)
        {
            _itemRepository = itemRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<Response<CreateItemCommand>> Handle(CreateItemCommand command, CancellationToken cancellationToken)
        {
            bool itemAlreadyRegistered = await _itemRepository.IsUniqueAsync(command.Name, command.CompanyId, cancellationToken);
            if (itemAlreadyRegistered)
            {
                throw new KeyNotFoundException();
            }

            bool categoryExists = await _categoryRepository.ExistsByIdAsync(command.CategoryId, command.CompanyId, cancellationToken);
            if (categoryExists == false)
            {
                throw new ValidationException([new("CategoryId", "Categoria não encontrada. Verifique e tente novamente.")]);
            }

            Item item = CreateItemCommand.Map(command);
            await _itemRepository.CreateAsync(item);

            return new Response<CreateItemCommand>("Item cadastrado com sucesso.", command);
        }
    }
}