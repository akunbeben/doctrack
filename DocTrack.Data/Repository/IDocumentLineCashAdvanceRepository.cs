using DocTrack.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DocTrack.Data.Repository
{
    public interface IDocumentLineCashAdvanceRepository : IDisposable
    {
        IEnumerable<DocumentLineCashAdvance> GetAll();

        IEnumerable<DocumentLineCashAdvance> GetWhere(Expression<Func<DocumentLineCashAdvance, bool>> predicate);

        DocumentLineCashAdvance GetById(int? id);

        DocumentLineCashAdvance Save(DocumentLineCashAdvance documentLineCashAdvance, string createdBy);

        DocumentLineCashAdvance Update(int id, DocumentLineCashAdvance documentLineCashAdvance, string modifiedBy);

        void Delete(int id, string modifiedBy);
    }

    public class DocumentLineCashAdvanceRepository : GenericRepository<DocumentLineCashAdvance>, IDocumentLineCashAdvanceRepository
    {
        public DocumentLineCashAdvanceRepository(ApplicationDbContext context) : base(context) { }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
