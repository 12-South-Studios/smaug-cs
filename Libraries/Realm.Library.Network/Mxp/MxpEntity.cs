using Realm.Library.Common;

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
        public string Value { get; private set; }

        /// <summary>
        /// Override ToString to create the actual mxp entity value
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("!ELEMENT {0} \"{1}\"", Name, Value);
        }
    }
}