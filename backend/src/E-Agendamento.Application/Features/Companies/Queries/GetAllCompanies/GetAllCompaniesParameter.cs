using E_Agendamento.Application.Parameters;

namespace E_Agendamento.Application.Features.Companies.Queries.GetAllCompanies
{
    public class GetAllCompaniesParameter : RequestParameter
    {
        public static GetAllCompaniesParameter Map(RequestParameter parameter)
        {
            return new()
            {
                PageNumber = parameter.PageNumber,
                PageSize = parameter.PageSize
            };
        }
    }
}