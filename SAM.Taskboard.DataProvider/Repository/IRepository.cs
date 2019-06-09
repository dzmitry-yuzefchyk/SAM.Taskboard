using System;
using System.Collections;
using System.Collections.Generic;

namespace SAM.Taskboard.DataProvider.Repository
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        TEntity Get(int id);
        IEnumerable<TEntity> Get(int amount, int skip, Func<TEntity, object> orderBy, Func<TEntity, bool> where);
        int Count(Func<TEntity, bool> where);
        TEntity GetFirstOrDefaultWhere(Func<TEntity, bool> where);
        IEnumerable<TEntity> Get(Func<TEntity, bool> where);
        IEnumerable<TEntity> Get(Func<TEntity, bool> where, Func<TEntity, object> orderBy, string orderDirection);
        void Create(TEntity item);
        void Update(TEntity item);
        void Delete(int id);
        void Delete(Func<TEntity, bool> where);
    }
}
