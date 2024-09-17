using E_Agendamento.Application.Features.Schedules.Commands.CancelSchedule;
using E_Agendamento.Application.Features.Schedules.Commands.CreateSchedule;
using E_Agendamento.Application.Features.Schedules.Commands.DeleteSchedule;
using E_Agendamento.Application.Features.Schedules.Queries.GetSchedulesByCompany;
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
        public async Task<IActionResult> GetMySchedules([FromQuery] string status = "all")
        {
            GetSchedulesByUserQuery command = new()
            {
                Status = status,
                RequestedBy = AuthenticatedUser.UserId
            };

            return Ok(await Mediator.Send(command));
        }

        // TODO => get schedules by company here or on CompanyController ???
        // correct endpoint = companies/{companyId}/schedules

        [HttpGet("{scheduleId}")]
        public async Task<IActionResult> GetByCompany(string scheduleId)
        {
            GetSchedulesByCompanyQuery query = new()
            {
                CompanyId = scheduleId
            };

            return Ok(await Mediator.Send(query));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateScheduleCommand command)
        {
            command.CompanyId = AuthenticatedUser.CompanyId;
            command.RequestedBy = AuthenticatedUser.UserId;
            return StatusCode(StatusCodes.Status201Created, await Mediator.Send(command));
        }

        [HttpPut("{scheduleId}/cancel/")]
        public async Task<IActionResult> CancelSchedule(string scheduleId)
        {
            CancelScheduleCommand command = new()
            {
                Id = scheduleId,
                CompanyId = AuthenticatedUser.CompanyId,
                CanceledBy = AuthenticatedUser.UserId
            };

            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{scheduleId}")]
        public async Task<IActionResult> Delete(string scheduleId)
        {
            DeleteScheduleCommand command = new()
            {
                CompanyId = AuthenticatedUser.CompanyId,
                ScheduleId = scheduleId
            };

            return StatusCode(StatusCodes.Status204NoContent, await Mediator.Send(command));
        }
    }
}