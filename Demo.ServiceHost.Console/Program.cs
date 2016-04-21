using System;
using System.Security.Principal;
using System.Threading;
using System.Timers;
using System.Transactions;
using Core.Common.Core;
using Demo.Business.Bootstrapper;
using Demo.Business.Managers;
using Timer = System.Timers.Timer;
using Demo.Business.Managers.Monitoring;

namespace Demo.ServiceHost.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // the user must be in admin role to
            // approve orders (unattended process) 
            var principal = new GenericPrincipal(
                new GenericIdentity("Pingo"), 
                new[] { "DemoAdmin" });
            Thread.CurrentPrincipal = principal;

            // set up the dependency container because instantiating
            // shopping manager and dependencies of it (ShoppingManager)
            ObjectBase.Container = MefLoader.Init();

            System.Console.WriteLine("Starting up services...");
            System.Console.WriteLine("");

            // initalize the service manager
            var hostCustomerManager =
                new System.ServiceModel.ServiceHost(typeof(CustomerManager));
            var hostInventoryManager =
                new System.ServiceModel.ServiceHost(typeof(InventoryManager));
            var hostShoppingManager =
                new System.ServiceModel.ServiceHost(typeof(ShoppingManager));

            // start the service manager
            StartService(hostCustomerManager, "CustomerManager");
            StartService(hostInventoryManager, "InventoryManager");
            StartService(hostShoppingManager, "ShoppingManager");

            // adding an unattended process if needed
            var timer = new Timer(20000);
            timer.Elapsed += OnTimerElapsed;
            //timer.Start();

            System.Console.WriteLine("ShoppingManager monitoring started.");

            System.Console.WriteLine("");
            System.Console.WriteLine("Press [Enter] to stop services.");
            System.Console.WriteLine("");
            System.Console.ReadKey();

            //timer.Stop();

            StopService(hostCustomerManager, "CustomerManager");
            StopService(hostInventoryManager, "InventoryManager");
            StopService(hostShoppingManager, "ShoppingManager");

            System.Console.WriteLine("Press [Enter] to exit.");
            System.Console.WriteLine("");
            System.Console.ReadKey();
        }

        /// <summary>
        /// long running unanttended process
        /// transaction scoped
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            System.Console.WriteLine($"Looking for new orders => {DateTime.Now.ToLongTimeString()}");

            var shoppingManager = new ShoppingManager();
            var newlyCreatedOrders = shoppingManager.GetNewCarts();
            if (newlyCreatedOrders == null) return;

            foreach (var o in newlyCreatedOrders)
            {
                using (var scope = new TransactionScope())
                {
                    try
                    {
                        shoppingManager.SetCartAsApproved(o.CartId);
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine($"An exception occured when attempting to set the order with id {o.CartId} as approved.");
                    }
                }
            }
        }

        /// <summary>
        /// logging the service manager parameter
        /// </summary>
        /// <param name="host"></param>
        /// <param name="service"></param>
        static void StartService(System.ServiceModel.ServiceHost host, string service)
        {
            var behavior = host.Description.Behaviors.Find<OperationReportServiceBehaviorAttribute>();
            if (behavior == null)
            {
                behavior = new OperationReportServiceBehaviorAttribute(true);
                host.Description.Behaviors.Add(behavior);
            }

            // register monitoring events (before- / after- operation call)
            behavior.ServiceOperationCalled += (sender, args) =>
            {
                System.Console.WriteLine(string.Format("{0} - {3} => '{1}.{2}'", 
                    DateTime.Now.ToLongTimeString(), 
                    args.ServiceName, 
                    args.OperationName,
                    args.Direction.ToUpper().Equals("UP") ? "AFTER " : "BEFORE"));

                if (args.Direction.ToUpper().Equals("UP"))
                {
                    System.Console.WriteLine("");
                }
            };

            host.Open();
            System.Console.WriteLine("Service => {0} started...", service);

            foreach (var endpoint in host.Description.Endpoints)
            {
                System.Console.WriteLine("\t=> Listening on endpoint:");
                System.Console.WriteLine("\t\tAddress : {0}", endpoint.Address.Uri);
                System.Console.WriteLine("\t\tBinding : {0}", endpoint.Binding.Name);
                System.Console.WriteLine("\t\tContract: {0}", endpoint.Contract.ConfigurationName);
            }

            System.Console.WriteLine();
        }

        /// <summary>
        /// do some housekeeping
        /// </summary>
        /// <param name="host"></param>
        /// <param name="serviceDescription"></param>
        static void StopService(System.ServiceModel.ServiceHost host, string serviceDescription)
        {
            // do not abort!!!
            host.Close();
            System.Console.WriteLine("Service {0} stopped.", serviceDescription);
        }
    }
}
