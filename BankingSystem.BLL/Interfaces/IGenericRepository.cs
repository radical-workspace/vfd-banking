using BankingSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.BLL.Interfaces
{

    public interface IGenericRepository <T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();

        T? Get(int id);

        void Add(T Entity);

        void Update(T Entity);

        void Delete(T Entity);
    }
}
