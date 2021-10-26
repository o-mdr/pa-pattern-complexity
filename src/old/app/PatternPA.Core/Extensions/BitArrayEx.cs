using System;
using System.Collections;
using System.Text;

namespace PatternPA.Core.Extensions
{
    public static class BitArrayEx
    {
        public static string ToBitString(this BitArray bits)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < bits.Count; i++)
            {
                char c = bits[i] ? '1' : '0';
                sb.Append(c);
            }

            return sb.ToString();
        }

        public static byte[] ToByteArray(this BitArray bits)
        {
            var byteSize = (int)Math.Ceiling((double)bits.Length / 8);
            byte[] bytes = new byte[byteSize];
            bits.CopyTo(bytes, 0);

            return bytes;
        }
    }
}