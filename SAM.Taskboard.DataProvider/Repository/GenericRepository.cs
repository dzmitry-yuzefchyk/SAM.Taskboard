using System.Data.Entity;

namespace SAM.Taskboard.DataProvider.Repository
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private TaskboardContext context;
        private DbSet<TEntity> model;

        public GenericRepository(TaskboardContext context)
        {
            this.context = context;
            model = context.Set<TEntity>();
        }

        public void Create(TEntity item)
        {
            model.Add(item);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            TEntity entity = model.Find(id);

            if (entity != null)
            {
                model.Remove(entity);
                context.SaveChanges();
            }
        }

        public TEntity Get(int id)
        {
            return model.Find(id);
        }

        public void Update(TEntity item)
        {
            context.Entry(item).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
