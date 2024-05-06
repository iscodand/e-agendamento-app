using System.ComponentModel.DataAnnotations;

namespace E_Agendamento.Application.DTOs.Account
{
    public class AuthenticationRequest
    {
        [Required(ErrorMessage = "Seu nome de usuário é obrigatório.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Sua senha é obrigatória.")]
        public string Password { get; set; }
    }
}