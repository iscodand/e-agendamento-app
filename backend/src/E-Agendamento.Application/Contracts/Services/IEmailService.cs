using E_Agendamento.Application.DTOs.Email;
using E_Agendamento.Application.Wrappers;

namespace E_Agendamento.Application.Contracts.Services
{
    public interface IEmailService
    {
        public Task SendEmailAsync(EmailRequest emailRequest);
    }
}