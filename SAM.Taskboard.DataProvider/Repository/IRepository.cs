using System;

namespace SAM.Taskboard.DataProvider.Repository
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        TEntity Get(int id);
        void Create(TEntity item);
        void Update(TEntity item);
        void Delete(int id);
    }
}
