// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Class that defines a value or modification to an object. A Property
    /// can describe or modify an object.
    /// </summary>
    public class Property : Cell
    {
        private PropertyTypeOptions _propertyBits;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        public Property(string name)
        {
            _propertyBits = PropertyTypeOptions.None;
            Name = name;
        }

        /// <summary>
        /// Gets if the property is persisted to the database
        /// </summary>
        public bool Persistable
        {
            get { return (_propertyBits & PropertyTypeOptions.Persistable) != 0; }
            set
            {
                if (value)
                    _propertyBits |= PropertyTypeOptions.Persistable;
                else
                    _propertyBits &= ~PropertyTypeOptions.Persistable;
            }
        }

        /// <summary>
        /// Gets if the property is visible for examination
        /// </summary>
        public bool Visible
        {
            get { return (_propertyBits & PropertyTypeOptions.Visible) != 0; }
            set
            {
                if (value)
                    _propertyBits |= PropertyTypeOptions.Visible;
                else
                    _propertyBits &= ~PropertyTypeOptions.Visible;
            }
        }

        /// <summary>
        /// Gets if the property can be changed
        /// </summary>
        public bool Volatile
        {
            get { return (_propertyBits & PropertyTypeOptions.Volatile) != 0; }
            set
            {
                if (value)
                    _propertyBits |= PropertyTypeOptions.Volatile;
                else
                    _propertyBits &= ~PropertyTypeOptions.Volatile;
            }
        }

        /// <summary>
        /// Gets the value of the property as an object
        /// </summary>
        public object Value { get; set; }
    }
}