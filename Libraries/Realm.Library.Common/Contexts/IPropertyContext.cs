using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Realm.Library.Common
{
    public interface IPropertyContext
    {
        T GetProperty<T>(string name);

        void SetProperty(String name, object value, PropertyTypeOptions bits = 0);

        void SetProperty(Enum prop, object value, PropertyTypeOptions bits = 0);

        bool HasProperty(string name);

        bool IsPersistable(string name);

        bool IsVolatile(string name);

        bool IsVisible(string name);

        bool RemoveProperty(string name);

        IEnumerable<string> PropertyKeys { get; }

        int Count { get; }

        string GetPropertyBits(string name);
    }
}