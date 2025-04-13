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
    public class UnitOfWork(BankingSystemContext dbContext) : IUnitOfWork
    {
        private readonly BankingSystemContext _dbContext = dbContext;

        private readonly Hashtable _repostories = [];

        public IGenericRepository<T> Repository<T>() where T : class
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
