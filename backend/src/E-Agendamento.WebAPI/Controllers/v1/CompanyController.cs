using E_Agendamento.Application.Features.Companies.Commands.AddUserToCompany;
using E_Agendamento.Application.Features.Companies.Commands.CreateCompany;
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
        public async Task<IActionResult> Get([FromQuery] GetAllCompaniesParameter filter)
        {
            GetAllCompaniesQuery query = new()
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };

            return Ok(await Mediator.Send(query));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCompanyCommand request)
        {
            return StatusCode(StatusCodes.Status201Created, await Mediator.Send(request));
        }

        [HttpPost("{companyId}/add-user")]
        public async Task<IActionResult> AddUserToCompany(string companyId, [FromBody] AddUserToCompanyCommand request)
        {
            request.CompanyId = companyId;
            return Ok(await Mediator.Send(request));
        }
    }
}