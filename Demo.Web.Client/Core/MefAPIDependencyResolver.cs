using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Web.Http.Dependencies;
using Core.Common.Extensions;

namespace Demo.Web.Client.Core
{
    public class MefApiDependencyResolver : IDependencyResolver
    {
        public MefApiDependencyResolver(CompositionContainer container)
        {
            this._container = container;
        }

        private readonly CompositionContainer _container;

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public object GetService(Type serviceType)
        {
            return this._container.GetExportedValueByType(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this._container.GetExportedValuesByType(serviceType);
        }

        public void Dispose()
        {
        }
    }
}
