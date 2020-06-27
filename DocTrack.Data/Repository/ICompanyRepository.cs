using DocTrack.Common;
using DocTrack.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DocTrack.Data.Repository
{
    public interface ICompanyRepository : IDisposable
    {
        IEnumerable<Company> GetCompanyDataTable(DatatablesRequest dataTable);

        int GetTotalCompanyFiltered(string search);

        IEnumerable<Company> GetAll();

        IEnumerable<Company> GetWhere(Expression<Func<Company, bool>> predicate);

        Company GetById(int? id);

        Company Save(Company company, string createdBy);

        Company Update(int id, Company company, string modifiedBy);

        void Delete(int id, string modifiedBy);
    }

    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext context) : base(context) { }

        public void Dispose()
        {
            _context.Dispose();
        }

        public override Company Update(int id, Company company, string modifiedBy)
        {
            var currentData = GetById(id);

            currentData.CompanyName = company.CompanyName;
            currentData.LastModifiedBy = modifiedBy;
            currentData.LastModifiedDate = DateTime.Now;
            _context.SaveChanges();

            return currentData;
        }

        public override Company Save(Company company, string createdBy)
        {
            company.CreatedBy = createdBy;
            _context.Company.Add(company);
            _context.SaveChanges();

            return company;
        }

        public override void Delete(int id, string modifiedBy)
        {
            var currentData = GetById(id);

            currentData.IsHidden = true;
            currentData.LastModifiedBy = modifiedBy;
            currentData.LastModifiedDate = DateTime.Now;
            _context.SaveChanges();
        }

        public IEnumerable<Company> GetCompanyDataTable(DatatablesRequest dataTable)
        {
            return _context.Database.SqlQuery<Company>("usp_Company_GetCompanyDatatable @p0, @p1, @p2, @p3, @p4 ", parameters: new object[] { dataTable.Search, dataTable.Start, dataTable.Length, dataTable.SortColumnName, dataTable.SortDir }).ToList();
        }

        public int GetTotalCompanyFiltered(string search)
        {
            return _context.Database.SqlQuery<int>("usp_Company_GetTotalCompanyFiltered @p0", parameters: new object[] { search }).Single();
        }
    }
}
