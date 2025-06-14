using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.DataAccess.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(string[] includes = null);
        Task<T> GetByIdAsync(int id, string[] includes = null);
        Task<IEnumerable<T>> GetAllByFilterAsync(Expression<Func<T, bool>> filter, string[] includes = null);
        Task<T> GetOneByFilterAsync(Expression<Func<T, bool>> filter, string[] includes = null);
        Task<T> CreateAsync(T entity);
        T Update(T entity);
        void Delete(int id);
    }
}
