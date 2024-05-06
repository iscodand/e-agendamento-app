using System.Security.Claims;
using E_Agendamento.Application.Contracts.Services;

namespace E_Agendamento.WebAPI.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue("uid");
        public string CompanyId => _httpContextAccessor.HttpContext?.User?.FindFirstValue("companyId");
    }
}