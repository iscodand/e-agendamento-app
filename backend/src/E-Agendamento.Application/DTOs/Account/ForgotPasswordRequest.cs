using System.ComponentModel.DataAnnotations;

namespace E_Agendamento.Application.DTOs.Account
{
    public class ForgotPasswordRequest
    {
        [Required(ErrorMessage = "Insira seu e-mail.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail incorreto.")]
        public string Email { get; set; }
    }
}