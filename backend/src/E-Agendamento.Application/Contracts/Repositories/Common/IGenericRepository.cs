namespace E_Agendamento.Application.Contracts.Repositories.Common
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<ICollection<T>> GetAllAsync();
        public Task<IReadOnlyList<T>> GetPagedResponseAsync(int pageNumber, int pageSize);
        public Task<int> GetTotalItemsCountAsync();
        public Task<T> GetByIdAsync(int id);
        public Task<T> CreateAsync(T entity);
        public Task AddAsync(T entity);
        public void UpdateWithoutSave(T entity);
        public Task UpdateAsync(T entity);
        public Task DeleteAsync(T entity);
        public Task SaveAsync();
    }
}