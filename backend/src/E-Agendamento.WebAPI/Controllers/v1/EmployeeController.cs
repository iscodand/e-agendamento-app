using E_Agendamento.Application.Features.Employees.Queries.GetEmployeeById;
using E_Agendamento.WebAPI.Controllers.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Agendamento.WebAPI.Controllers.v1
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    [Route("api/v1/employees/")]
    public class EmployeeController : BaseController
    {
        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetByIdAsync(string employeeId)
        {
            var query = new GetEmployeeByIdQuery()
            {
                Id = employeeId
            };

            return Ok(Mediator.Send(query));
        }
    }
}