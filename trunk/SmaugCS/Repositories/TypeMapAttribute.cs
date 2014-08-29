using System;

namespace SmaugCS.Repositories
{
    public class TypeMapAttribute : Attribute
    {
        public Type Repository { get; set; }
        public Type Object { get; set; }

        public TypeMapAttribute(Type Repository = null, Type Object = null)
        {
            this.Repository = Repository;
            this.Object = Object;
        }
    }
}
