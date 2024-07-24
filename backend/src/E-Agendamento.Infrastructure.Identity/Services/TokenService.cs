using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using E_Agendamento.Application.Contracts.Services;
using E_Agendamento.Domain.Entities;
using E_Agendamento.Infrastructure.Identity.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace E_Agendamento.Infrastructure.Identity.Services
{
    public class TokenService : ITokenService
    {
        private readonly JWTSettings _jwtSettings;

        public TokenService(IOptions<JWTSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public JwtSecurityToken GenerateJWToken(ApplicationUser user, ICollection<string> roles, ICollection<string> companies)
        {
            List<Claim> roleClaims = [];
            foreach (string role in roles)
            {
                roleClaims.Add(new Claim("role", role));
            }

            List<Claim> companyClaims = [];
            foreach (string company in companies)
            {
                companyClaims.Add(new Claim("companyId", company));
            }

            IEnumerable<Claim> claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Name, user.FullName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
            }.Union(roleClaims)
             .Union(companyClaims);

            SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            SigningCredentials signingCredentials = new(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials
            );

            return jwtSecurityToken;

        }

        public SymmetricSecurityKey AuthSigningKey()
        {
            return new(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        }
    }
}