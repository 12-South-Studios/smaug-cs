using System;
using System.Diagnostics.CodeAnalysis;
using Realm.Library.Common.Entities;

namespace Realm.Library.Common.Objects

{
    /// <summary>
    /// Defines a generic disposable entity
    /// </summary>
    public abstract class Entity : Cell, IEntity, IDisposable
    {
        /// <summary>
        /// Default constructor for the Entity class
        /// </summary>
        protected Entity(long id, string name)
        {
            ID = id;
            Name = name;
        }

        /// <summary>
        /// Deconstructor
        /// </summary>
        ~Entity()
        {
            Dispose(false);
        }

        /// <summary>
        /// Overrides the Equals function to perform a comparison of ID and Name
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj.IsNull() || GetType() != obj.GetType())
                return false;

            var entity = obj.CastAs<Entity>();
            return entity.IsNotNull() && ID == entity.ID && Name.Equals(entity.Name, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Overrides the GetHashCode to incorporate a true hash of the object properties
        /// </summary>
        public override int GetHashCode()
        {
            var result = 0;
            result = (result * 397) ^ ID.GetHashCode();
            result = (result * 397) ^ Name.GetHashCode();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (((object)a == null) || ((object)b == null)) return false;
            return a.ID == b.ID && a.Name.Equals(b.Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        #region IDisposable

        /// <summary>
        /// Overrides the base Dispose to make this object disposable
        /// </summary>
        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            Dispose(true);

            // Use SupressFinalize in case a subclass
            // of this type implements a finalizer.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose of any internal resources
        /// </summary>
        /// <param name="disposing"></param>
        [ExcludeFromCodeCoverage]
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Nothing to dispose
            }
        }

        #endregion IDisposable
    }
}