﻿using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Tests.Data.Domain;
using System.Data.Entity;

namespace Infrastructure.Tests.Data
{
    public class DataSeedingInitializer : DropCreateDatabaseAlways<MyDbContext>
    {
        protected override void Seed(MyDbContext context)
        {
            CreateCustomer(context);
            CreateProducts(context);
            AddOrders(context);
        }

        private static void CreateCustomer(MyDbContext context)
        {
            var c = new Customer { Firstname = "John", Lastname = "Doe", Inserted = DateTime.Now };
            context.Customers.Add(c);
            context.SaveChanges();
        }

        private static void CreateProducts(MyDbContext context)
        {
            Category osCategory = new Category { Name = "Operating System" };
            Category msProductCategory = new Category { Name = "MS Product" };

            context.Categories.Add(osCategory);
            context.Categories.Add(msProductCategory);

            var p1 = new Product { Name = "Windows Seven Professional", Price = 100 };
            p1.Categories.Add(osCategory);
            p1.Categories.Add(msProductCategory);
            context.Products.Add(p1);

            var p2 = new Product { Name = "Windows XP Professional", Price = 20 };
            p2.Categories.Add(osCategory);
            p2.Categories.Add(msProductCategory);
            context.Products.Add(p2);

            var p3 = new Product { Name = "Windows Seven Home", Price = 80 };
            p3.Categories.Add(osCategory);
            p3.Categories.Add(msProductCategory);
            context.Products.Add(p3);

            var p4 = new Product { Name = "Windows Seven Ultimate", Price = 110 };
            p4.Categories.Add(osCategory);
            p4.Categories.Add(msProductCategory);
            context.Products.Add(p4);

            var p5 = new Product { Name = "Windows Seven Premium", Price = 150 };
            p5.Categories.Add(osCategory);
            p5.Categories.Add(msProductCategory);
            context.Products.Add(p5);

            context.SaveChanges();
            Console.WriteLine("Saved five Products in 2 Category");
        }

        private static void AddOrders(MyDbContext context)
        {
            var c = context.Customers.SingleOrDefault(x => x.Firstname == "John" && x.Lastname == "Doe");

            var winXp = context.Products.SingleOrDefault(x => x.Name == "Windows XP Professional");
            var winSeven = context.Products.SingleOrDefault(x => x.Name == "Windows Seven Professional");

            var o = new Order
            {
                OrderDate = DateTime.Now,
                Customer = c,
                OrderLines = new List<OrderLine>
                {
                    new OrderLine { Price = 200, Product = winXp, Quantity = 1},
                    new OrderLine { Price = 699.99, Product = winSeven, Quantity = 5 }
                }
            };

            context.Orders.Add(o);
            context.SaveChanges();
            Console.WriteLine("Saved one order");
        }
    }
}
