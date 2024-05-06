namespace E_Agendamento.Application.DTOs.Account
{
    public class AuthenticationResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public ICollection<string> Roles { get; set; }
        public ICollection<string> Companies { get; set; }
        public string AccessToken { get; set; }
    }
}