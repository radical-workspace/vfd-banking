using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Data;
using BankingSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.BLL.Services
{
    public class MyCardBL : IGenericRepository<VisaCard>
    {
        private readonly BankingSystemContext _context;

        public MyCardBL(BankingSystemContext context)
        {
            _context = context;
        }



        public IEnumerable<VisaCard> GetAll(string? userID = "", int flag = 1)
        {
            return _context.Cards
                   .Include(c => c.Account)
                       .ThenInclude(a => a.Customer)
                   .Where(c => c.Account != null && c.Account.Customer != null && c.Account.Customer.Id == userID)
                   .ToList();
        }


        public VisaCard? Get(int id)
        {
            throw new NotImplementedException();
        }



        public void Add(VisaCard Entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(VisaCard Entity)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<VisaCard> GetAllIncluding(params Expression<Func<VisaCard, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public VisaCard? GetSingleIncluding(Expression<Func<VisaCard, bool>> predicate, params Expression<Func<VisaCard, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public void Update(VisaCard Entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(Expression<Func<VisaCard, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public VisaCard? GetSingleDeepIncluding(Expression<Func<VisaCard, bool>> predicate, params Func<IQueryable<VisaCard>, IIncludableQueryable<VisaCard, object>>[] includes)
        {
            throw new NotImplementedException();
        }
    }
}
