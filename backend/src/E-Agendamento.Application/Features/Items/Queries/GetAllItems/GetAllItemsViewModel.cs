using E_Agendamento.Domain.Entities;

namespace E_Agendamento.Application.Features.Items.Queries.GetAllItems
{
    public class GetAllItemsViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public int TotalQuantity { get; set; }
        public int QuantityAvailable { get; set; }
        public bool IsAvailable { get; set; }
        public string CompanyId { get; set; }

        public static IEnumerable<GetAllItemsViewModel> Map(IEnumerable<Item> items)
        {
            return items.Select(item => new GetAllItemsViewModel
            {
                Name = item.Name,
                Description = item.Description,
                CategoryId = item.CategoryId,
                TotalQuantity = item.TotalQuantity,
                QuantityAvailable = item.QuantityAvailable,
                IsAvailable = item.IsAvailable,
                CompanyId = item.CompanyId,
            }).ToList();
        }
    }
}