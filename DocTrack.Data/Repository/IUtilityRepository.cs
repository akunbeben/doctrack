using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DocTrack.Entity.Models;

namespace DocTrack.Data.Repository
{
    public interface IUtilityRepository : IDisposable
    {
        IEnumerable<UtilityDocument> GetWhere(Expression<Func<UtilityDocument, bool>> predicate);
    }

    public class UtilityRepository : GenericRepository<UtilityDocument>, IUtilityRepository
    {
        public UtilityRepository(ApplicationDbContext context) : base(context) { }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
