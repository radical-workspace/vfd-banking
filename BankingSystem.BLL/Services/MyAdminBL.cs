using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Data;
using BankingSystem.DAL.Models;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.BLL.Services
{
    public class MyAdminBL : IGenericRepository<Admin>, ISearchPaginationRepo<Admin> 
    {
        private readonly BankingSystemContext _context;

        public MyAdminBL(BankingSystemContext context)
        {
            _context = context;
        }


        public Admin? Get(int id, string? UID = "", long number = 0)
        {
            return _context.Admins
                .FirstOrDefault(a => a.Id == UID);
        }


        public void Update(Admin Entity)
        {
            if (Entity != null)
            {
                _context.Admins.Update(Entity);
                _context.SaveChanges();
            }
        }



        public void Add(Admin Entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Admin Entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(Expression<Func<Admin, bool>> predicate)
        {
            throw new NotImplementedException();
        }


        public IQueryable<Admin> GetAll(string? userID = "", int flag = 1)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Admin> GetAllByPagination(string? ID, string? filter, out int totalRecords, out int totalPages, int pageNumber = 1)
        {
            throw new NotImplementedException();
        }




        public IEnumerable<Admin> GetAllIncluding(params Expression<Func<Admin, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Admin? GetSingleDeepIncluding(Expression<Func<Admin, bool>> predicate, params Func<IQueryable<Admin>, IIncludableQueryable<Admin, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Admin? GetSingleIncluding(Expression<Func<Admin, bool>> predicate, params Expression<Func<Admin, object>>[] includes)
        {
            throw new NotImplementedException();
        }




        public IEnumerable<Admin> Search(string search, string? userID = "")
        {
            throw new NotImplementedException();
        }

    }
}
