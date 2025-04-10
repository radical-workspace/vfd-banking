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
    public class CustomerBL : IGenericRepository<Customer>
    {
        private readonly BankingSystemContext _context;

        public CustomerBL(BankingSystemContext context)
        {
            _context = context;
        }


        public IEnumerable<Customer> GetAll(string? userID = "", int flag = 1)
        {
            return _context.Customers
                .Include(c => c.Branch)
                .Where(c => c.Id == userID);
        }

        public Customer? Get(int id)
        {
            throw new NotImplementedException();
        }
        
        
        public void Add(Customer Entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Customer Entity)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<Customer> GetAllIncluding(params Expression<Func<Customer, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Customer? GetSingleIncluding(Expression<Func<Customer, bool>> predicate, params Expression<Func<Customer, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public void Update(Customer Entity)
        {
            throw new NotImplementedException();
        }
    }
}