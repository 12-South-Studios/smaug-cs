using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Realm.Library.Common
{
    public class PropertyContext : BaseContext<IEntity>, IPropertyContext
    {
        private readonly Dictionary<string, Property> _properties = new Dictionary<string, Property>();

        public PropertyContext(IEntity owner)
            : base(owner)
        {
        }

        public T GetProperty<T>(string name)
        {
            return _properties.ContainsKey(name) ? (T)_properties[name].Value : default(T);
        }

        public void SetProperty(String name, object value, PropertyTypeOptions bits = 0)
        {
            if (_properties.ContainsKey(name))
            {
                var property = _properties[name];
                if (property.Volatile)
                    property.Value = value;
            }
            else
            {
                _properties.Add(name, new Property(name)
                    {
                        Value = value,
                        Persistable = (bits & PropertyTypeOptions.Persistable) != 0,
                        Volatile = (bits & PropertyTypeOptions.Volatile) != 0,
                        Visible = (bits & PropertyTypeOptions.Visible) != 0
                    });
            }
        }

        public void SetProperty(Enum aEnum, object aValue, PropertyTypeOptions bits = 0)
        {
            SetProperty(aEnum.GetName(), aValue, bits);
        }

        public bool HasProperty(string name)
        {
            return _properties.ContainsKey(name);
        }

        public bool IsPersistable(string name)
        {
            var returnVal = false;

            if (_properties.ContainsKey(name))
            {
                var property = _properties[name];
                returnVal = property.IsNotNull() && property.Persistable;
            }

            return returnVal;
        }

        public bool IsVolatile(string name)
        {
            var returnVal = false;

            if (_properties.ContainsKey(name))
            {
                var property = _properties[name];
                returnVal = property.IsNotNull() && property.Volatile;
            }

            return returnVal;
        }

        public bool IsVisible(string name)
        {
            var returnVal = false;

            if (_properties.ContainsKey(name))
            {
                var property = _properties[name];
                returnVal = property.IsNotNull() && property.Visible;
            }

            return returnVal;
        }

        public bool RemoveProperty(string name)
        {
            return _properties.ContainsKey(name) && _properties.Remove(name);
        }

        public IEnumerable<string> PropertyKeys { get { return _properties.Keys; } }

        public int Count { get { return _properties.Count; } }

        public string GetPropertyBits(string name)
        {
            var returnVal = string.Empty;

            if (_properties.ContainsKey(name))
            {
                var property = _properties[name];

                var bits = string.Empty;
                if (property.Persistable) bits += "p";
                if (property.Volatile) bits += "v";
                if (property.Visible) bits += "i";

                returnVal = bits;
            }

            return returnVal;
        }
    }
}