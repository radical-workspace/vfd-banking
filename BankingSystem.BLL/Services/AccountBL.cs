using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Data;
using BankingSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.BLL.Services
{
    public class AccountBL : IGenericRepository<Account>
    {
        private readonly BankingSystemContext _context;

        public AccountBL(BankingSystemContext context)
        {
            _context = context;
        }


        public IEnumerable<Account> GetAll()
        {
            return _context.Accounts
                //.IgnoreQueryFilters()
                .Include(a => a.Customer)
                .Include(a => a.Branch)
                .ToList();
        }


        public Account? Get(int id)
        {
            return _context.Accounts
                //.IgnoreQueryFilters()
                .Include(a => a.Customer)
                .Include(a => a.Branch)
                .FirstOrDefault(a => a.Id == id);
        }


        public void Add(Account Entity)
        {
            throw new NotImplementedException();
        }


        public void Update(Account Entity)
        {
            throw new NotImplementedException();
        }


        public void Delete(Account entity)
        {
            if (entity != null)
            {
                var account = _context.Accounts.FirstOrDefault(a => a.Id == entity.Id);
                if (account != null)
                {
                    account.IsDeleted = true;
                    _context.SaveChanges();
                }
            }
        }



        public IEnumerable<Account> GetAllIncluding(params System.Linq.Expressions.Expression<Func<Account, object>>[] includes)
        {
            throw new NotImplementedException();
        }


        public Account? GetSingleIncluding(System.Linq.Expressions.Expression<Func<Account, bool>> predicate, params System.Linq.Expressions.Expression<Func<Account, object>>[] includes)
        {
            throw new NotImplementedException();
        }

    
        public Task<bool> ExistsAsync(Expression<Func<Account, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
