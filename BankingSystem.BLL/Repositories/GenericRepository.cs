using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Data;
using BankingSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.BLL.Repositories
{


    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly BankingSystemContext _dbContext;

        public GenericRepository(BankingSystemContext dbContext)
        {
           _dbContext = dbContext;
        }
        public void Add(T Entity)
        {
          _dbContext.Set<T>().Add(Entity);
        }

        public void Delete(T Entity)
        {
            _dbContext.Remove(Entity);
        }

        public T? Get(int id)
        {
            return _dbContext.Find<T>(id); 
        }

        public IEnumerable<T> GetAll()
        {
            //if (typeof(T) == typeof(Empolyee))
            //{
            //    return (IEnumerable<T>)_dbContext.Empolyees.Include(E => E.Department).AsNoTracking().ToList();
            //}
            
             return _dbContext.Set<T>().AsNoTracking().ToList();
            
        }

        public void Update(T Entity)
        {
            _dbContext.Update(Entity);
        }

        public IEnumerable<T> GetAllIncluding(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.ToList();
        }

        public T? GetSingleIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.FirstOrDefault(predicate);
        }

        public T? GetSingleDeepIncluding(Expression<Func<T, bool>> predicate,params Func<IQueryable<T>, IIncludableQueryable<T, object>>[] includes)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            foreach (var include in includes)
            {
                query = include(query);
            }
            return query.FirstOrDefault(predicate);
        }


    }
}

