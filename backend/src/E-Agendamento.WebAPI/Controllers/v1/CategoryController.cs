using E_Agendamento.Application.Features.Categories.Queries.GetCategoriesByCompany;
using E_Agendamento.WebAPI.Controllers.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
            return Ok(await Mediator.Send(
                new GetCategoriesByCompanyQuery()
                {
                    CompanyId = AuthenticatedUser.CompanyId
                }
            ));
        }
    }
}