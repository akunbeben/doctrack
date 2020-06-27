using DocTrack.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DocTrack.Data.Repository
{
    public interface IDocumentLineMemoRepository : IDisposable
    {
        IEnumerable<DocumentLineMemo> GetAll();

        IEnumerable<DocumentLineMemo> GetWhere(Expression<Func<DocumentLineMemo, bool>> predicate);

        DocumentLineMemo GetById(int? id);

        DocumentLineMemo Save(DocumentLineMemo documentLineMemo, string createdBy);

        DocumentLineMemo Update(int id, DocumentLineMemo documentLineMemo, string modifiedBy);

        void Delete(int id, string modifiedBy);
    }

    public class DocumentLineMemoRepository : GenericRepository<DocumentLineMemo>, IDocumentLineMemoRepository
    {
        public DocumentLineMemoRepository(ApplicationDbContext context) : base(context) { }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
