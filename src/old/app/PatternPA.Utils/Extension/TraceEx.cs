using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Text;
using log4net;
using PatternPA.Core.Extensions;
using PatternPA.Utils.Huffman.Interface;

namespace PatternPA.Utils.Extension
{
    public static class TraceEx
    {
        private static ILog defaultLog = LogManager.GetLogger("UnitTestLogger");

        public static void TraceWords(this IHuffmanFixedLengthCoding c, ILog log = null)
        {
            if (log == null)
                log = defaultLog;

            log.Debug("Total numer of unique words in sequence: " + c.WordsProbability.Count);

            int i = 1;
            c.WordsProbability
                .OrderByDescending(p => p.Value)
                .ToList()
                .ForEach(pair =>
                    {
                        var encoded = c.WordsRepresentation[pair.Key];
                        string s = String.Format(
                            "{0}\t Word: '{1}'. Encoding '{2}'. Probability: '{3}'",
                            i, pair.Key, encoded.ToBitString(), pair.Value);
                        log.Debug(s);
                        i++;
                    });
        }

        public static string GetWordsAsSingleString(this IHuffmanFixedLengthCoding c)
        {
            var sb = new StringBuilder();
            c.WordsProbability
                .OrderByDescending(p => p.Value)
                .ToList()
                .ForEach(pair => sb.AppendLine(pair.Key + ", " + pair.Value));

            return sb.ToString();
        }

        public static void TraceEncoded(this BitArray data, ILog log = null)
        {
            if (log == null)
                log = defaultLog;

            log.Debug("Encoded sequence: " + data.ToBitString());
        }
    }
}