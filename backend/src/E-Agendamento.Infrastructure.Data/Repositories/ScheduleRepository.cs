using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Domain.Entities;
using E_Agendamento.Infrastructure.Data.Context;
using E_Agendamento.Infrastructure.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace E_Agendamento.Infrastructure.Data.Repositories
{
    public class ScheduleRepository : GenericRepository<Schedule>, IScheduleRepository
    {
        private readonly DbSet<Schedule> _schedules;

        public ScheduleRepository(ApplicationDbContext context) : base(context)
        {
            _schedules = context.Schedules;
        }

        public async Task<IEnumerable<Schedule>> GetByCompanyAsync(string companyId)
        {
            return await _schedules.AsNoTracking()
                                .Where(x => x.CompanyId == companyId)
                                .ToListAsync()
                                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Schedule>> GetByUserAsync(string userId)
        {
            return await _schedules.AsNoTracking()
                                .Where(x => x.RequestedById == userId)
                                .ToListAsync()
                                .ConfigureAwait(false);
        }

        public async Task<bool> ItemAlreadyScheduledByUserAsync(string itemId, string userId)
        {
            return await _schedules.AsNoTracking()
                                .Where(x => x.ItemId == itemId)
                                .Where(x => x.RequestedById == userId)
                                .AnyAsync()
                                .ConfigureAwait(false);
        }
    }
}