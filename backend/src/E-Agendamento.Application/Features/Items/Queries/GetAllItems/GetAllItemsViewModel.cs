using E_Agendamento.Application.Features.Categories.Queries.GetCategoryById;
using E_Agendamento.Domain.Entities;

namespace E_Agendamento.Application.Features.Items.Queries.GetAllItems
{
    public class GetAllItemsViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public GetCategoryByIdViewModel Category { get; set; }
        public int TotalQuantity { get; set; }
        public int QuantityAvailable { get; set; }
        public bool IsAvailable { get; set; }
        public string CompanyId { get; set; }

        public static IEnumerable<GetAllItemsViewModel> Map(IEnumerable<Item> items)
        {
            return items.Select(item => new GetAllItemsViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Category = GetCategoryByIdViewModel.Map(item.Category),
                CategoryId = item.CategoryId,
                TotalQuantity = item.TotalQuantity,
                QuantityAvailable = item.QuantityAvailable,
                IsAvailable = item.IsAvailable,
                CompanyId = item.CompanyId,
            }).ToList();
        }
    }
}