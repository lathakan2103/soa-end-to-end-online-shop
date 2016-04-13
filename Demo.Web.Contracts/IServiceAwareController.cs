using System.Collections.Generic;
using Core.Common.Contracts;

namespace Demo.Web.Contracts
{
    public interface IServiceAwareController
    {
        void RegisterDisposableServices(List<IServiceContract> disposableServices);

        List<IServiceContract> DisposableServices { get; }
    }
}
