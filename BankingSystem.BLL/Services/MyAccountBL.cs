using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Data;
using BankingSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace BankingSystem.BLL.Services
{
    public partial class MyAccountBL : IGenericRepository<Account>, ISearchPaginationRepo<Account>
    {
        private readonly BankingSystemContext _context;
        private static Random _random = new Random();

        public MyAccountBL(BankingSystemContext context)
        {
            _context = context;
        }


        public IEnumerable<Account> GetAllByPagination(string? ID, string? filter, out int totalRecords, out int totalPages, int pageNumber = 1)
        {
            int pageSize = 10;

            var query = _context.Accounts
                .Include(a => a.Customer)
                    .ThenInclude(c => c.Branch)
                .Include(a => a.Branch)
                //.Where(a => a.Customer!.Branch.Tellers.Any(t => t.Id == ID))
                .AsQueryable();


            if (filter != null)
                query = query.Where(a => a.AccountStatus.ToString() == filter);


            totalRecords = query.Count();
            totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            if (pageNumber < 1) pageNumber = 1;
            if (totalPages > 0 && pageNumber > totalPages) pageNumber = totalPages;


            return query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }


        public IQueryable<Account> GetAll(string? ID, int flag = 1)
        {
            if (flag == 1)
                return _context.Accounts
                    .Include(a => a.Customer)
                        .ThenInclude(c => c.Branch)
                    .Include(a => a.Branch)
                    //.Where(a => a.Customer!.Branch.Tellers.Any(teller => teller.Id == ID))
                    ;

            else
                return _context.Accounts
                    .Include(a => a.Customer)
                        .ThenInclude(c => c.Branch)
                    .Include(a => a.Branch)
                    .Where(a => a.Customer!.Id == ID)
                    ;
        }


        public IEnumerable<Account> Search(string search, string? tellerID)
        {
            if (search == null)
                return _context.Accounts
                    .Include(c => c.Customer)
                    .Include(c => c.Branch).ToList();

            var query = GetAll(tellerID)
                .Where(a => a.Number.ToString()
                    .Contains(ISearchPaginationRepo<Account>.MyRegex().Replace(search.Trim(), " ")));

            if (!query.Any())
                query = GetAll(tellerID)
                        .Where(a => (a.Customer.FirstName + " " + a.Customer.LastName).ToLower().Trim()
                            .Contains(ISearchPaginationRepo<Account>.MyRegex().Replace(search.ToLower().Trim(), " ")));

            return query;
        }



        public Account? Get(int id, long number)
        {
            return _context.Accounts
                //.IgnoreQueryFilters()
                .Include(a => a.Customer)
                .Include(a => a.Branch)
                .Include(a => a.Card)
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
                    Entity.Number = Generate();
                    Entity.CreatedAt = DateTime.Now;

                    _context.Accounts.Add(Entity);
                    _context.SaveChanges();
                }
                else
                    throw new InvalidOperationException("This customer already have 2 accounts, cannot add more than 2.");
            }
        }


        private long Generate()
        {
            var number = "";
            for (int i = 0; i < 12; i++)
                number += _random.Next(0, 10).ToString();

            return long.Parse(number);
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
                var account = _context.Accounts
                    .Include(a => a.Card)
                    .FirstOrDefault(a => a.Id == entity.Id);

                if (account != null)
                {
                    account.IsDeleted = true;

                    if (account.Card != null)
                        account.Card.IsDeleted = true;

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

        public Account? GetSingleDeepIncluding(Expression<Func<Account, bool>> predicate, params Func<IQueryable<Account>, IIncludableQueryable<Account, object>>[] includes)
        {
            throw new NotImplementedException();
        }
    }
}
