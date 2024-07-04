using E_Agendamento.Application.Parameters;

namespace E_Agendamento.Application.Features.Items.Queries.GetAllItems
{
    public class GetAllItemsParameter : RequestParameter
    {
        public static GetAllItemsParameter Map(RequestParameter parameter)
        {
            return new()
            {
                PageNumber = parameter.PageNumber,
                PageSize = parameter.PageSize
            };
        }
    }
}