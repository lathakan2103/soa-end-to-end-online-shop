using Core.Common.Contracts;
using Core.Common.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Demo.Client.Bootstrapper;
using Demo.Client.Contracts;
using Demo.Client.Proxies.Service_Procies;

namespace Demo.Client.Proxies.Tests
{
    [TestClass]
    public class ProxyObtainmentTests
    {
        [TestInitialize]
        public void Initialize()
        {
            ObjectBase.Container = MEFLoader.Init();
        }

        [TestMethod]
        public void test_obtain_proxy_from_container_using_service_contract()
        {
            var proxy = ObjectBase.Container.GetExportedValue<ICustomerService>();

            Assert.IsTrue(proxy is CustomerClient);
        }

        [TestMethod]
        public void test_obtain_proxy_from_service_factory()
        {
            IServiceFactory factory = new ServiceFactory();

            var proxy = factory.CreateClient<IInventoryService>();

            Assert.IsTrue(proxy is InventoryClient);
        }

        [TestMethod]
        public void test_obtain_service_factory_and_proxy_from_container()
        {
            var factory = ObjectBase.Container.GetExportedValue<IServiceFactory>();

            var proxy = factory.CreateClient<IShoppingService>();

            Assert.IsTrue(proxy is ShoppingClient);
        }
    }
}
