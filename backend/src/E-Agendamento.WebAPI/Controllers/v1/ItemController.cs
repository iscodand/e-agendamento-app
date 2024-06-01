using E_Agendamento.Application.Features.Items.Commands;
using E_Agendamento.Application.Features.Items.Commands.DeleteItemById;
using E_Agendamento.Application.Features.Items.Commands.UpdateItemById;
using E_Agendamento.Application.Features.Items.Queries.GetAllItems;
using E_Agendamento.WebAPI.Controllers.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Agendamento.WebAPI.Controllers.v1
{
    [Authorize]
    [Route("api/v1/items/")]
    public class ItemController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllItemsParameter filter)
        {
            var query = new GetAllItemsQuery()
            {
                PageSize = filter.PageSize,
                PageNumber = filter.PageNumber,
                CompanyId = AuthenticatedUser.CompanyId
            };

            return Ok(await Mediator.Send(query));
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateItemCommand command)
        {
            command.CompanyId = AuthenticatedUser.CompanyId;
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UpdateItemCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            command.CompanyId = AuthenticatedUser.CompanyId;
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok(await Mediator.Send(new DeleteItemCommand
            {
                Id = id
            }));
        }
    }
}