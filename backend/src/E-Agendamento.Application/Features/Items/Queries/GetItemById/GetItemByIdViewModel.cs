using E_Agendamento.Application.Features.Categories.Queries.GetCategoryById;
using E_Agendamento.Domain.Entities;

namespace E_Agendamento.Application.Features.Items.Queries.GetItemById
{
    public class GetItemByIdViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public GetCategoryByIdViewModel Category { get; set; }
        public int TotalQuantity { get; set; }
        public int QuantityAvailable { get; set; }
        public bool IsAvailable { get; set; }
        public string CompanyId { get; set; }

        public static GetItemByIdViewModel Map(Item item)
        {
            return new()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Category = item.Category is not null ? GetCategoryByIdViewModel.Map(item.Category) : null,
                TotalQuantity = item.TotalQuantity,
                QuantityAvailable = item.QuantityAvailable,
                IsAvailable = item.IsAvailable,
                CompanyId = item.CompanyId
            };
        }
    }
}