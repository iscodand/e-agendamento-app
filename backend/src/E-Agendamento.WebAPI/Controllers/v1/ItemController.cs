using E_Agendamento.Application.Features.Items.Commands;
using E_Agendamento.WebAPI.Controllers.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Agendamento.WebAPI.Controllers
{
    [Authorize]
    [Route("items/")]
    public class ItemController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Post(CreateItemCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}