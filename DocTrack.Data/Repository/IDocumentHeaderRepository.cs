using DocTrack.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DocTrack.Data.Repository
{
    public interface IDocumentHeaderRepository : IDisposable
    {
        IEnumerable<DocumentHeader> GetAll();

        IEnumerable<DocumentHeader> GetWhere(Expression<Func<DocumentHeader, bool>> predicate);

        DocumentHeader GetById(int? id);

        DocumentHeader Save(DocumentHeader documentHeader, string createdBy);

        DocumentHeader Update(int id, DocumentHeader documentHeader, string modifiedBy);

        void Delete(int id, string modifiedBy);
    }

    public class DocumentHeaderRepository : GenericRepository<DocumentHeader>, IDocumentHeaderRepository
    {
        public DocumentHeaderRepository(ApplicationDbContext context) : base(context) { }

        public void Dispose()
        {
            _context.Dispose();
        }

        public override DocumentHeader Save(DocumentHeader entity, string createdBy)
        {
            entity.CreatedBy = createdBy;

            _context.DocumentHeader.Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public override DocumentHeader Update(int id, DocumentHeader entity, string modifiedBy)
        {
            var currentData = GetById(id);

            currentData.DocumentSequenceNumber = entity.DocumentSequenceNumber;
            currentData.DocumentTypeId = entity.DocumentTypeId;
            currentData.VendorId = entity.VendorId;
            currentData.Amount = entity.Amount;
            currentData.DocumentStatus = entity.DocumentStatus;
            currentData.FlowStatus = entity.FlowStatus;
            currentData.LastModifiedBy = modifiedBy;
            currentData.LastModifiedDate = DateTime.Now;

            _context.SaveChanges();

            return currentData;
        }

        public override void Delete(int id, string modifiedBy)
        {
            var currentData = GetById(id);

            currentData.IsHidden = true;
            currentData.LastModifiedBy = modifiedBy;
            currentData.LastModifiedDate = DateTime.Now;

            _context.SaveChanges();
        }
    }
}
