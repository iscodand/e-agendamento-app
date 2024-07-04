using E_Agendamento.Domain.Entities;

namespace E_Agendamento.Application.Features.Categories.Queries.GetCategoriesByCompany
{
    public class GetCategoriesViewModel
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string CompanyId { get; set; }

        public static IEnumerable<GetCategoriesViewModel> Map(IEnumerable<Category> categories)
        {
            return categories.Select(category => new GetCategoriesViewModel
            {
                Id = category.Id,
                Description = category.Description,
                CompanyId = category.CompanyId
            });
        }
    }
}