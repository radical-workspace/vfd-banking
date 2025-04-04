using BankingSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.BLL.Interfaces
{
    public interface IUniitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseEntity;

        int Complete();
    }
}
