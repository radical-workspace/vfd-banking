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
    public class MyCustomerBL : IGenericRepository<MyCustomer>, ISearchPaginationRepo<MyCustomer>
    {
        private readonly BankingSystemContext _context;

        public MyCustomerBL(BankingSystemContext context)
        {
            _context = context;
        }


        public IEnumerable<MyCustomer> GetAllByPagination(string? ID, string? filter, out int totalRecords, out int totalPages, int pageNumber = 1)
        {
            throw new NotImplementedException();
        }
        public MyCustomer? Get(int id)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<MyCustomer> GetAll(string? userID = "", int flag = 1)
        {
            return _context.Customers
                .Include(c => c.Branch)
                .Where(c => c.Id == userID);
        }


        public IEnumerable<MyCustomer> Search(string search, string? tellerID)
        {
            if (search == null)
                return [];


            var query = _context.Customers
                .Include(c => c.Branch)
                    .ThenInclude(b => b.Tellers)
                .Where(c => c.Branch.Tellers.FirstOrDefault()!.Id == tellerID && c.SSN.ToString() == search.Trim());

            if (!query.Any())
                query = _context.Customers
                .Include(c => c.Branch)
                    .ThenInclude(b => b.Tellers)
                .Where(c => c.Branch.Tellers.FirstOrDefault()!.Id == tellerID && (c.FirstName + " " + c.LastName).ToLower().Trim().Contains(search.ToLower().Trim()));


            return query;
        }



        public void Add(MyCustomer Entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(MyCustomer Entity)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<MyCustomer> GetAllIncluding(params Expression<Func<MyCustomer, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public MyCustomer? GetSingleIncluding(Expression<Func<MyCustomer, bool>> predicate, params Expression<Func<MyCustomer, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public void Update(MyCustomer Entity)
        {
            throw new NotImplementedException();
        }

    }
}