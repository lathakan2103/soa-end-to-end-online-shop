using System.ServiceModel;
using Demo.Business.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.ServiceHost.Tests
{
    [TestClass]
    public class ServiceAccessTests
    {
        [TestMethod]
        public void test_customer_manager_as_service()
        {
            var channelFactory = new ChannelFactory<ICustomerService>("");

            var proxy = channelFactory.CreateChannel();

            (proxy as ICommunicationObject).Open();

            channelFactory.Close();
        }

        [TestMethod]
        public void test_inventory_manager_as_service()
        {
            var channelFactory = new ChannelFactory<IInventoryService>("");

            var proxy = channelFactory.CreateChannel();

            (proxy as ICommunicationObject).Open();

            channelFactory.Close();
        }

        [TestMethod]
        public void test_shopping_manager_as_service()
        {
            var channelFactory = new ChannelFactory<IShoppingService>("");

            var proxy = channelFactory.CreateChannel();

            (proxy as ICommunicationObject).Open();

            channelFactory.Close();
        }
    }
}
