using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Application.Contracts.Services;
using E_Agendamento.Application.DTOs.Account;
using E_Agendamento.Application.DTOs.Email;
using E_Agendamento.Application.Exceptions;
using E_Agendamento.Application.Wrappers;
using E_Agendamento.Domain.Entities;
using E_Agendamento.Infrastructure.Identity.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace E_Agendamento.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWTSettings _jwtSettings;
        private readonly IEmailService _emailService;
        private readonly ICompanyRepository _companyRepository;

        public AccountService(UserManager<ApplicationUser> userManager, IOptions<JWTSettings> jwtSettings, IEmailService emailService, ICompanyRepository companyRepository)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _emailService = emailService;
            _companyRepository = companyRepository;
        }

        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
        {
            ApplicationUser user = await _userManager.Users
                .Include(x => x.Companies)
                .Where(x => x.UserName == request.Username)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            if (user is null)
            {
                throw new ApiException("Credenciais Inválidas. Verifique e tente novamente. 1");
            }

            bool passwordIsValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (passwordIsValid == false)
            {
                throw new ApiException("Credenciais Inválidas. Verifique e tente novamente. 2");
            }

            // if (user.EmailConfirmed == false)
            // {
            //     throw new ApiException($"E-mail ainda não verificado. Ative sua conta e tente novamente. ({user.Email})");
            // }

            if (user.IsActive == false)
            {
                throw new ApiException("Sua conta está inativa. Contate um administrador e tente novamente.");
            }

            ICollection<string> roles = await _userManager.GetRolesAsync(user);
            ICollection<string> companies = user.Companies.Select(x => x.Name).ToList();

            JwtSecurityToken jwtSecurityToken = GenerateJWToken(user, roles, companies);
            string accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            AuthenticationResponse response = new()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles,
                AccessToken = accessToken,
                Companies = companies
            };

            return new Response<AuthenticationResponse>("Autenticação realizada com sucesso.", response);
        }

        private JwtSecurityToken GenerateJWToken(ApplicationUser user, ICollection<string> roles, ICollection<string> companies)
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

        public async Task<Response<string>> RegisterAsync(RegisterRequest request, string origin)
        {
            bool userNameAlreadyRegistered = await _userManager.Users.AnyAsync(x => x.UserName == request.UserName);
            if (userNameAlreadyRegistered)
            {
                throw new ApiException("Esse nome de usuário já está em uso. Verifique e tente novamente.");
            }

            bool emailAlreadyRegistered = await _userManager.Users.AnyAsync(x => x.Email == request.Email);
            if (userNameAlreadyRegistered)
            {
                throw new ApiException("Esse e-mail já está em uso. Verifique e tente novamente.");
            }

            bool phoneNumberAlreadyRegistered = await _userManager.Users.AnyAsync(x => x.PhoneNumber == request.PhoneNumber);
            if (phoneNumberAlreadyRegistered)
            {
                throw new ApiException("Esse número de telefone já está em uso. Verifique e tente novamente.");
            }

            ApplicationUser newUser = RegisterRequest.Map(request);

            Company company = await _companyRepository.GetByIdAsync(request.CompanyId);
            if (company is null)
            {
                throw new ApiException("Empresa não encontrada. Verifique e tente novamente.");
            }

            IdentityResult result = await _userManager.CreateAsync(newUser, request.Password);
            if (!result.Succeeded)
            {
                throw new ApiException($"Ops! Ocorreu um erro ao processar a solicitação. {result.Errors.Select(x => x.Description).FirstOrDefault()}");
            }

            await _companyRepository.AddUserToCompanyAsync(company, newUser);
            await _userManager.AddToRolesAsync(newUser, request.Roles);

            // Isco: envio de e-mail de verificação
            string verificationUri = await SendVerificationEmail(newUser, origin);

            EmailRequest emailRequest = new()
            {
                To = newUser.Email,
                Subject = "Confirmar Cadastro no E-Agendamento.",
                Body = $"Confirme sua conta através do link: {verificationUri}",
                From = "nao-responda@e-agendamento.com"
            };

            await _emailService.SendEmailAsync(emailRequest);

            return new Response<string>(
                "Usuário cadastrado com sucesso",
                null
            );
        }

        private async Task<string> SendVerificationEmail(ApplicationUser user, string origin)
        {
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            string route = "api/account/confirm-email";
            Uri endpointUri = new(string.Concat($"{origin}/", route));

            string verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);

            return verificationUri;
        }

        public async Task<Response<string>> ConfirmEmailAsync(string userId, string code)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                throw new ApiException("Usuário não encontrado. Verifique e tente novamente.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (!result.Succeeded)
            {
                throw new ApiException("Um erro ocorreu ao verificar o e-mail. Tente novamente mais tarde");
            }

            return new Response<string>($"Conta confirmada para o e-mail {user.Email}");
        }

        public async Task ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return;
            }

            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            string route = "api/account/reset-password/";
            Uri endpointUri = new(string.Concat($"{origin}/", route));

            EmailRequest emailRequest = new()
            {
                Body = $"Seu token de recuperação é: {code}",
                To = request.Email,
                Subject = "Recuperar senha - E-Agendamento App"
            };

            await _emailService.SendEmailAsync(emailRequest);
        }

        public async Task<Response<string>> ResetPasswordAsync(ResetPasswordRequest request)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                throw new ApiException("Usuário não encontrado.");
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            if (!result.Succeeded)
            {
                throw new ApiException("Token inválido ou expirado. Por favor, solicite a recuperação novamente.");
            }

            return new(
                "Senha recuperada com sucesso.",
                request.Email
            );
        }

        public Response<string> VerifyToken(string token)
        {
            SymmetricSecurityKey authSigningKey = new(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = authSigningKey
            };

            JwtSecurityTokenHandler tokenHandler = new();
            _ = tokenHandler.ValidateToken(token, tokenValidationParameters,
                out SecurityToken securityToken);

            // Isco - 22/11/2023: Valida se o token é do tipo certo (JwtSecurityToken) e se o Algoritmo usado para criptografar o algoritmo é um HmacSha256
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                // throw new SecurityTokenException("Token inválido/expirado.");
                throw new ApiException("Token inválido/expirado.");
            }

            return new("Token validado com sucesso.", null);
        }
    }
}