using System.IdentityModel.Tokens.Jwt;
using E_Agendamento.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace E_Agendamento.Application.Contracts.Services
{
    public interface ITokenService
    {
        public JwtSecurityToken GenerateJWToken(ApplicationUser user, ICollection<string> roles, ICollection<string> companies);
        public SymmetricSecurityKey AuthSigningKey();
    }
}