using BankingSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.BLL.Interfaces
{
    public interface ISearchPaginationRepo<T> where T : class
    {
        public IEnumerable<T> Search(string search, string? tellerID);
        public IEnumerable<T> GetAllByPagination(string? ID, string? filter, out int totalRecords, out int totalPages, int pageNumber = 1);
    }
}
