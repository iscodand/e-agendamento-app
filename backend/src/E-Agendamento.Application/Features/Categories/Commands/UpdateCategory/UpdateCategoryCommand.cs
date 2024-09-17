// using System.Text.Json.Serialization;
// using E_Agendamento.Application.Contracts.Repositories;
// using E_Agendamento.Application.Exceptions;
// using E_Agendamento.Application.Wrappers;
// using E_Agendamento.Domain.Entities;
// using MediatR;

// namespace E_Agendamento.Application.Features.Categories.Commands.UpdateCategory
// {
//     public class UpdateCategoryCommand : IRequest<Response<UpdateCategoryCommand>>
//     {
//         public string Id { get; set; }
//         public string Description { get; set; }

//         [JsonIgnore]
//         public string CompanyId { get; set; }

//         public static Category Map(Category category, UpdateCategoryCommand command)
//         {
//             category.Description = command.Description;
//             return category;
//         }
//     }

//     public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Response<UpdateCategoryCommand>>
//     {
//         private readonly ICategoryRepository _categoryRepository;

//         public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
//         {
//             _categoryRepository = categoryRepository;
//         }

//         public async Task<Response<UpdateCategoryCommand>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
//         {
//             Category category = await _categoryRepository.GetByIdAsync(request.Id);
//             if (category is null)
//             {
//                 throw new KeyNotFoundException();
//             }

//             if (category.CompanyId != request.CompanyId)
//             {
//                 throw new KeyNotFoundException();
//             }

//             bool categoryAlreadyExists = await _categoryRepository.IsUniqueAsync(request.Description, request.CompanyId, cancellationToken, request.Id);
//             if (categoryAlreadyExists)
//             {
//                 throw new ValidationException([new("Description", "Uma categoria já está cadastrada com esse nome.")]);
//             }

//             UpdateCategoryCommand.Map(category, request);
//             await _categoryRepository.UpdateAsync(category);

//             return new("Categoria atualizada com sucesso.", request);
//         }
//     }
// }