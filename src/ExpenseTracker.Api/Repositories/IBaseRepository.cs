using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExpenseTracker.Api.Repositories
{
    public interface IBaseRepository<TContext>
    {
        IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class;
        TEntity GetById<TEntity>(int Id) where TEntity : class;
        bool Create<TEntity>(TEntity entity) where TEntity : class;
        void CreateMany<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        bool Delete<TEntity>(int Id) where TEntity : class;
        bool Update<TEntity>(TEntity entity) where TEntity : class;
        bool UpdateById<TEntity>(int Id) where TEntity : class;
        bool Save();
        void Dispose();
    }
}
