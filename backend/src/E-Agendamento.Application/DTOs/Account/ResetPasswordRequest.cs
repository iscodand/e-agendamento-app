using System.ComponentModel.DataAnnotations;

namespace E_Agendamento.Application.DTOs.Account
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        public string Token { get; set; }

        [Required(ErrorMessage = "Insira sua nova senha.")]
        [Length(8, 32, ErrorMessage = "Sua senha precisa conter entre 8 e 32 caracteres.")]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Suas senhas não coincidem.")]
        public string ConfirmPassword { get; set; }
    }
}