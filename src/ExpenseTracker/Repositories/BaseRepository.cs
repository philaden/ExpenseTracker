using ExpenseTracker.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExpenseTracker.Repositories
{
    public class BaseRepository<TContext> : IBaseRepository<TContext>
    {
        private readonly ExpenseTrackerContext _context;
        public BaseRepository(ExpenseTrackerContext context)
        {
            _context = context;
        }

        public bool Create<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Set<TEntity>().Add(entity);
            return _context.SaveChanges() > 0;
        }

        public void CreateMany<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            _context.Set<TEntity>().AddRange(entities);
            _context.SaveChangesAsync();
        }

        public bool Delete<TEntity>(int Id) where TEntity : class
        {
            if (Id == 0 || Id < 0)
            {
                return false;
            }
            var entity = _context.Set<TEntity>().Find(Id);
            if (entity == null) return false;
            var dbSet = _context.Set<TEntity>();
            if (_context.Entry(entity).State == EntityState.Modified)
            {
                dbSet.Attach(entity);
                return true;
            } else
            {
                return false;
            }
        }

        public IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return _context.Set<TEntity>().ToList();
        }

        public TEntity GetById<TEntity>(int Id) where TEntity : class
        {
            return _context.Set<TEntity>().Find(Id);
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public bool Update<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null) return false;
            else if (_context.Entry(entity).State == EntityState.Modified)
            {
                var ent = _context.Entry(entity);
                ent.State = EntityState.Modified;
                return _context.SaveChanges() > 0;
            }
            else
            {
                return false;
            }
            
        }
        public bool UpdateById<TEntity>(int Id) where TEntity : class
        {
            if (Id == 0 || Id < 0)
            {
                return false;
            }
            var entity = _context.Set<TEntity>().Find(Id);
            if (_context.Entry(entity).State == EntityState.Modified)
            {
                var ent = _context.Entry(entity);
                ent.State = EntityState.Modified;
                return _context.SaveChanges() > 0;
            }
            else
            {
                return false;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
