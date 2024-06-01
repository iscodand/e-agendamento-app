using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Application.Wrappers;
using E_Agendamento.Domain.Entities;
using MediatR;

namespace E_Agendamento.Application.Features.Categories.Queries.GetCategoriesByCompany
{
    public class GetCategoriesByCompanyQuery : IRequest<Response<IEnumerable<GetCategoriesViewModel>>>
    {
        public string CompanyId { get; set; }
    }

    public class GetCategoriesByCompanyQueryHandler : IRequestHandler<GetCategoriesByCompanyQuery, Response<IEnumerable<GetCategoriesViewModel>>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoriesByCompanyQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Response<IEnumerable<GetCategoriesViewModel>>> Handle(GetCategoriesByCompanyQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Category> categories = await _categoryRepository.GetByCompanyAsync(request.CompanyId, cancellationToken);
            var categoriesMapped = GetCategoriesViewModel.Map(categories);
            return new("Categorias recuperadas com sucesso", categoriesMapped);
        }
    }
}