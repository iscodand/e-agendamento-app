using E_Agendamento.Domain.Entities;

namespace E_Agendamento.Application.Features.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdViewModel
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string CompanyId { get; set; }

        public static GetCategoryByIdViewModel Map(Category category)
        {
            return new()
            {
                Id = category.Id,
                Description = category.Description,
                CompanyId = category.CompanyId
            };
        }
    }
}
