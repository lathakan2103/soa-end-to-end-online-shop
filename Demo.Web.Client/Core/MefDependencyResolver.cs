using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Web.Mvc;
using Core.Common.Extensions;

namespace Demo.Web.Client.Core
{
    public class MefDependencyResolver : IDependencyResolver
    {
        public MefDependencyResolver(CompositionContainer container)
        {
            this._container = container;
        }

        private readonly CompositionContainer _container;

        public object GetService(Type serviceType)
        {
            return this._container.GetExportedValueByType(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this._container.GetExportedValuesByType(serviceType);
        }
    }
}
