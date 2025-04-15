using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Data;
using BankingSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.BLL.Services
{
    public class MyTicketBL : IGenericRepository<SupportTicket>, ISearchPaginationRepo<SupportTicket>
    {
        private readonly BankingSystemContext _context;

        public MyTicketBL(BankingSystemContext context)
        {
            _context = context;
        }
       


        public IEnumerable<SupportTicket> GetAll(string? userID = "", int flag = 1)
        {
            return _context.SupportTickets
                .Include(t => t.Account)
                .Include(t => t.Teller)
                    .ThenInclude(t => t.Branch)
                .Include(t => t.Customer)
                    .ThenInclude(c => c.Accounts)
                .ToList();
        }


        public SupportTicket? Get(int id, long number = 0)
        {
            return _context.SupportTickets
                .Include(t => t.Teller)
                    .ThenInclude(t => t.Branch)
                .Include(t => t.Customer)
                    .ThenInclude(c => c.Accounts)
                .FirstOrDefault(t => t.Id == id);
        }


        public void Update(SupportTicket entity)
        {
            var existing = _context.SupportTickets.FirstOrDefault(t => t.Id == entity.Id);
            if (existing != null)
            {
                existing.Response = entity.Response;
                existing.Status = entity.Status;
                _context.SaveChanges();
            }
        }


        public IEnumerable<SupportTicket> Search(string search, string? userID = "")
        {
            var query = GetAll();

            if (search == null)
                return query;


            var sanitizedSearch = ISearchPaginationRepo<SupportTicket>.MyRegex().Replace(search.Trim(), "");

            query = query
                .Where(t => t.Customer != null
                    && t.Customer.Accounts != null
                    && t.Customer.Accounts
                        .Any(a => a.Number.ToString().Contains(sanitizedSearch)));


            if (!query.Any())
                query = query
                            .Where(t => (t.Customer?.FirstName + " " + t.Customer?.LastName).ToLower().Trim()
                                .Contains(ISearchPaginationRepo<Account>.MyRegex().Replace(search.ToLower().Trim(), " ")));

            return query;
        }




        public void Add(SupportTicket entity)
        {
            throw new NotImplementedException();
        }


        public void Delete(SupportTicket entity)
        {
            throw new NotImplementedException();
        }




        public IEnumerable<SupportTicket> GetAllIncluding(params Expression<Func<SupportTicket, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public SupportTicket GetById(int id)
        {
            throw new NotImplementedException();
        }

        public SupportTicket? GetSingleIncluding(Expression<Func<SupportTicket, bool>> predicate, params Expression<Func<SupportTicket, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        
        public IEnumerable<SupportTicket> GetAllByPagination(string? ID, string? filter, out int totalRecords, out int totalPages, int pageNumber = 1)
        {
            throw new NotImplementedException();
        }
    }
}