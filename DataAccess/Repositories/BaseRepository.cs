using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using E_commerce.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.DataAccess.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private ApplicationDbContext _context;
        private DbSet<T> _dbSet;
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        private IQueryable<T> AddIncludes(string[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
            {
                query = _dbSet.Include(include);
            }
                return query;
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
                _dbSet.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync(string[] includes)
        {
            IQueryable<T> query = _dbSet;
            if (includes!= null)
                query = AddIncludes(includes);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllByFilterAsync(Expression<Func<T, bool>> filter, string[] includes)
        {
            IQueryable<T> query = _dbSet;
            if (includes != null)
                query = AddIncludes(includes);
            return await query.Where(filter).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id, string[] includes)
        {
            if (id == null || id <= 0)
                return null;
            IQueryable<T> query = _dbSet;
            if (includes != null)
                query = AddIncludes(includes);
            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        public async Task<T> GetOneByFilterAsync(Expression<Func<T, bool>> filter, string[] includes)
        {
            IQueryable<T> query = _dbSet;
            if (includes != null)
                query = AddIncludes(includes);
            return await query.FirstOrDefaultAsync(filter);
        }

        public T Update(T entity)
        {
            _dbSet.Update(entity);
            return entity;
        }
    }
}
