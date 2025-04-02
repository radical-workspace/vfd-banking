using BankingSystem.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.BLL.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(params object[] keyValues);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task RemoveAsync(params object[] keyValues);
    }

    public class GenericRepository<T>(BankingSystemContext context) : IRepository<T> where T : class
    {
        private readonly BankingSystemContext _context = context;

        public async Task<T> GetByIdAsync(params object[] keyValues) => await _context.Set<T>().FindAsync(keyValues);
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }
        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public async Task RemoveAsync(params object[] keyValues)
        {
            var entity = await GetByIdAsync(keyValues);
            if (entity != null)
            {
                Remove(entity);
            }
        }
    }
}
