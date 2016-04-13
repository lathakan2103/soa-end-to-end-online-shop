using System;
using System.Collections.Generic;
using System.Data.Entity;
using Demo.Business.Entities;

namespace Demo.Data
{
    public class DemoDbInitializer : DropCreateDatabaseIfModelChanges<DemoContext>
    {
        protected override void Seed(DemoContext context)
        {
            var customers = new List<Customer>
            {
                new Customer
                {
                    Age = 44,
                    FirstName = "Aleksandar",
                    LastName = "Ristic",
                    LoginEmail = "al.ri@gmx.at",
                    Street = "Fernkorngasse",
                    Hausnumber = "56/210",
                    ZipCode = "1100",
                    City = "Vienna",
                    IsActive = true,
                    State = "Austria",
                    CreditCard = "1234123412341234",
                    ExpirationDate = "12/16",
                    
                },
                new Customer
                {
                    Age = 20,
                    FirstName = "Denis",
                    LastName = "Straus",
                    LoginEmail = "denis@straus.com",
                    Street = "Langobardenstrasse",
                    Hausnumber = "128/22/1",
                    ZipCode = "1220",
                    City = "Vienna",
                    IsActive = false,
                    State = "Austria",
                    CreditCard = "9632963296329632",
                    ExpirationDate = "04/18"
                }
            };

            var products = new List<Product>
            {
                new Product
                {
                    ArticleNumber = "36swdrf8",
                    Name = "MJ I",
                    Description = "Michael Jordan Air - 1988",
                    IsActive = true,
                    Price = 100
                },
                new Product
                {
                    ArticleNumber = "854698d7",
                    Name = "MJ II",
                    Description = "Michael Jordan Air Retro shoes - 1989",
                    IsActive = true,
                    Price = 200
                },
                new Product
                {
                    ArticleNumber = "45879dfv",
                    Name = "MJ III",
                    Description = "Michael Jordan Air - 1990",
                    IsActive = true,
                    Price = 300
                },
                new Product
                {
                    ArticleNumber = "as854fr6",
                    Name = "MJ IV",
                    Description = "Michael Jordan Air Retro shoes - 1991",
                    IsActive = true,
                    Price = 400
                },
                new Product
                {
                    ArticleNumber = "nmgv5896",
                    Name = "MJ V",
                    Description = "Michael Jordan Air - 1992",
                    IsActive = true,
                    Price = 500
                },
                new Product
                {
                    ArticleNumber = "fgrn5874",
                    Name = "MJ VI",
                    Description = "Michael Jordan Air Retro shoes - the coolest  1993",
                    IsActive = true,
                    Price = 600
                }
            };

            var carts = new List<Cart>
            {
                new Cart
                {
                    CustomerId = 1,
                    Created = DateTime.Now.AddDays(-2),
                    CartItemId = new []{1,2}
                }
            };

            var cartItems = new List<CartItem>
            {
                new CartItem
                {
                    CartId = 1,
                    ProductId = 1,
                    Quantity = 1
                },
                new CartItem
                {
                    CartId = 1,
                    ProductId = 2,
                    Quantity = 2
                }
            };

            foreach (var c in customers)
            {
                context.CustomerSet.Add(c);
            }

            foreach (var p in products)
            {
                context.ProductSet.Add(p);
            }

            foreach (var c in carts)
            {
                context.CartSet.Add(c);
            }

            foreach (var i in cartItems)
            {
                context.CartItemSet.Add(i);
            }

            base.Seed(context);
        }
    }
}
