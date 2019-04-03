using SAM.Taskboard.DataProvider.Utility;
using System.Linq;

namespace SAM.Taskboard.DataProvider.Repository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> Get(int page, int pageSize);
        T Get(int id);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
    }
}
