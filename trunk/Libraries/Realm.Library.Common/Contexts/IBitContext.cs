
// ReSharper disable once CheckNamespace
namespace Realm.Library.Common
{
    public interface IBitContext
    {
        bool HasBit(int bit);

        bool HasBit(System.Enum value);

        void SetBit(int bit);

        void SetBit(System.Enum value);

        void UnsetBit(int bit);

        void UnsetBit(System.Enum value);

        int GetBits { get; }

        void SetBits(int value);
    }
}