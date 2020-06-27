using DocTrack.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DocTrack.Data.Repository
{
    public interface IDocumentLineSettlementRepository : IDisposable
    {
        IEnumerable<DocumentLineSettlement> GetAll();

        IEnumerable<DocumentLineSettlement> GetWhere(Expression<Func<DocumentLineSettlement, bool>> predicate);

        DocumentLineSettlement GetById(int? id);

        DocumentLineSettlement Save(DocumentLineSettlement documentLineSettlement, string createdBy);

        DocumentLineSettlement Update(int id, DocumentLineSettlement documentLineSettlement, string modifiedBy);

        void Delete(int id, string modifiedBy);
    }

    public class DocumentLineSettlementRepository : GenericRepository<DocumentLineSettlement>, IDocumentLineSettlementRepository
    {
        public DocumentLineSettlementRepository(ApplicationDbContext context) : base(context) { }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
