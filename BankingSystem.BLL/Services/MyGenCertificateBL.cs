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
    public class MyGenCertificateBL : IGenericRepository<GeneralCertificate>, ISearchPaginationRepo<GeneralCertificate>
    {
        private readonly BankingSystemContext _context;

        public MyGenCertificateBL(BankingSystemContext context)
        {
            _context = context;
        }


        public IQueryable<GeneralCertificate> GetAll(string? userID = "", int flag = 1)
        {
            return _context.GeneralCertificates;
        }


        public GeneralCertificate? Get(int id, string? UID = "", long number = 0)
        {
            return _context.GeneralCertificates.FirstOrDefault(c => c.Id == id);
        }



        public void Add(GeneralCertificate Entity)
        {
            if (Entity != null)
            {
                _context.GeneralCertificates.Add(Entity);
                _context.SaveChanges();
            }
        }


        public void Update(GeneralCertificate Entity)
        {
            if (Entity != null)
            {
                _context.GeneralCertificates.Update(Entity);
                _context.SaveChanges();
            }
        }


        public void Delete(GeneralCertificate Entity)
        {
            if (Entity != null)
            {
                _context.GeneralCertificates.Remove(Entity);
                _context.SaveChanges();
            }
        }



        public Task<bool> ExistsAsync(Expression<Func<GeneralCertificate, bool>> predicate)
        {
            throw new NotImplementedException();
        }




        public IEnumerable<GeneralCertificate> GetAllByPagination(string? ID, string? filter, out int totalRecords, out int totalPages, int pageNumber = 1)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GeneralCertificate> GetAllIncluding(params Expression<Func<GeneralCertificate, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public GeneralCertificate? GetSingleDeepIncluding(Expression<Func<GeneralCertificate, bool>> predicate, params Func<IQueryable<GeneralCertificate>, IIncludableQueryable<GeneralCertificate, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public GeneralCertificate? GetSingleIncluding(Expression<Func<GeneralCertificate, bool>> predicate, params Expression<Func<GeneralCertificate, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GeneralCertificate> Search(string search, string? userID = "")
        {
            throw new NotImplementedException();
        }

    }
}
