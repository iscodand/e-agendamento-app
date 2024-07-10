using E_Agendamento.Application.Features.Companies.Queries.GetAllCompanies;
using E_Agendamento.Domain.Enums;
using E_Agendamento.WebAPI.Controllers.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Agendamento.WebAPI.Controllers.v1
{
    [Authorize(Roles = nameof(Roles.SuperAdmin))]
    [Route("api/v1/companies/")]
    public class CompanyController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] GetAllCompaniesParameter filter)
        {
            GetAllCompaniesQuery query = new()
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };

            return Ok(await Mediator.Send(query));
        }
    }
}