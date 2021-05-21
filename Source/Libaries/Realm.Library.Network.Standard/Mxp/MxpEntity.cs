using Realm.Library.Common;
using Realm.Library.Common.Objects;

namespace Realm.Library.Network.Mxp
{
    /// <summary>
    /// Defines an entity for the Mxp framework
    /// </summary>
    public class MxpEntity : Entity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public MxpEntity(long id, string name, string value)
            : base(id, name)
        {
            Validation.IsNotNullOrEmpty(value, "value");

            Value = value;
        }

        /// <summary>
        /// Gets the value of the entity string
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Override ToString to create the actual mxp entity value
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"!ELEMENT {Name} \"{Value}\"";
        }
    }
}