using DocTrack.Entity.Models;
using System;
using System.Linq;

namespace DocTrack.Data.Repository
{
    public interface ISequenceNumberRepository : IDisposable
    {
        SequenceNumber GetById(int? id);
        string GetSequenceNumber(int id);
        void UpdateSequenceNumber(int id);
    }

    public class SequenceNumberRepository : GenericRepository<SequenceNumber>, ISequenceNumberRepository
    {
        public SequenceNumberRepository(ApplicationDbContext context) : base(context)
        {

        }

        public string GetSequenceNumber(int id)
        {
            return _context.Database.SqlQuery<string>("usp_SequenceNumber_GetSequenceNumber @p0", parameters: new object[] { id }).Single();
        }

        public void UpdateSequenceNumber(int id)
        {
            SequenceNumber currentData = GetById(id);

            if (currentData == null)
                return;

            currentData.DocSequenceNumber += 1;

            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
