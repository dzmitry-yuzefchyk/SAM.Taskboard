using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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

        public int Count(Func<TEntity, bool> where)
        {
            return model.Where(where).Count();
        }

        public IEnumerable<TEntity> Get(int amount, int skip, Func<TEntity, object> orderBy, Func<TEntity, bool> where)
        {
            return model.Where(where).OrderBy(orderBy).Skip(skip).Take(amount);
        }

        public IEnumerable<TEntity> Get(Func<TEntity, bool> where)
        {
            return model.Where(where);
        }

        public IEnumerable<TEntity> Get(Func<TEntity, bool> where, Func<TEntity, object> orderBy, string orderDirection)
        {
            string direction = orderDirection.ToUpper();

            if (direction == "ASC")
            {
                return model.Where(where).OrderBy(orderBy);
            }
            else
            {
                return model.Where(where).OrderByDescending(orderBy);
            }
        }

        public TEntity GetFirstOrDefaultWhere(Func<TEntity, bool> where)
        {
            return model.Where(where).FirstOrDefault();
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
