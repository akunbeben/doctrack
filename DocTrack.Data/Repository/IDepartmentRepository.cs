using DocTrack.Common;
using DocTrack.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DocTrack.Data.Repository
{
    public interface IDepartmentRepository : IDisposable
    {
        IEnumerable<Department> GetDepartmentDataTable(DatatablesRequest dataTable);

        int GetTotalDepartmentFiltered(string search);

        IEnumerable<Department> GetAll();

        IEnumerable<Department> GetWhere(Expression<Func<Department, bool>> predicate);

        Department GetById(int? id);

        Department Save(Department department, string createdBy);

        Department Update(int id, Department department, string modifiedBy);

        void Delete(int id, string modifiedBy);
    }

    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDbContext context) : base(context) { }

        public void Dispose()
        {
            _context.Dispose();
        }

        public override Department Save(Department department, string createdBy)
        {
            department.CreatedBy = createdBy;
            _context.Department.Add(department);
            _context.SaveChanges();

            return department;
        }

        public override Department Update(int id, Department department, string modifiedBy)
        {
            var currentData = GetById(id);

            currentData.DepartmentName = department.DepartmentName;
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

        public IEnumerable<Department> GetDepartmentDataTable(DatatablesRequest dataTable)
        {
            return _context.Database.SqlQuery<Department>("usp_Department_GetDepartmentDatatable @p0, @p1, @p2, @p3, @p4 ", parameters: new object[] { dataTable.Search, dataTable.Start, dataTable.Length, dataTable.SortColumnName, dataTable.SortDir }).ToList();
        }

        public int GetTotalDepartmentFiltered(string search)
        {
            return _context.Database.SqlQuery<int>("usp_Department_GetTotalDepartmentFiltered @p0", parameters: new object[] { search }).Single();
        }
    }
}
