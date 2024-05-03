using E_Agendamento.Infrastructure.Shared;
using E_Agendamento.Infrastructure.Data;
using E_Agendamento.Infrastructure.Identity;
using E_Agendamento.Application.Contracts.Services;
using E_Agendamento.WebAPI.Services;
using E_Agendamento.Application;
using E_Agendamento.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#region Required Services

builder.Services.AddApplicationLayer();
builder.Services.AddSharedInfrastructure(builder.Configuration);
builder.Services.AddDataInfrastructure(builder.Configuration);
builder.Services.AddIdentityInfrastructure(builder.Configuration);

#endregion

#region Dependency Injection

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseErrorHandlerMiddleware();

app.Run();