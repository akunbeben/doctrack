using DocTrack.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DocTrack.Data.Repository
{
    public interface ILookupValueRepository : IDisposable
    {
        IEnumerable<LookupValue> GetAll();

        IEnumerable<LookupValue> GetWhere(Expression<Func<LookupValue, bool>> predicate);

        LookupValue GetById(int? id);

        LookupValue Save(LookupValue lookupValue, string createdBy);

        LookupValue Update(int id, LookupValue lookupValue, string modifiedBy);

        void Delete(int id, string modifiedBy);
    }

    public class LookupValueRepository : GenericRepository<LookupValue>, ILookupValueRepository
    {
        public LookupValueRepository(ApplicationDbContext context) : base(context) { }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
