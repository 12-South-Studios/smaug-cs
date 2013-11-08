using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmaugCS.Common
{
    public class ExtendedBitvector
    {
        #region Extended Bitvectors
#if !INTBITS
        private const int INTBITS = 32;
#endif

        public const int XBM = 31; // extended bitmask (INTBITS - 1)
        public const int RSV = 5; // Right-shift value (sqrt(XBM+1))
        public const int XBI = 4; // Integers in an extended bitvector
        public const int MAX_BITS = XBI*INTBITS;

        #endregion

        public uint[] Bits { get; private set; }

        public ExtendedBitvector()
        {
            Bits = new uint[XBI];
        }

        public ExtendedBitvector(ExtendedBitvector exBV)
        {
            Bits = new uint[XBI];
            SetBits(exBV);    
        }

        public bool IsEmpty()
        {
            return !Bits.Any(x => x != 0);
        }

        public uint HasBits(ExtendedBitvector checkBits)
        {
            uint bit;
            for (int x = 0; x < XBI; x++)
                if ((bit = (Bits[x] & checkBits.Bits[x])) != 0)
                    return bit;
            return 0;
        }

        public bool SameBits(ExtendedBitvector checkBits)
        {
            for (int x = 0; x < XBI; x++)
                if (Bits[x] != checkBits.Bits[x])
                    return false;
            return true;
        }

        public void SetBits(ExtendedBitvector bitsToSet)
        {
            for (int x = 0; x < XBI; x++)
                Bits[x] |= bitsToSet.Bits[x];
        }

        public void RemoveBits(ExtendedBitvector bitsToRemove)
        {
            for (int x = 0; x < XBI; x++)
                Bits[x] &= ~(bitsToRemove.Bits[x]);
        }

        public void ToggleBits(ExtendedBitvector bitsToToggle)
        {
            for (int x = 0; x < XBI; x++)
                Bits[x] ^= bitsToToggle.Bits[x];
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            int counter;

            for (counter = XBI - 1; counter > 0; counter--)
            {
                if (Bits[counter] == 0)
                    break;
            }

            for (int x = 0; x <= counter; x++)
            {
                sb.Append(Bits[x]);
                if (x < counter)
                    sb.Append("&");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flags"></param>
        /// <returns></returns>
        public string GetFlagString(IEnumerable<string> flags)
        {
            string flagString = string.Empty;
            string[] flagArray = flags.ToArray();

            for (int x = 0; x < MAX_BITS; ++x)
            {
                if (IsSet(x))
                    flagString += flagArray[x] + " ";
            }

            return flagString;
        }

        public bool IsSet(int bit)
        {
            return (Bits[bit >> RSV] & 1 << (bit & XBM)) > 0;
        }

        public uint SetBit(int bit)
        {
            return (Bits[bit >> RSV] |= (uint)1 << (bit & XBM));
        }

        public uint RemoveBit(int bit)
        {
            return (Bits[bit >> RSV] &= ~((uint)1 << (bit & XBM)));
        }

        public uint ToggleBit(int bit)
        {
            return (Bits[bit >> RSV] ^= (uint)1 << (bit & XBM));
        }

        public void ClearBits()
        {
            for (int x = 0; x < XBI; x++)
                Bits[x] = 0;
        }

        public static ExtendedBitvector Meb(int bit)
        {
            ExtendedBitvector bits = new ExtendedBitvector();
            bits.ClearBits();
            if (bit >= 0)
                bits.SetBit(bit);
            return bits;
        }

        public static ExtendedBitvector MultiMeb(int bit, params object[] args)
        {
            if (bit < 0)
                return new ExtendedBitvector();

            ExtendedBitvector bits = new ExtendedBitvector();
            bits.SetBit(bit);

            foreach (int val in args.Cast<int>().Where(val => val != -1))
            {
                bits.SetBit(val);
            }

            return bits;
        }
    }
}
