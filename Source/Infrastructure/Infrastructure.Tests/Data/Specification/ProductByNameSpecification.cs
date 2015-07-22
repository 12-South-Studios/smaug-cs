using Infrastructure.Data.Specification;
using Infrastructure.Tests.Data.Domain;

namespace Infrastructure.Tests.Data.Specification
{
    public class ProductByNameSpecification : Specification<Product>
    {
        public ProductByNameSpecification(string nameToMatch)
            : base(p => p.Name == nameToMatch)
        { 
        }        
    }
}
