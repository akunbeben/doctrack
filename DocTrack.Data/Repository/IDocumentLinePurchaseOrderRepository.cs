using DocTrack.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DocTrack.Data.Repository
{
    public interface IDocumentLinePurchaseOrderRepository : IDisposable
    {
        IEnumerable<DocumentLinePurchaseOrder> GetAll();

        IEnumerable<DocumentLinePurchaseOrder> GetWhere(Expression<Func<DocumentLinePurchaseOrder, bool>> predicate);

        DocumentLinePurchaseOrder GetById(int? id);

        DocumentLinePurchaseOrder Save(DocumentLinePurchaseOrder documentLinePurchaseOrder, string createdBy);

        DocumentLinePurchaseOrder Update(int id, DocumentLinePurchaseOrder documentLinePurchaseOrder, string modifiedBy);

        void Delete(int id, string modifiedBy);
    }

    public class DocumentLinePurchaseOrderRepository : GenericRepository<DocumentLinePurchaseOrder>, IDocumentLinePurchaseOrderRepository
    {
        public DocumentLinePurchaseOrderRepository(ApplicationDbContext context) : base(context) { }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
