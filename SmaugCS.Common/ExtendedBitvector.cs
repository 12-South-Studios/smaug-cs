﻿using System.Linq;
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

        public int[] Bits { get; private set; }

        public ExtendedBitvector()
        {
            Bits = new int[XBI];
        }

        public ExtendedBitvector(ExtendedBitvector exBV)
        {
            SetBits(exBV);    
        }

        public bool IsEmpty()
        {
            return !Bits.Any(x => x != 0);
        }

        public int HasBits(ExtendedBitvector checkBits)
        {
            int bit;
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

        public string GetFlagString(string[] flagarray)
        {
            string flags = string.Empty;

            for (int x = 0; x < MAX_BITS; ++x)
            {
                if (IsSet(x))
                    flags += flagarray[x] + " ";
            }

            return flags;
        }

        public bool IsSet(int bit)
        {
            return (Bits[bit >> RSV] & 1 << (bit & XBM)) > 0;
        }

        public long SetBit(int bit)
        {
            return (Bits[bit >> RSV] |= 1 << (bit & XBM));
        }

        public long RemoveBit(int bit)
        {
            return (Bits[bit >> RSV] &= ~(1 << (bit & XBM)));
        }

        public long ToggleBit(int bit)
        {
            return (Bits[bit >> RSV] ^= 1 << (bit & XBM));
        }

        public void ClearBits()
        {
            for (int x = 0; x < XBI; x++)
                Bits[x] = 0;
        }
    }
}