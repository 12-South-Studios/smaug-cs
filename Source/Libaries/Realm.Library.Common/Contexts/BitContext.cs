using System;
using Realm.Library.Common.Entities;
using Realm.Library.Common.Extensions;

namespace Realm.Library.Common.Contexts

{
    /// <summary>
    ///
    /// </summary>
    public class BitContext : BaseContext<IEntity>, IBitContext
    {
        private int _bits;

        /// <summary>
        ///
        /// </summary>
        /// <param name="owner"></param>
        public BitContext(IEntity owner)
            : base(owner)
        {
            _bits = 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="bit"></param>
        /// <returns></returns>
        public bool HasBit(int bit)
        {
            return (_bits & bit) != 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool HasBit(Enum value)
        {
            return HasBit(value.GetValue());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="bit"></param>
        public void SetBit(int bit)
        {
            _bits |= bit;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        public void SetBit(Enum value)
        {
            SetBit(value.GetValue());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="bit"></param>
        public void UnsetBit(int bit)
        {
            _bits &= ~bit;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        public void UnsetBit(Enum value)
        {
            UnsetBit(value.GetValue());
        }

        /// <summary>
        ///
        /// </summary>
        public int GetBits => _bits;

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        public void SetBits(int value)
        {
            _bits = value;
        }
    }
}