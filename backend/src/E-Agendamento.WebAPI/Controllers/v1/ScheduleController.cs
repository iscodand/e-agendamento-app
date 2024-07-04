using E_Agendamento.Application.Features.Schedules.Commands.CreateSchedule;
using E_Agendamento.WebAPI.Controllers.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Agendamento.WebAPI.Controllers.v1
{
    [Authorize]
    [Route("api/v1/schedules")]
    public class ScheduleController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateScheduleCommand command)
        {
            command.CompanyId = AuthenticatedUser.CompanyId;
            command.RequestedBy = AuthenticatedUser.UserId;
            return StatusCode(StatusCodes.Status201Created, await Mediator.Send(command));
        }
    }
}