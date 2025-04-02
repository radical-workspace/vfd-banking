using BankingSystem.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.BLL.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class;
        Task<int> SaveChangesAsync();
    }

    public class UnitOfWork(BankingSystemContext context) : IUnitOfWork
    {
        private readonly BankingSystemContext _context = context;
        private readonly Dictionary<Type, object> _repositories = [];

        public IRepository<T> GetRepository<T>() where T : class
        {
            if (_repositories.ContainsKey(typeof(T)))
                return (IRepository<T>)_repositories[typeof(T)];

            var repository = new GenericRepository<T>(_context);
            _repositories.Add(typeof(T), repository);
            return repository;
        }

        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public void Dispose()
            => _context.Dispose();
    }
}
