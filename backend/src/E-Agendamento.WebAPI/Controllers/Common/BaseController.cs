using E_Agendamento.Application.Contracts.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace E_Agendamento.WebAPI.Controllers.Common
{
    [ApiController]
    [Route("/api")]
    public abstract class BaseController : Controller
    {
        private IMediator _mediator;
        private IAuthenticatedUserService _authenticatedUser;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        protected IAuthenticatedUserService AuthenticatedUser => _authenticatedUser ??= HttpContext.RequestServices.GetService<IAuthenticatedUserService>();
    }
}