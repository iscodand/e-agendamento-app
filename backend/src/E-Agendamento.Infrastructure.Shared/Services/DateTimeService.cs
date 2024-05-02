using E_Agendamento.Application.Contracts.Services;

namespace E_Agendamento.Infrastructure.Shared
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}