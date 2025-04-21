using BankingSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.BLL.Interfaces
{

    public interface IGenericRepository <T> where T : class
    {
        IQueryable<T> GetAll(string? userID = "", int flag = 1);
        //public IQueryable<T> GetQueryable();

        T? Get(int id, string? id2 = "", long number = 0);

        void Add(T Entity);

        void Update(T Entity);

        void Delete(T Entity);
    

        IEnumerable<T> GetAllIncluding(params Expression<Func<T, object>>[] includes);
        T? GetSingleIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate); //NEW 

        T? GetSingleDeepIncluding(Expression<Func<T, bool>> predicate, params Func<IQueryable<T>, IIncludableQueryable<T, object>>[] includes);


    }
}
