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
    public class UseMyDbContextTest
    {
        private ICustomerRepository _customerRepository;
        private IRepository _repository;
        private MyDbContext _context;

        [TestInitialize]
        public void SetUp()
        {
            Database.SetInitializer(new DataSeedingInitializer());
            _context = new MyDbContext("DefaultDb");
            
            _customerRepository = new CustomerRepository(_context);
            _repository = new GenericRepository(_context);
        }

        [Ignore]
        [TestMethod]
        [TestCategory("Integration")]
        public void GenerateDatabaseScriptTest()
        {
            string script = ((IObjectContextAdapter)_context).ObjectContext.CreateDatabaseScript();
            // for debugging
            Console.WriteLine(script);
            Assert.IsTrue(!string.IsNullOrEmpty(script));
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
            DoAction(() => FindOneCustomer());
            DoAction(() => FindCategoryWithInclude());
            DoAction(() => FindManyOrdersForJohnDoe());
            DoAction(() => FindNewlySubscribed());
            DoAction(() => FindBySpecification());
            DoAction(() => FindByCompositeSpecification());
            DoAction(() => FindByConcretSpecification());
            DoAction(() => FindByConcretCompositeSpecification());
            DoAction(() => UpdateProduct());
        }
        
        private void FindCategoryWithInclude()
        {
            var category = _repository.GetQuery<Category>(x => x.Name == "Operating System").Include(c => c.Products).SingleOrDefault();
            Assert.IsNotNull(category);
            Assert.IsTrue(category.Products.Count > 0);
        }        

        private void FindManyOrdersForJohnDoe()
        {
            var customer = _customerRepository.FindByName("John", "Doe");
            var orders = _repository.Find<Order>(x => x.Customer.Id == customer.Id);

            Console.Write("Found {0} Orders with {1} OrderLines", orders.Count(), orders.ToList()[0].OrderLines.Count);
        }

        private void FindNewlySubscribed()
        {
            var newCustomers = _customerRepository.NewlySubscribed();

            Console.Write("Found {0} new customers", newCustomers.Count);
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

        private void FindOneCustomer()
        {
            var c = _repository.FindOne<Customer>(x => x.Firstname == "John" &&
                                                    x.Lastname == "Doe");

            Console.Write("Found Customer: {0} {1}", c.Firstname, c.Lastname);
        }

        private void UpdateProduct()
        {
            _repository.UnitOfWork.BeginTransaction();

            var output = _repository.FindOne<Product>(x => x.Name == "Windows XP Professional");
            Assert.IsNotNull(output);

            output.Name = "Windows XP Home";
            _repository.Update(output);
            _repository.UnitOfWork.CommitTransaction();

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
