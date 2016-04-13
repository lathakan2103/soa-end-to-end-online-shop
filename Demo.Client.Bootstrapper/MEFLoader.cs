using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using Demo.Client.Proxies;
using Demo.Web.Controllers.Mvc;
using Demo.Admin.ViewModels;

namespace Demo.Client.Bootstrapper
{
    public static class MEFLoader
    {
        public static CompositionContainer Init()
        {
            return Init(null);
        }

        public static CompositionContainer Init(ICollection<ComposablePartCatalog> catalogParts)
        {
            var catalog = new AggregateCatalog();

            catalog.Catalogs.Add(new AssemblyCatalog(typeof(CustomerClient).Assembly));

            // musst add catalog with controllers
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(AccountController).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(MainViewModel).Assembly));

            if (catalogParts != null)
                foreach (var part in catalogParts)
                    catalog.Catalogs.Add(part);

            var container = new CompositionContainer(catalog);
            
            return container;
        }

    }
}
