using System;
using System.IO;
using PatternPA.Core.Interfaces;
using SevenZip;
using SevenZip.Compression.LZMA;

namespace PatternPA.Utils._7zip
{
    public class LZMAArchiver : IArchiver
    {
        private const string CompressedExtension = ".LZMA";

        #region private members
        private static CoderPropID[] propIDs = 
				{
					CoderPropID.DictionarySize,
					CoderPropID.PosStateBits,
					CoderPropID.LitContextBits,
					CoderPropID.LitPosBits,
					CoderPropID.Algorithm,
					CoderPropID.NumFastBytes,
					CoderPropID.MatchFinder,
					CoderPropID.EndMarker
				};

        // these are the default properties
        private static object[] properties = 
				{
					(Int32)1 << 23,
					(Int32)(2),
					(Int32)(3),
					(Int32)(0),
					(Int32)(2),
					(Int32)(128),
					"bt4",
					false
				};
        #endregion

        public string Compress(FileInfo fi)
        {
            string encodedPath = fi.FullName + CompressedExtension;
            byte[] rawBytes = GetByteFromFile(fi);
            byte[] compressedBytes = InternalCompress(rawBytes);

            using (Stream encodedStream = File.Create(encodedPath))
            {
                encodedStream.Write(compressedBytes, 0, compressedBytes.Length);
            }

            return encodedPath;
        }

        public string Decompress(FileInfo fi)
        {
            string decodedPath = fi.FullName.Remove(fi.FullName.Length - fi.Extension.Length);
            byte[] rawBytes = GetByteFromFile(fi);
            byte[] decompressedBytes = InternalDecompress(rawBytes);

            using (Stream encodedStream = File.Create(decodedPath))
            {
                encodedStream.Write(decompressedBytes, 0, decompressedBytes.Length);
            }

            return decodedPath;
        }

        public double GetMaxCompressionRatio(string fPath)
        {
            if (!File.Exists(fPath))
            {
                throw new FileNotFoundException();
            }

            //Compress
            byte[] input = File.ReadAllBytes(fPath);
            byte[] output = InternalCompress(input);

            return (double)input.Length / output.Length;
        }

        private byte[] GetByteFromFile(FileInfo fi)
        {
            using (Stream stream = fi.OpenRead())
            {
                int size = (int)stream.Length;
                byte[] rawBytes = new byte[size];
                stream.Read(rawBytes, 0, size);

                return rawBytes;
            }
        }

        private byte[] InternalCompress(byte[] inputBytes)
        {
            var inStream = new MemoryStream(inputBytes);
            var outStream = new MemoryStream();
            var encoder = new Encoder();
            encoder.SetCoderProperties(propIDs, properties);
            encoder.WriteCoderProperties(outStream);

            long fileSize = inStream.Length;
            for (int i = 0; i < 8; i++)
            {
                outStream.WriteByte((Byte)(fileSize >> (8 * i)));
            }

            encoder.Code(inStream, outStream, -1, -1, null);
            return outStream.ToArray();
        }

        private byte[] InternalDecompress(byte[] inputBytes)
        {
            var newInStream = new MemoryStream(inputBytes);

            var decoder = new Decoder();

            newInStream.Seek(0, 0);
            var newOutStream = new MemoryStream();

            byte[] properties2 = new byte[5];
            if (newInStream.Read(properties2, 0, 5) != 5)
            {
                throw (new Exception("input .lzma is too short"));
            }

            long outSize = 0;
            for (int i = 0; i < 8; i++)
            {
                int v = newInStream.ReadByte();
                if (v < 0)
                {
                    throw (new Exception("Can't Read 1"));
                }

                outSize |= ((long)(byte)v) << (8 * i);
            }

            decoder.SetDecoderProperties(properties2);

            long compressedSize = newInStream.Length - newInStream.Position;
            decoder.Code(newInStream, newOutStream, compressedSize, outSize, null);

            byte[] b = newOutStream.ToArray();

            return b;
        }
    }
}
