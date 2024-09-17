using E_Agendamento.Application.Features.Categories.Queries.GetCategoryById;
using E_Agendamento.Domain.Entities;

namespace E_Agendamento.Application.Features.Items.Common
{
    public class ItemViewModel
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

        public static IEnumerable<ItemViewModel> Map(IEnumerable<Item> items)
        {
            return items.Select(item => new ItemViewModel
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

        public static ItemViewModel Map(Item item)
        {
            return new()
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
            };
        }
    }
}