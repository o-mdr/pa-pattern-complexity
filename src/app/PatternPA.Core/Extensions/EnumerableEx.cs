using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PatternPA.Core.Extensions
{
    public static class EnumerableEx
    {
        public static IEnumerable<string> SplitBy(this string str, int chunkLenght)
        {
            if (String.IsNullOrEmpty(str))
                throw new ArgumentException();
            if (chunkLenght < 1)
                throw new ArgumentException();
            
            int stringLength = str.Length;
            for (int i = 0; i < stringLength; i += chunkLenght)
            {
                if (i + chunkLenght > stringLength)
                {
                    chunkLenght = stringLength - i;
                }

                yield return str.Substring(i, chunkLenght);
            }
        }

        public static string AllToString<T>(this IEnumerable<T> data)
        {
            var sb = new StringBuilder();
            var enumerator = data.GetEnumerator();

            while (enumerator.MoveNext())
            {
                sb.Append(enumerator.Current);
            }

            return sb.ToString();
        }

        public static string ToBitString(this IEnumerable<bool> data)
        {
            var sb = new StringBuilder();
            var enumerator = data.GetEnumerator();

            while (enumerator.MoveNext())
            {
                char c = enumerator.Current ? '1' : '0';
                sb.Append(c);
            }

            return sb.ToString();
        }
    }
}
