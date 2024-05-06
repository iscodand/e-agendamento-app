using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        private readonly IEmailService _emailService;
        private readonly JWTSettings _jwtSettings;

        public AccountService(UserManager<ApplicationUser> userManager, IOptions<JWTSettings> jwtSettings, IEmailService emailService)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _emailService = emailService;
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

            if (user.EmailConfirmed == false)
            {
                throw new ApiException($"E-mail ainda não verificado. Ative sua conta e tente novamente. ({user.Email})");
            }

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
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            List<Claim> companyClaims = [];
            foreach (string company in companies)
            {
                companyClaims.Add(new Claim("company", company));
            }

            IEnumerable<Claim> claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Name, user.FullName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
            }.Union(roleClaims);

            Console.WriteLine(_jwtSettings.Key);

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
                throw new ApiException("Esse nome de e-mail já está em uso. Verifique e tente novamente.");
            }

            bool phoneNumberAlreadyRegistered = await _userManager.Users.AnyAsync(x => x.PhoneNumber == request.PhoneNumber);
            if (phoneNumberAlreadyRegistered)
            {
                throw new ApiException("Esse nome de e-mail já está em uso. Verifique e tente novamente.");
            }

            // enviar email de verificação

            ApplicationUser newUser = RegisterRequest.Map(request);
            IdentityResult result = await _userManager.CreateAsync(newUser, request.Password);
            if (!result.Succeeded)
            {
                throw new ApiException($"Ops! Ocorreu um erro ao processar a solicitação. {result.Errors.Select(x => x.Description).FirstOrDefault()}");
            }

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
                throw new ApiException("Usuário não encontrado.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (!result.Succeeded)
            {
                throw new ApiException("Um erro ocorreu ao verificar o e-mail. Tente novamente mais tarde");
            }

            return new Response<string>($"Conta confirmada para o e-mail {user.Email}");
        }

        public Task<Response<string>> ForgotPassword()
        {
            throw new NotImplementedException();
        }

        public Task<Response<string>> ResetPassword()
        {
            throw new NotImplementedException();
        }
    }
}