using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Infrastructure.Tests.Data.Domain;
using Infrastructure.Data.Specification;
using Infrastructure.Tests.Data.Specification;
using Infrastructure.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Infrastructure.Tests.Data
{
    [TestClass]
    public class WithoutStorageTest
    {
        private ICustomerRepository _customerRepository;
        private IRepository _repository;
        private DbContext _context;

        [TestInitialize]
        public void SetUp()
        {
            DbContextBuilder<DbContext> builder = new DbContextBuilder<DbContext>("DefaultDb", new[] { "Infrastructure.Tests" }, true, true);
            _context = builder.BuildDbContext();

            _customerRepository = new CustomerRepository(_context);
            _repository = new GenericRepository(_context);
        }

        [TestCleanup]
        public void TearDown()
        {
            if ((_context != null) && (((IObjectContextAdapter)_context).ObjectContext.Connection.State == System.Data.ConnectionState.Open))
            {
                ((IObjectContextAdapter)_context).ObjectContext.Connection.Close();
                _context = null;
            }
        }

        [Ignore]
        [TestMethod]
        [TestCategory("Integration")]
        public void Test()
        {
            DoAction(() => CreateCustomer());
            DoAction(() => CreateProducts());
            DoAction(() => AddOrders());
            DoAction(() => FindOneCustomer());
            DoAction(() => FindManyOrdersForJohnDoe());
            DoAction(() => FindNewlySubscribed());
            DoAction(() => FindOrderWithInclude());
            DoAction(() => FindBySpecification());
            DoAction(() => FindByCompositeSpecification());
            DoAction(() => FindByConcretSpecification());
            DoAction(() => FindByConcretCompositeSpecification());
            DoAction(() => FindCategoryWithInclude());
            DoAction(() => UpdateProduct());
        }

        private void FindBySpecification()
        {
            Specification<Product> specification = new Specification<Product>(p => p.Price < 100);
            IEnumerable<Product> productsOnSale = _repository.Find(specification);
            Assert.AreEqual(2, productsOnSale.Count());
        }

        private void FindByCompositeSpecification()
        {
            IEnumerable<Product> products = _repository.Find(
                new Specification<Product>(p => p.Price < 100).And(new Specification<Product>(p => p.Name == "Windows XP Professional")));
            Assert.AreEqual(1, products.Count());
        }

        private void FindByConcretSpecification()
        {
            ProductOnSaleSpecification specification = new ProductOnSaleSpecification();
            IEnumerable<Product> productsOnSale = _repository.Find(specification);
            Assert.AreEqual(2, productsOnSale.Count());
        }

        private void FindByConcretCompositeSpecification()
        {
            IEnumerable<Product> products = _repository.Find(
                new AndSpecification<Product>(
                    new ProductOnSaleSpecification(),
                    new ProductByNameSpecification("Windows XP Professional")));
            Assert.AreEqual(1, products.Count());
        }

        private void FindOrderWithInclude()
        {
            var c = _customerRepository.FindByName("John", "Doe");
            List<Order> orders = _repository.Find<Order>(x => x.Customer.Id == c.Id).ToList();
            Console.Write("Found {0} Orders with {1} OrderLines", orders.Count(), orders.ToList()[0].OrderLines.Count);
        }

        private void CreateProducts()
        {
            Category osCategory = new Category { Name = "Operating System" };
            Category msProductCategory = new Category { Name = "MS Product" };

            _repository.Add(osCategory);
            _repository.Add(msProductCategory);

            var p1 = new Product { Name = "Windows Seven Professional", Price = 100 };
            p1.Categories.Add(osCategory);
            p1.Categories.Add(msProductCategory);
            _repository.Add(p1);

            var p2 = new Product { Name = "Windows XP Professional", Price = 20 };
            p2.Categories.Add(osCategory);
            p2.Categories.Add(msProductCategory);
            _repository.Add(p2);

            var p3 = new Product { Name = "Windows Seven Home", Price = 80 };
            p3.Categories.Add(osCategory);
            p3.Categories.Add(msProductCategory);
            _repository.Add(p3);

            var p4 = new Product { Name = "Windows Seven Ultimate", Price = 110 };
            p4.Categories.Add(osCategory);
            p4.Categories.Add(msProductCategory);
            _repository.Add(p4);

            var p5 = new Product { Name = "Windows Seven Premium", Price = 150 };
            p5.Categories.Add(osCategory);
            p5.Categories.Add(msProductCategory);
            _repository.Add(p5);

            _repository.UnitOfWork.SaveChanges();

            Console.Write("Saved five Products in 2 Category");
        }

        private void FindManyOrdersForJohnDoe()
        {
            var c = _customerRepository.FindByName("John", "Doe");
            var orders = _repository.Find<Order>(x => x.Customer.Id == c.Id);

            Console.Write("Found {0} Orders with {1} OrderLines", orders.Count(), orders.ToList()[0].OrderLines.Count);
        }

        private void FindNewlySubscribed()
        {
            var newCustomers = _customerRepository.NewlySubscribed();

            Console.Write("Found {0} new customers", newCustomers.Count);
        }

        private void AddOrders()
        {
            var c = _customerRepository.FindByName("John", "Doe");

            var winXp = _repository.FindOne<Product>(x => x.Name == "Windows XP Professional");
            var winSeven = _repository.FindOne<Product>(x => x.Name == "Windows Seven Professional");

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

            _repository.Add(o);
            _repository.UnitOfWork.SaveChanges();
            Console.Write("Saved one order");
        }

        private void CreateCustomer()
        {
            _customerRepository.UnitOfWork.BeginTransaction();

            var c = new Customer { Firstname = "John", Lastname = "Doe", Inserted = DateTime.Now };
            _customerRepository.Add(c);

            _customerRepository.UnitOfWork.CommitTransaction();
        }

        private void FindOneCustomer()
        {
            var c = _repository.FindOne<Customer>(x => x.Firstname == "John" &&
                                                    x.Lastname == "Doe");

            Console.Write("Found Customer: {0} {1}", c.Firstname, c.Lastname);
        }

        private void FindCategoryWithInclude()
        {
            var category = _repository.GetQuery<Category>(x => x.Name == "Operating System").Include(c => c.Products).SingleOrDefault();
            Assert.IsNotNull(category);
            Assert.IsTrue(category.Products.Count > 0);
        }

        private void UpdateProduct()
        {
            var output = _repository.FindOne<Product>(x => x.Name == "Windows XP Professional");
            Assert.IsNotNull(output);

            output.Name = "Windows XP Home";
            _repository.Update(output);
            _repository.UnitOfWork.SaveChanges();

            var updated = _repository.FindOne<Product>(x => x.Name == "Windows XP Home");
            Assert.IsNotNull(updated);
        }

        private static void DoAction(Expression<Action> action)
        {
            Console.Write("Executing {0} ... ", action.Body);

            var act = action.Compile();
            act.Invoke();

            Console.WriteLine();
        }
    }
}
