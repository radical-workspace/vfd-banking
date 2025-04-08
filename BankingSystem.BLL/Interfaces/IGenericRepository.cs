using BankingSystem.DAL.Models;
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
        IEnumerable<T> GetAll();

        T? Get(int id);

        void Add(T Entity);

        void Update(T Entity);

        void Delete(T Entity);
    

        IEnumerable<T> GetAllIncluding(params Expression<Func<T, object>>[] includes);
        T? GetSingleIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    }
}
