using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace E_Agendamento.WebAPI.Controllers.Common
{
    [ApiController]
    [Route("/api")]
    public abstract class BaseController : Controller
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}