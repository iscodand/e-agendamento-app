using System.Text;
using E_Agendamento.Application.Contracts.Services;
using E_Agendamento.Application.Wrappers;
using E_Agendamento.Domain.Entities;
using E_Agendamento.Infrastructure.Data.Context;
using E_Agendamento.Infrastructure.Identity.Services;
using E_Agendamento.Infrastructure.Identity.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace E_Agendamento.Infrastructure.Identity
{
    public static class ServiceRegistration
    {
        public static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                // Password Options
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                // User Options
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            #region Services

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITokenService, TokenService>();

            #endregion

            services.Configure<JWTSettings>(configuration.GetSection(nameof(JWTSettings)));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JWTSettings:Issuer"],
                        ValidAudience = configuration["JWTSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                    };
                    o.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = context =>
                        {
                            context.NoResult();
                            context.Response.StatusCode = 500;
                            context.Response.ContentType = "text/plain";
                            var result = JsonConvert.SerializeObject(new Response<string>(context.Exception.ToString()));
                            return context.Response.WriteAsync(result);
                        },
                        OnChallenge = context =>
                        {
                            if (!context.Response.HasStarted)
                            {
                                context.Response.StatusCode = 401;
                                context.Response.ContentType = "application/json";
                                var result = JsonConvert.SerializeObject(new Response<string>("Você não está autorizado."));
                                return context.Response.WriteAsync(result);
                            }

                            return Task.CompletedTask;
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new Response<string>("Você não tem permissão para acessar esse recurso."));
                            return context.Response.WriteAsync(result);
                        },
                    };
                });
        }
    }
}