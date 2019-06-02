using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using PatternPA.Core.Interfaces.Convertor;
using PatternPA.Core.Model;

namespace PatternPA.Utils
{
    public class BinaryConverter : IBinaryConverter
    {
        public byte[] ToBinArray(IEnumerable<int>  data)
        {
            var sb = new StringBuilder();

            foreach (int dataItem in data)
            {
                sb.Append(Convert.ToString(dataItem, 2));
            }

            var binArray = new BitArray(sb.Length);

            for (int i = 0; i < sb.Length; i++)
            {
                bool value = sb[i] == '0' ? false : true;
                binArray.Set(i, value);
            }

            var byteSize = (int) Math.Ceiling((double) binArray.Length/8);
            var bytes = new byte[byteSize];
            binArray.CopyTo(bytes, 0);

            return bytes;
        }

        public byte[] ToBinArray(IEnumerable<Record> data)
        {
            var sb = new StringBuilder();

            foreach (Record dataItem in data)
            {
                sb.Append(Convert.ToString((int) dataItem.ActivityCode, 2));
            }

            var binArray = new BitArray(sb.Length);

            for (int i = 0; i < sb.Length; i++)
            {
                bool value = sb[i] == '0' ? false : true;
                binArray.Set(i, value);
            }

            var byteSize = (int) Math.Ceiling((double) binArray.Length/8);
            var bytes = new byte[byteSize];
            binArray.CopyTo(bytes, 0);

            return bytes;
        }
    }
}