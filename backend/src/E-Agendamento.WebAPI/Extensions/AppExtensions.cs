using E_Agendamento.WebAPI.Middlewares;

namespace E_Agendamento.WebAPI.Extensions
{
    public static class AppExtensions
    {
        public static void UseErrorHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}