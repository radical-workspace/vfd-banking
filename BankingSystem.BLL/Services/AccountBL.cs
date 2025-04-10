using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Data;
using BankingSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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


        public IEnumerable<Account> GetAll(string? ID, int flag = 1)
        {
            if (flag == 1) 
                return _context.Accounts
                    .Include(a => a.Customer)
                        .ThenInclude(c => c.Branch)
                    .Include(a => a.Branch)
                    .Where(a => a.Customer!.Branch.Tellers.Any(teller => teller.Id == ID))
                    .ToList();
            else
                return _context.Accounts
                    .Include(a => a.Customer)
                        .ThenInclude(c => c.Branch)
                    .Include(a => a.Branch)
                    .Where(a => a.Customer!.Id == ID)
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
            if (Entity != null)
            {
                int custAccountCount = _context.Accounts
                   .Where(a => a.CustomerId == Entity.CustomerId)
                   .Count();

                if (custAccountCount < 2)
                {
                    _context.Accounts.Add(Entity);
                    _context.SaveChanges();
                }
                else
                    throw new InvalidOperationException("This customer already have 2 accounts, cannot add more than 2.");
            }
        }


        public void Update(Account Entity)
        {
            if (Entity != null)
            {
                _context.Accounts.Update(Entity);
                _context.SaveChanges();
            }
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

    }
}
