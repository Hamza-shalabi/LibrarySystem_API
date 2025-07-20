using System.Linq.Expressions;

namespace LibrarySystem.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<List<T>?> GetAllAsync(params Expression<Func<T, object>>[] Includes);
        public Task<T?> GetAsync(int id, params Expression<Func<T, object>>[] Includes);
        public Task<T?> CreateAsync(T entity);
        public Task<T?> UpdateAsync(T entity);
        public Task<T?> DeleteAsync(int id);
        public Task<bool> IsExistAsync(int id);
        public Task<List<T>?> GetByValueAsync(Expression<Func<T, bool>> Where, params Expression<Func<T, object>>[] Includes);
    }
}
