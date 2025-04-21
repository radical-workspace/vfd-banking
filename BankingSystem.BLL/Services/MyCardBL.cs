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
    public class MyCardBL : IGenericRepository<VisaCard>, ISearchPaginationRepo<VisaCard>
    {
        private readonly BankingSystemContext _context;
        private static Random _random = new Random();

        public MyCardBL(BankingSystemContext context)
        {
            _context = context;
        }


        public IQueryable<VisaCard> GetAll(string? userID = "", int flag = 1)
        {
            return _context.Cards
                   .Include(c => c.Account)
                       .ThenInclude(a => a.Customer)
                   .Where(c => c.Account != null && c.Account.Customer != null && c.Account.Customer.Id == userID)
                   ;
        }


        public VisaCard? Get(int id, string? UID = "", long number = 0)
        {
            return _context.Cards
                   .Include(c => c.Account)
                       .ThenInclude(a => a.Customer)
                   .FirstOrDefault(a => a.Account!.Number == number);
        }


        public IEnumerable<VisaCard> Search(string search, string? usereID = "")
        {
            var query = _context.Cards
                        .Include(c => c.Account)
                            .ThenInclude(a => a.Customer)
                            .AsQueryable();


            if (search == null)
                return query;


            query = query
                .Where(c => c.Account!.Number.ToString()
                    .Contains(ISearchPaginationRepo<VisaCard>.MyRegex().Replace(search.Trim(), "")));


            if (!query.Any())
                return _context.Cards
                       .Include(c => c.Account)
                          .ThenInclude(a => a.Customer)
                       .Where(c => c.Number.ToString()
                           .Contains(ISearchPaginationRepo<VisaCard>.MyRegex().Replace(search.Trim(), "")));

            return query;
        }




        public void Add(VisaCard Entity)
        {
            if (Entity != null)
            {
                Entity.Number = GenerateCardNumber();
                Entity.CVV = GenerateCVV().ToString();
                Entity.CreationDate = DateTime.Now;
                Entity.ExpDate = Entity.CreationDate.AddYears(7);
                Entity.Account!.CustomerId = _context.Accounts.FirstOrDefault(a => a.Number == Entity.Account.Number).CustomerId;

                _context.Cards.Add(Entity);
                _context.SaveChanges();
            }
        }


        private int GenerateCVV()
        {
            var number = "";
            for (int i = 0; i < 3; i++)
                number += _random.Next(0, 10).ToString();

            return int.Parse(number);
        }
        

        private string GenerateCardNumber()
        {
            string number = "";
            for (int i = 0; i < 16; i++)
                number += _random.Next(0, 10).ToString();

            return number;
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

        public IEnumerable<VisaCard> GetAllByPagination(string? ID, string? filter, out int totalRecords, out int totalPages, int pageNumber = 1)
        {
            throw new NotImplementedException();
        }
    }
}
