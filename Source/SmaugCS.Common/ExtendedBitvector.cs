using System.Linq;

namespace SmaugCS.Common;

/// <summary>
/// 
/// </summary>
public class ExtendedBitvector
{
    /// <summary>
    /// 
    /// </summary>
    public const int INTBITS = 32;

    /// <summary>
    /// Extended Bitmask (INTBITS - 1)
    /// </summary>
    public const int XBM = INTBITS - 1;

    /// <summary>
    /// Right-Shift Value.  Square-Root of XBM + 1
    /// </summary>
    public const int RSV = 5;

    /// <summary>
    /// Integers in an ExtendedBitvector
    /// </summary>
    public const int XBI = 4;

    /// <summary>
    /// 
    /// </summary>
    public const int MAX_BITS = XBI * INTBITS;

    private int[] _bits;

    /// <summary>
    /// 
    /// </summary>
    public ExtendedBitvector()
    {
            _bits = new int[XBI];
            ClearBits();
        }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="clone"></param>
    public ExtendedBitvector(ExtendedBitvector clone)
    {
            _bits = new int[XBI];
            for (var x = 0; x < XBI; x++)
                _bits[x] = clone.Bits[x];
        }

    /// <summary>
    /// 
    /// </summary>
    public int[] Bits => _bits.ToArray();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bit"></param>
    /// <returns></returns>
    public bool IsSet(int bit)
    {
            var result = _bits[bit >> RSV] & 1 << (bit & XBM);
            return result != 0;
        }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bit"></param>
    public void SetBit(int bit)
    {
            _bits[bit >> RSV] |= 1 << (bit & XBM);
        }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bits"></param>
    public void SetBits(ExtendedBitvector bits)
    {
            for (var x = 0; x < XBI; x++)
                _bits[x] |= bits.Bits[x];
        }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bit"></param>
    public void RemoveBit(int bit)
    {
            _bits[bit >> RSV] &= ~(1 << (bit & XBM));
        }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bits"></param>
    public void RemoveBits(ExtendedBitvector bits)
    {
            for (var x = 0; x < XBI; x++)
                _bits[x] &= ~bits.Bits[x];
        }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bit"></param>
    public void ToggleBit(int bit)
    {
            _bits[bit >> RSV] ^= 1 << (bit & XBM);
        }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bits"></param>
    public void ToggleBits(ExtendedBitvector bits)
    {
            for (var x = 0; x < XBI; x++)
                _bits[x] ^= bits.Bits[x];
        }

    /// <summary>
    /// 
    /// </summary>
    public void ClearBits()
    {
            for (var x = 0; x < XBI; x++)
                _bits[x] = 0;
        }

    /// <summary>
    /// 
    /// </summary>
    public bool IsEmpty() => _bits.All(x => x == 0);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bits"></param>
    /// <returns></returns>
    public int HasBits(ExtendedBitvector bits)
    {
            for (var x = 0; x < XBI; x++)
            {
                var bit = _bits[x] & bits.Bits[x];
                if (bit != 0) return bit;
            }

            return 0;
        }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bits"></param>
    /// <returns></returns>
    public bool SameBits(ExtendedBitvector bits)
    {
            for (var x = 0; x < XBI; x++)
            {
                if (_bits[x] != bits.Bits[x])
                    return false;
            }
            return true;
        }

    /// <summary>
    /// AKA meb
    /// </summary>
    /// <param name="bit"></param>
    public static ExtendedBitvector Meb(int bit)
    {
            var bits = new ExtendedBitvector();
            bits.ClearBits();

            if (bit >= 0)
                bits.SetBit(bit);

            return bits;
        }

    /// <summary>
    /// AKA multimeb
    /// </summary>
    /// <param name="bit"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static ExtendedBitvector MultiMeb(int bit, params int[] args)
    {
            var bits = new ExtendedBitvector();
            bits.ClearBits();

            if (bit < 0) return bits;

            bits.SetBit(bit);

            foreach (var arg in args)
                bits.SetBit(arg);

            return bits;
        }
}