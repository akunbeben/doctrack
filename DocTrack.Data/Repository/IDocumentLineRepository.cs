using DocTrack.Common;
using DocTrack.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DocTrack.Data.Repository
{
    public interface IDocumentLineRepository : IDisposable
    {
        IEnumerable<DocumentLine> GetDocumentLineDataTable(DatatablesRequest dataTable);

        int GetTotalDocumentLineFiltered(string search);
    }

    public class DocumentLineRepository : GenericRepository<DocumentLine>, IDocumentLineRepository
    {
        public DocumentLineRepository(ApplicationDbContext context) : base(context) { }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IEnumerable<DocumentLine> GetDocumentLineDataTable(DatatablesRequest dataTable)
        {
            return _context.Database.SqlQuery<DocumentLine>("usp_DocumentLine_GetDocumentLineDatatable @p0, @p1, @p2, @p3, @p4 ", parameters: new object[] { dataTable.Search, dataTable.Start, dataTable.Length, dataTable.SortColumnName, dataTable.SortDir }).ToList();
        }

        public int GetTotalDocumentLineFiltered(string search)
        {
            return _context.Database.SqlQuery<int>("usp_DocumentLine_GetTotalDocumentLineFiltered @p0", parameters: new object[] { search }).Single();
        }
    }
}
