using E_Agendamento.Application.Contracts.Repositories.Common;
using E_Agendamento.Domain.Entities;

namespace E_Agendamento.Application.Contracts.Repositories
{
    public interface IScheduleRepository : IGenericRepository<Schedule>
    {
        public Task<IEnumerable<Schedule>> GetByCompanyAsync(string companyId);
        public Task<IEnumerable<Schedule>> GetByUserAsync(string userId);
        public Task<IEnumerable<Schedule>> GetByUserWithStatusAsync(string userId, string status);
        public Task<bool> ItemAlreadyScheduledByUserAsync(string itemId, string userId);
    }
}