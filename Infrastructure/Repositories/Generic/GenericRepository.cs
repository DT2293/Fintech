using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Generic
{
    //public class GenericRepository<T> : IGenericRepository<T> where T : class
    //{
    //    private readonly AppDbContext _context;
    //    private readonly DbSet<T> _dbSet;

    //    public GenericRepository(AppDbContext context)
    //    {
    //        _context = context;
    //        _dbSet = context.Set<T>();
    //    }

    //    public IQueryable<T> Query() => _dbSet.AsQueryable();
    //    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
    //    public async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);
    //    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
    //    public void Update(T entity) => _dbSet.Update(entity);
    //    public void Delete(T entity) => _dbSet.Remove(entity);
    //    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    //}

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public IQueryable<T> Query() => _dbSet.AsQueryable();

        public async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);
        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        
        public virtual Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

    
    }


}
