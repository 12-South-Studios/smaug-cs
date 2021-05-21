
namespace Realm.Library.Common.Contexts

{
    /// <summary>
    /// Declares the contract for an object that can handle a bit field
    /// </summary>
    public interface IBitContext : IContext
    {
        /// <summary>
        /// Gets whether or not the object has the given bit
        /// </summary>
        /// <param name="bit">Bit value</param>
        /// <returns>Returns whether or not the object has the bit</returns>
        bool HasBit(int bit);

        /// <summary>
        /// Gets whether or not the object has the given enum (with a bit value)
        /// </summary>
        /// <param name="value">Enum value</param>
        /// <returns>Returns whether or not the object has the enum</returns>
        bool HasBit(System.Enum value);

        /// <summary>
        /// Sets the given bit onto the object
        /// </summary>
        /// <param name="bit">Bit value</param>
        void SetBit(int bit);

        /// <summary>
        /// Sets the given enum (with a bit value) onto the object
        /// </summary>
        /// <param name="value">Enum value</param>
        void SetBit(System.Enum value);

        /// <summary>
        /// Removes the bit value from the object
        /// </summary>
        /// <param name="bit">Bit value</param>
        void UnsetBit(int bit);

        /// <summary>
        /// Removes the enum (with a bit value) from the object
        /// </summary>
        /// <param name="value">Enum value</param>
        void UnsetBit(System.Enum value);

        /// <summary>
        /// Gets the total value of the object's bit-field
        /// </summary>
        /// <returns>Returns an integer value</returns>
        int GetBits { get; }

        /// <summary>
        /// Sets all of the bits for the object. Overwrites any existing.
        /// </summary>
        /// <param name="value">Bit value</param>
        void SetBits(int value);
    }
}