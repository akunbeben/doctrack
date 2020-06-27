using DocTrack.Common;
using DocTrack.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocTrack.Data.Repository
{
    public interface IFormTypeRepository : IDisposable
    {
        IEnumerable<LookupValue> GetFormTypeDatatable(DatatablesRequest datatableRequest);

        int GetTotalFormTypeFiltered(string search);
    }

    public class FormTypeRepository : GenericRepository<LookupValue>, IFormTypeRepository
    {
        public FormTypeRepository(ApplicationDbContext context) : base(context) { }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IEnumerable<LookupValue> GetFormTypeDatatable(DatatablesRequest datatableRequest)
        {
            return _context.Database.SqlQuery<LookupValue>("usp_LookupValue_GetFormTypeDatatable @p0, @p1, @p2, @p3, @p4 ", parameters: new object[] { datatableRequest.Search, datatableRequest.Start, datatableRequest.Length, datatableRequest.SortColumnName, datatableRequest.SortDir }).ToList();
        }

        public int GetTotalFormTypeFiltered(string search)
        {
            return _context.Database.SqlQuery<int>("usp_LookupValue_GetTotalFormTypeFiltered @p0", parameters: new object[] { search }).Single();
        }
    }
}
