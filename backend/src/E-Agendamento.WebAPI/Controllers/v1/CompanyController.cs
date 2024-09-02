using E_Agendamento.Application.Features.Companies.Commands.AddUserToCompany;
using E_Agendamento.Application.Features.Companies.Commands.CreateCompany;
using E_Agendamento.Application.Features.Companies.Commands.UpdateCompany;
using E_Agendamento.Application.Features.Companies.Queries.GetAllCompanies;
using E_Agendamento.Application.Features.Companies.Queries.GetCompanyById;
using E_Agendamento.Application.Features.Employees.Queries.GetEmployeesByCompany;
using E_Agendamento.Application.Features.Employees.Queries.SearchByEmployee;
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

        [HttpGet("{companyId}/")]
        public async Task<IActionResult> Get(string companyId)
        {
            GetCompanyByIdQuery query = new() { Id = companyId };
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("{companyId}/employees")]
        public async Task<IActionResult> Get(string companyId, [FromQuery] GetEmployeesByCompanyQuery filter)
        {
            GetEmployeesByCompanyQuery query = new()
            {
                CompanyId = companyId,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };

            return Ok(await Mediator.Send(query));
        }

        [HttpGet("{companyId}/employees/search")]
        public async Task<IActionResult> Search(string companyId, [FromQuery] string search)
        {
            SearchByEmployeeQuery query = new()
            {
                CompanyId = companyId,
                SearchTerm = search
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

        [HttpPut("{companyId}/")]
        public async Task<IActionResult> Put(string companyId, [FromBody] UpdateCompanyCommand request)
        {
            request.Id = companyId;
            return Ok(await Mediator.Send(request));
        }
    }
}