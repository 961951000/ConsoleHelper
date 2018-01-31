using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using SimpleInjector;

namespace ConsoleHelper
{
    public class SimpleInjectorResolver : DefaultDependencyResolver
    {
        private readonly Container _container;
        public SimpleInjectorResolver(Container container)
        {
            _container = container;
        }

        public override object GetService(Type serviceType)
        {
            return _container.GetInstance(serviceType) ?? base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.GetAllInstances(serviceType) ?? base.GetServices(serviceType);
        }
    }
}
