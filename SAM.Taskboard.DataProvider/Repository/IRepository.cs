using System;
using System.Collections.Generic;

namespace SAM.Taskboard.DataProvider.Repository
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        TEntity Get(int id);
        List<TEntity> Get(int amount, int skip);
        void Create(TEntity item);
        void Update(TEntity item);
        void Delete(int id);
    }
}
