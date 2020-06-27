using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DocTrack.Data.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById(int? id);

        IEnumerable<T> GetAll();

        IEnumerable<T> GetWhere(Expression<Func<T, bool>> expression);

        T Save(T entity, string createdBy);

        T Update(int id, T entity, string modifiedBy);

        void Delete(int id, string modifiedBy);
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public virtual T GetById(int? id)
        {
            return _context.Set<T>().Find(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public virtual IEnumerable<T> GetWhere(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public virtual T Save(T entity, string createdBy)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public virtual T Update(int id, T entity, string modifiedBy)
        {
            if (entity == null)
                return null;

            T current = GetById(id);
            if (current != null)
            {
                _context.Entry(current).CurrentValues.SetValues(entity);
                _context.SaveChanges();
            }
            return entity;
        }

        public virtual void Delete(int id, string modifiedBy)
        {
            T current = GetById(id);
            if (current == null)
                return;
            _context.Set<T>().Remove(current);
            _context.SaveChanges();
        }
    }
}
