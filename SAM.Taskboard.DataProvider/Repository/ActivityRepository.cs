using System;
using System.Linq;
using SAM.Taskboard.DataProvider.Models;
using SAM.Taskboard.DataProvider.Utility;

namespace SAM.Taskboard.DataProvider.Repository
{
    public class ActivityRepository : Pagination<Activity>, IRepository<Activity>
    {
        private TaskboardContext context;

        public ActivityRepository(TaskboardContext context)
        {
            this.context = context;
        }
        public Activity Get(int id)
        {
            return context.Activities.Find(id);
        }
        public IQueryable<Activity> GetAll()
        {
            return context.Activities;
        }
        public IQueryable<Activity> Get(int page, int pageSize)
        {
            CurrentPage = page;
            PageSize = pageSize;
            RowsCount = context.Activities.Count();

            double pageCount = (double)RowsCount / pageSize;
            PageCount = (int)Math.Ceiling(pageCount);

            int skip = (page - 1) * pageSize;
            var result = context.Activities.Skip(skip).Take(PageSize);

            return result;
        }
        public void Create(Activity item)
        {
            context.Activities.Add(item);
        }
        public void Delete(int id)
        {
            Activity activity = context.Activities.Find(id);
            if (activity != null)
                context.Activities.Remove(activity);
        }
        public void Update(Activity item)
        {
            throw new System.NotImplementedException(); // TODO implement
        }
    }
}
