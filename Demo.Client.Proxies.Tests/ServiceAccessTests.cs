using Demo.Client.Proxies.Service_Procies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.Client.Proxies.Tests
{
    [TestClass]
    public class ServiceAccessTests
    {
        [TestMethod]
        public void test_inventory_client_connection()
        {
            var proxy = new InventoryClient();

            proxy.Open();
        }

        [TestMethod]
        public void test_shopping_client_connection()
        {
            var proxy = new ShoppingClient();

            proxy.Open();
        }

        [TestMethod]
        public void test_customer_client_connection()
        {
            var proxy = new CustomerClient();

            proxy.Open();
        }
    }
}
