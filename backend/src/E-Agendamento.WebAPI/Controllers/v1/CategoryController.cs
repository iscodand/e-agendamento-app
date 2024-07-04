using System.Net;
using E_Agendamento.Application.Features.Categories.Commands.CreateCategory;
using E_Agendamento.Application.Features.Categories.Commands.DeleteCategory;
using E_Agendamento.Application.Features.Categories.Commands.UpdateCategory;
using E_Agendamento.Application.Features.Categories.Queries.GetCategoriesByCompany;
using E_Agendamento.WebAPI.Controllers.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Agendamento.WebAPI.Controllers.v1
{
    [Authorize]
    [Route("api/v1/categories/")]
    public class CategoryController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            GetCategoriesByCompanyQuery query = new()
            {
                CompanyId = AuthenticatedUser.CompanyId
            };

            if (query.CompanyId is null)
                return StatusCode(StatusCodes.Status401Unauthorized, "Você não tem permissão.");

            return Ok(await Mediator.Send(query));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCategoryCommand command)
        {
            command.CompanyId = AuthenticatedUser.CompanyId;
            return StatusCode(StatusCodes.Status201Created, await Mediator.Send(command));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateCategoryCommand command)
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
            return Ok(await Mediator.Send(new DeleteCategoryCommand()
            {
                Id = id,
                UserId = AuthenticatedUser.UserId
            }));
        }
    }
}