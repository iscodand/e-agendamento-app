using System.Text.Json.Serialization;
using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Application.Exceptions;
using E_Agendamento.Application.Wrappers;
using E_Agendamento.Domain.Entities;
using MediatR;

namespace E_Agendamento.Application.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommand : IRequest<Response<string>>
    {
        public string Id { get; set; }

        [JsonIgnore]
        public string UserId { get; set; }
    }

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Response<string>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;

        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, IUserRepository userRepository)
        {
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
        }

        public async Task<Response<string>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            Category category = await _categoryRepository.GetWithItemsAsync(request.Id);
            if (category is null)
            {
                throw new KeyNotFoundException();
            }

            ApplicationUser user = await _userRepository.GetWithCompaniesAsync(request.UserId);
            if (user is null)
            {
                throw new ApplicationException("Erro ao processar sua solicitação. Verifique e tente novamente");
            }

            bool userAndCategoryAreSameCompanies = user.Companies.Any(x => x.Id == category.CompanyId);
            if (userAndCategoryAreSameCompanies == false)
            {
                throw new CompanyException();
            }

            if (category.Items.Count != 0)
            {
                throw new ApiException("Você não pode deletar uma categoria que possui itens cadastrados. Delete todos os itens da categoria e tente novamente.");
            }

            await _categoryRepository.DeleteAsync(category);

            return new("Categoria deletada com sucesso.", request.Id);
        }
    }
}