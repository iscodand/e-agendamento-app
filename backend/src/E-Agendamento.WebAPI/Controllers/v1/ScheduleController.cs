using E_Agendamento.Application.Features.Schedules.Commands.CreateSchedule;
using E_Agendamento.Application.Features.Schedules.Queries.GetSchedulesByUser;
using E_Agendamento.WebAPI.Controllers.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Agendamento.WebAPI.Controllers.v1
{
    [Authorize]
    [Route("api/v1/schedules")]
    public class ScheduleController : BaseController
    {
        [HttpGet("me/")]
        public async Task<IActionResult> Get()
        {
            GetSchedulesByUserQuery command = new()
            {
                RequestedBy = AuthenticatedUser.UserId
            };

            return Ok(await Mediator.Send(command));
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