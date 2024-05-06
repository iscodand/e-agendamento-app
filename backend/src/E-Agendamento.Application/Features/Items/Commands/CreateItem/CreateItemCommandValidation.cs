using E_Agendamento.Application.Contracts.Repositories;
using FluentValidation;

namespace E_Agendamento.Application.Features.Items.Commands
{
    public class CreateItemCommandValidation : AbstractValidator<CreateItemCommand>
    {
        private readonly IItemRepository _itemRepository;

        public CreateItemCommandValidation(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .NotNull()
                .MaximumLength(50).WithMessage("Nome não pode exceder 50 caracteres.")
                .MustAsync(IsUnique).WithMessage("O Item já está cadastrado nessa empresa.");
        }

        private async Task<bool> IsUnique(string name, CancellationToken cancellationToken)
        {
            return await _itemRepository.IsUniqueAsync(name, "", cancellationToken);
        }
    }
}