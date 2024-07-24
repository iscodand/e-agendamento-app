using E_Agendamento.Application.DTOs.Account;

namespace E_Agendamento.Application.Contracts.Services
{
    public interface IAuthenticatedUserService
    {
        public string UserId { get; }
        public string CompanyId { get; }
    }
}