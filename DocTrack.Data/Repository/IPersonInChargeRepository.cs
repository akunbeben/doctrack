using DocTrack.Common;
using DocTrack.Entity.Dto;
using DocTrack.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DocTrack.Data.Repository
{
    public interface IPersonInChargeRepository : IDisposable
    {
        IEnumerable<PersonInChargeDto> GetPersonInChargeDataTable(DatatablesRequest dataTable);

        int GetTotalPersonInChargeFiltered(string search);

        IEnumerable<PersonInCharge> GetAll();

        IEnumerable<PersonInCharge> GetWhere(Expression<Func<PersonInCharge, bool>> predicate);

        PersonInCharge GetById(int? id);

        PersonInCharge Save(PersonInCharge personInCharge, string createdBy);

        PersonInCharge Update(int id, PersonInCharge personInCharge, string modifiedBy);

        void Delete(int id, string modifiedBy);
    }

    public class PersonInChargeRepository : GenericRepository<PersonInCharge>, IPersonInChargeRepository
    {
        public PersonInChargeRepository(ApplicationDbContext context) : base(context) { }

        public void Dispose()
        {
            _context.Dispose();
        }

        public override PersonInCharge Save(PersonInCharge entity, string createdBy)
        {
            entity.CreatedBy = createdBy;

            _context.PersonInCharge.Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public override PersonInCharge Update(int id, PersonInCharge entity, string modifiedBy)
        {
            var currentData = GetById(id);

            currentData.PersonInChargeName = entity.PersonInChargeName;
            currentData.DepartmentId = entity.DepartmentId;
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

        public IEnumerable<PersonInChargeDto> GetPersonInChargeDataTable(DatatablesRequest dataTable)
        {
            return _context.Database.SqlQuery<PersonInChargeDto>("usp_PersonInCharge_GetPersonInChargeDatatable @p0, @p1, @p2, @p3, @p4 ", parameters: new object[] { dataTable.Search, dataTable.Start, dataTable.Length, dataTable.SortColumnName, dataTable.SortDir }).ToList();
        }

        public int GetTotalPersonInChargeFiltered(string search)
        {
            return _context.Database.SqlQuery<int>("usp_PersonInCharge_GetTotalPersonInChargeFiltered @p0", parameters: new object[] { search }).Single();
        }
    }
}
