using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Runtime.Serialization;
using Core.Common.Contracts;
using Demo.Business.Entities;

namespace Demo.Data
{
    public class DemoContext : DbContext
    {
        public DemoContext() 
            : base("name=DemoDbConnection")
        {
            Database.SetInitializer<DemoContext>(new DemoDbInitializer());
        }

        public  DbSet<Customer> CustomerSet { get; set; } 
        public DbSet<Product> ProductSet { get; set; } 
        public DbSet<Cart> CartSet { get; set; } 
        public DbSet<CartItem> CartItemSet { get; set; } 

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // remove entity framweork's table pluralizing
            // for example: EF tries to find Persons table
            // instead of Person
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // set ignored xxx
            modelBuilder.Ignore<PropertyChangedEventHandler>();
            modelBuilder.Ignore<ExtensionDataObject>();
            modelBuilder.Ignore<IIdentifiableEntity>();

            // set fluent configuration of the entities 
            modelBuilder.Entity<Customer>().HasKey<int>(c => c.CustomerId).Ignore(c => c.EntityId);
            modelBuilder.Entity<Cart>().HasKey<int>(c => c.CartId).Ignore(c => c.EntityId).Ignore(c => c.Total);
            modelBuilder.Entity<Product>().HasKey<int>(p => p.ProductId).Ignore(p => p.EntityId);
            modelBuilder.Entity<CartItem>().HasKey<int>(c => c.CartItemId).Ignore(c => c.EntityId);
        }
    }
}
