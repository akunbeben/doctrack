using DocTrack.Common;
using DocTrack.Entity.Dto;
using DocTrack.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DocTrack.Data.Repository
{
    public interface IDocumentTypeRepository : IDisposable
    {
        IEnumerable<DocumentTypeDto> GetDocumentTypeDataTable(DatatablesRequest dataTable);

        int GetTotalDocumentTypeFiltered(string search);

        IEnumerable<DocumentType> GetAll();

        IEnumerable<DocumentType> GetWhere(Expression<Func<DocumentType, bool>> predicate);

        DocumentType GetById(int? id);

        DocumentType Save(DocumentType documentType, string createdBy);

        DocumentType Update(int id, DocumentType documentType, string modifiedBy);

        void Delete(int id, string modifiedBy);
    }

    public class DocumentTypeRepository : GenericRepository<DocumentType>, IDocumentTypeRepository
    {
        public DocumentTypeRepository(ApplicationDbContext context) : base(context) { }

        public void Dispose()
        {
            _context.Dispose();
        }

        public override DocumentType Save(DocumentType entity, string createdBy)
        {
            entity.CreatedBy = createdBy;

            _context.DocumentType.Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public override DocumentType Update(int id, DocumentType entity, string modifiedBy)
        {
            var currentData = GetById(id);

            currentData.DocumentTypeName = entity.DocumentTypeName;
            currentData.FormType = entity.FormType;
            currentData.PoolName = entity.PoolName;
            currentData.DocumentReference = entity.DocumentReference;
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

        public IEnumerable<DocumentTypeDto> GetDocumentTypeDataTable(DatatablesRequest dataTable)
        {
            return _context.Database.SqlQuery<DocumentTypeDto>("usp_DocumentType_GetDocumentTypeDatatable @p0, @p1, @p2, @p3, @p4 ", parameters: new object[] { dataTable.Search, dataTable.Start, dataTable.Length, dataTable.SortColumnName, dataTable.SortDir }).ToList();
        }

        public int GetTotalDocumentTypeFiltered(string search)
        {
            return _context.Database.SqlQuery<int>("usp_DocumentType_GetTotalDocumentTypeFiltered @p0", parameters: new object[] { search }).Single();
        }
    }
}
