﻿using Unity;
using System.Web.Mvc;
using SAM.Taskboard.DataProvider;
using System;
using System.Collections.Generic;
using SAM.Taskboard.Logic.Services;

namespace SAM.Taskboard.IoC
{
    public class TaskboardDependencyResolver : IDependencyResolver
    {
        readonly IUnityContainer container;

        public TaskboardDependencyResolver()
        {
            container = new UnityContainer();
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IProjectService, ProjectService>();
            container.RegisterType<IBoardService, BoardService>();
            container.RegisterType<ITaskService, TaskService>();
            container.RegisterType<IProfileService, ProfileService>();
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
