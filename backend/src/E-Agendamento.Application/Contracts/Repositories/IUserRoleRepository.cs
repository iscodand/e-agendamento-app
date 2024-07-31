using E_Agendamento.Application.Contracts.Repositories.Common;
using E_Agendamento.Domain.Entities;

namespace E_Agendamento.Infrastructure.Data.Repositories
{
    public interface IUserRoleRepository : IGenericRepository<UsersRoles>
    {
        public Task<ICollection<ApplicationRole>> GetUserRolesByUserIdAsync(string userId);
    }
}