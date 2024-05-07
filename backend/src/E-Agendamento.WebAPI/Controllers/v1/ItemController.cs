using E_Agendamento.Application.Features.Items.Commands;
using E_Agendamento.Application.Features.Items.Queries.GetAllItems;
using E_Agendamento.WebAPI.Controllers.Common;
using Microsoft.AspNetCore.Mvc;

namespace E_Agendamento.WebAPI.Controllers.v1
{
    // [Authorize]
    [Route("v1/items/")]
    public class ItemController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllItemsParameter filter)
        {
            return Ok(await Mediator.Send(new GetAllItemsQuery
            {
                PageSize = filter.PageSize,
                PageNumber = filter.PageNumber
            }));
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateItemCommand command)
        {
            command.CompanyId = AuthenticatedUser.CompanyId;
            return Ok(await Mediator.Send(command));
        }
    }
}