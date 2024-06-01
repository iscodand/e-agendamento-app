using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Application.Exceptions;
using E_Agendamento.Application.Wrappers;
using E_Agendamento.Domain.Entities;
using MediatR;

namespace E_Agendamento.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<Response<CreateCategoryCommand>>
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string CompanyId { get; set; }

        public static Category Map(CreateCategoryCommand command)
        {
            return new Category
            {
                Description = command.Description,
                CompanyId = command.CompanyId
            };
        }
    }

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Response<CreateCategoryCommand>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Response<CreateCategoryCommand>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            bool categoryAlreadyExists = await _categoryRepository.AlreadyRegisteredByDescriptionAsync(command.Description, command.CompanyId, cancellationToken);
            if (categoryAlreadyExists)
            {
                throw new ValidationException([new("Description", "Uma categoria já está cadastrada com esse nome.")]);
            }

            throw new NotImplementedException();
        }
    }
}