using System;

// ReSharper disable once CheckNamespace
namespace Realm.Library.Common
{
    public class BitContext : BaseContext<IEntity>, IBitContext
    {
        private int _bits;

        public BitContext(IEntity owner)
            : base(owner)
        {
            _bits = 0;
        }

        public bool HasBit(int bit)
        {
            return (_bits & bit) != 0;
        }

        public bool HasBit(Enum value)
        {
            return HasBit(value.GetValue());
        }

        public void SetBit(int bit)
        {
            _bits |= bit;
        }

        public void SetBit(Enum value)
        {
            SetBit(value.GetValue());
        }

        public void UnsetBit(int bit)
        {
            _bits &= ~bit;
        }

        public void UnsetBit(Enum value)
        {
            UnsetBit(value.GetValue());
        }

        public int GetBits
        {
            get { return _bits; }
        }

        public void SetBits(int value)
        {
            _bits = value;
        }
    }
}