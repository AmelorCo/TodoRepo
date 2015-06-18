using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Todolist.Domain.Abstract;
using Todolist.Domain.Repository;
using TodoList.Service;

namespace Todolist.Web.Infrastructure
{
    public class AutofacConfig
    {
        public static void SetAutofacConfig()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerRequest();
            builder.RegisterType<GoalsService>().As<IGoalsService>().InstancePerRequest();
            builder.RegisterType<GoalsRepository>().As<IGoalsRepository>().InstancePerRequest();
            builder.RegisterType<PriorityService>().As<IPriorityService>().InstancePerRequest();
            builder.RegisterType<PriorityRepository>().As<IPriorityRepository>().InstancePerRequest();

            builder.RegisterFilterProvider();
            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container)); 
        }
    }
}