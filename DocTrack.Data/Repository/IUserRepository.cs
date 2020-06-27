using DocTrack.Entity.Models;
using System;
using System.Linq;

namespace DocTrack.Data.Repository
{
    public interface IUserRepository : IDisposable
    {
        Users GetUserByUsername(string username);

        Users GetById(int? id);
    }

    public class UserRepository : GenericRepository<Users>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Users GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(acc => acc.Username == username);
        }
    }
}
