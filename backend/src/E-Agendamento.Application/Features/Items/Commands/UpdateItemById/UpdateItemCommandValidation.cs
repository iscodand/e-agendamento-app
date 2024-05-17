using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Application.Features.Items.Commands.UpdateItemById;
using FluentValidation;

namespace E_Agendamento.Application.Features.Items.Commands
{
    public class UpdateItemCommandValidation : AbstractValidator<UpdateItemCommand>
    {

        public UpdateItemCommandValidation(IItemRepository itemRepository)
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .NotNull()
                .MaximumLength(50).WithMessage("Nome não pode exceder 50 caracteres.");
        }
    }
}