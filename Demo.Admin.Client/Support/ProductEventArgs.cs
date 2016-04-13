using Demo.Client.Entities;
using System;

namespace Demo.Admin.Client.Support
{
    public class ProductEventArgs : EventArgs
    {
        public ProductEventArgs(Product product, bool isNew)
        {
            this.Product = product;
            IsNew = isNew;
        }

        public Product Product { get; set; }
        public bool IsNew { get; set; }
    }
}
