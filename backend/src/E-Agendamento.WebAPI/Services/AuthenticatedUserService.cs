using System.Security.Claims;
using E_Agendamento.Application.Contracts.Services;

namespace E_Agendamento.WebAPI.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        public string UserId { get; }

        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue("uid");
        }
    }
}