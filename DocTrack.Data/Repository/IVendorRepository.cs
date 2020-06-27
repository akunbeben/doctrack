using DocTrack.Common;
using DocTrack.Entity.Dto;
using DocTrack.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DocTrack.Data.Repository
{
    public interface IVendorRepository : IDisposable
    {
        IEnumerable<VendorDto> GetVendorDataTable(DatatablesRequest dataTable);

        int GetTotalVendorFiltered(string search);

        IEnumerable<Vendor> GetAll();

        IEnumerable<Vendor> GetWhere(Expression<Func<Vendor, bool>> predicate);

        Vendor GetById(int? id);

        Vendor Save(Vendor vendor, string createdBy);

        Vendor Update(int id, Vendor vendor, string modifiedBy);

        void Delete(int id, string modifiedBy);
    }

    public class VendorRepository : GenericRepository<Vendor>, IVendorRepository
    {
        public VendorRepository(ApplicationDbContext context) : base(context) { }

        public void Dispose()
        {
            _context.Dispose();
        }

        public override Vendor Save(Vendor vendor, string createdBy)
        {
            vendor.CreatedBy = createdBy;
            _context.Vendor.Add(vendor);
            _context.SaveChanges();

            return vendor;
        }

        public override Vendor Update(int id, Vendor vendor, string modifiedBy)
        {
            var currentData = GetById(id);

            currentData.VendorName = vendor.VendorName;
            currentData.VendorOwn = vendor.VendorOwn;
            currentData.PaymentTerms = vendor.PaymentTerms;
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

        public int GetTotalVendorFiltered(string search)
        {
            return _context.Database.SqlQuery<int>("usp_Vendor_GetTotalVendorFiltered @p0", parameters: new object[] { search }).Single();
        }

        public IEnumerable<VendorDto> GetVendorDataTable(DatatablesRequest dataTable)
        {
            return _context.Database.SqlQuery<VendorDto>("usp_Vendor_GetVendorDatatable @p0, @p1, @p2, @p3, @p4 ", parameters: new object[] { dataTable.Search, dataTable.Start, dataTable.Length, dataTable.SortColumnName, dataTable.SortDir }).ToList();
        }
    }
}
