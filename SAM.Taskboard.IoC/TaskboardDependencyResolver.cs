using Unity;
using System.Web.Mvc;
using SAM.Taskboard.DataProvider;
using System;
using System.Collections.Generic;

namespace SAM.Taskboard.IoC
{
    public class TaskboardDependencyResolver : IDependencyResolver
    {
        readonly IUnityContainer container;

        public TaskboardDependencyResolver()
        {
            container = new UnityContainer();
            container.RegisterType<IUnitOfWork, UnitOfWork>();
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return container.Resolve(serviceType);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return container.ResolveAll(serviceType);
            }
            catch
            {
                return null;
            }
        }
    }
}
