using System.Linq.Expressions;
using LibrarySystem.Data;
using LibrarySystem.Interface;
using LibrarySystem.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        private readonly ApplicationDBContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(ApplicationDBContext context) { 
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task<T?> CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T?> DeleteAsync(int id)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(u => u.Id == id);
            if(entity == null)
            {
                return null;
            }
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> IsExistAsync(int id)
        {
            var isExists = await _dbSet.AnyAsync(u => u.Id == id);
            return isExists;
        }

        public async Task<List<T>?> GetAllAsync(params Expression<Func<T, object>>[] Includes) 
        {
            IQueryable < T > query = _dbSet;
            foreach (var include in Includes.Where(i => i != null)) { 
                query = query.Include(include);
            }
            return await query.ToListAsync(); 
        }

        public async Task<T?> GetAsync(
            int id,
            params Expression<Func<T, object>>[] Includes
            ) {

            IQueryable<T> query = _dbSet;

            foreach (var include in Includes) { 
                query = query.Include(include);
            }

            query = query.Where(e => e.Id == id);

            
            return await query.FirstOrDefaultAsync();
        }

        public async Task<T?> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<T>?> GetByValueAsync(
            Expression<Func<T, bool>> Where,
            params Expression<Func<T, object>>[] Includes)
        {
            IQueryable<T> query = _dbSet;

            query = query.Where(Where);

            foreach(var include in Includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }
    }
}
