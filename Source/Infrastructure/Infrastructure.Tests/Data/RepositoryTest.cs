using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Infrastructure.Tests.Data.Domain;
using Infrastructure.Data.Specification;
using Infrastructure.Tests.Data.Specification;
using Infrastructure.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Infrastructure.Tests.Data
{
    /// <summary>
    /// This class is used to test the _lab_ version of repository implementation
    /// </summary>
    [TestClass]
    public class RepositoryTest
    {
        private ICustomerRepository _customerRepository;
        private IRepository _repository;

        [TestInitialize]
        public void SetUp()
        {
            DbContextManager.InitStorage(new SimpleDbContextStorage());
            DbContextManager.Init("DefaultDb", new[] { "Infrastructure.Tests" }, true);

            _customerRepository = new CustomerRepository();
            _repository = new GenericRepository();
        }

        [TestCleanup]
        public void TearDown()
        {
            DbContextManager.CloseAllDbContexts();
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
            DoAction(() => FindByKey());
            DoAction(() => FindManyOrdersForJohnDoe());
            DoAction(() => FindNewlySubscribed());
            DoAction(() => FindOrderWithInclude());
            DoAction(() => FindBySpecification());
            DoAction(() => FindByCompositeSpecification());
            DoAction(() => FindByConcretSpecification());
            DoAction(() => FindByConcretCompositeSpecification());
            DoAction(() => FindByChainOfSpecifications());
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

        private void FindByChainOfSpecifications()
        {
            IEnumerable<Product> products = _repository.Find(
                new ProductOnSaleSpecification()
                    .And(new ProductByNameSpecification("Windows XP Professional")));
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

        private void FindByKey()
        {
            var c = _customerRepository.FindByName("John", "Doe");

            var customer = _customerRepository.GetByKey<Customer>(c.Id);

            Console.Write("Found Customer by its PK: {0}", customer != null);
        }

        private void CreateCustomer()
        {
            var c = new Customer { Firstname = "John", Lastname = "Doe", Inserted = DateTime.Now };
            _customerRepository.Add(c);

            _customerRepository.UnitOfWork.SaveChanges();
        }

        private void FindOneCustomer()
        {
            var c = _repository.FindOne<Customer>(x => x.Firstname == "John" &&
                                                    x.Lastname == "Doe");

            Console.Write("Found Customer: {0} {1}", c.Firstname, c.Lastname);
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
