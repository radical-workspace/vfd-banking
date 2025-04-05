using BankingSystem.BLL.Interfaces;
using BankingSystem.BLL.Repositories;
using BankingSystem.DAL.Data;
using BankingSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.BLL
{
    public class UnitOfWork : IUniitOfWork
    {
        private readonly BankingSystemContext _dbContext;

        private Hashtable _repostories;

        public UnitOfWork(BankingSystemContext dbContext )
        {
            _dbContext = dbContext;
            _repostories = new Hashtable();
        }
        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            var key = typeof(T).Name;
            if (!_repostories.ContainsKey(key))
            {
                var repository = new GenericRepository<T>(_dbContext);
                _repostories.Add(key, repository);
            }
            return _repostories[key] as IGenericRepository<T>;
        }

        public int Complete()
        {
            return _dbContext.SaveChanges();
        }


        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
