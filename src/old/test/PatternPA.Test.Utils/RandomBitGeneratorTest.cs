using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using PatternPA.Core.Extensions;
using PatternPA.Core.Model;
using PatternPA.Test.Utils._7zip;

namespace PatternPA.Test.Utils
{
    [TestFixture]
    [Ignore("Manual test, compute this only once to know the normalization boundaries")]
    public class RandomBitGeneratorTest : AbstractUtilsTest
    {
        private Random r = new Random();
        private string fName;

        [Test]
        public void Generate()
        {
            int length = 1024;
            BitArray bitArray = rndBitGenerator.Generate(length);
            fName = String.Concat(r.Next(), "tmp.bin");
            binaryFileWriter.Write("random.bin", FileMode.OpenOrCreate, bitArray.ToByteArray());
        }

        [Test]
        public void GetCopressionRatioOfFileFilledWithRandomBitsOfAround_200K_GZIP_Test()
        {
            int length = 8*1024*200; //generate around 200k random bits
            int max = 1000;
            var dynamicAverage = new BlockingDoubleAverage();
            int count = 0;
            Parallel.For(0, max ,i =>
            {
                Interlocked.Increment(ref count);
                string fName = String.Concat(r.Next(), "tmp.bin");
                BitArray bitArray = rndBitGenerator.Generate(length);
                binaryFileWriter.Write(fName, FileMode.OpenOrCreate, bitArray.ToByteArray());
                double value = GzipStreamHelper.Compress(fName);
                dynamicAverage.Add(value);
                if(count % 100 == 0)
                    Trace.WriteLine("Processed: " + count);
                File.Delete(fName);
            });

            double result = dynamicAverage.GetCurrentAverage();
            Trace.WriteLine("Compressed: " + max + "files");
            Trace.WriteLine("Lenght of binary data: " + length);
            Trace.WriteLine("Average compression: " + result);
        }

        [Test]
        public void GetCopressionRatioOfFileFilledWithRandomBitsOfAround_200K_LZMA_Test()
        {
           int length = 8 * 1024 * 200; //generate around 200k random bits
            int max = 1000;
            var dynamicAverage = new BlockingDoubleAverage();
            int count = 0;
            Parallel.For(0, max, i =>
            {
                Interlocked.Increment(ref count);
                string fName = String.Concat(r.Next(), "tmp.bin");
                BitArray bitArray = rndBitGenerator.Generate(length);
                binaryFileWriter.Write(fName, FileMode.OpenOrCreate, bitArray.ToByteArray());
                double value = GzipStreamHelper.Compress(fName);
                dynamicAverage.Add(value);
                if (count % 100 == 0)
                    Trace.WriteLine("Processed: " + count);
                File.Delete(fName);
            });

            double result = dynamicAverage.GetCurrentAverage();
            Trace.WriteLine("Compressed: " + max + "files");
            Trace.WriteLine("Lenght of binary data: " + length);
            Trace.WriteLine("Average compression: " + result);
        }

        [Test]
        public void GetCopressionRatioOfFileFilledWith_0_OfAround_200K_GZIP_Test()
        {
            int length = 8 * 1024 * 200; //around 200k bits
            int max = 1000;
            var dynamicAverage = new BlockingDoubleAverage();
            int count = 0;
            Parallel.For(0, max, i =>
            {
                Interlocked.Increment(ref count);
                string fName = String.Concat(r.Next(), "tmp.bin");
                var bitArray = new BitArray(length, false);
                binaryFileWriter.Write(fName, FileMode.OpenOrCreate, bitArray.ToByteArray());
                double value = GzipStreamHelper.Compress(fName);
                dynamicAverage.Add(value);
                if (count % 100 == 0)
                    Trace.WriteLine("Processed: " + count);
                File.Delete(fName);
            });

            double result = dynamicAverage.GetCurrentAverage();
            Trace.WriteLine("Compressed: " + max + "files");
            Trace.WriteLine("Lenght of binary data: " + length);
            Trace.WriteLine("Average compression: " + result);
        }

       //[Test]
        //public void Program()
        //{
        //    //generate random data
        //    int size = 10000000;
        //    byte[] randomData = new byte[size];
        //    var nonBufferingAverage1 = new NonBufferingAverage();
        //    var randomProvider = new RNGCryptoServiceProvider();
        //    randomProvider.GetNonZeroBytes(randomData);
        //    var bitArray1 = new BitArray(randomData);
        //    foreach (bool bit in bitArray1)
        //    {
        //        nonBufferingAverage1.Add(bit);
        //    }
        //    decimal nonBuffering1 = nonBufferingAverage1.GetCurrentAverage();

        //    var nonBufferingAverage2 = new NonBufferingAverage();
        //    for (int i = 0; i < randomData.Length; i++)
        //    {
        //        bool bit = randomData[i] <= 127;
        //        nonBufferingAverage2.Add(bit);
        //    }
        //    decimal nonBuffering2 = nonBufferingAverage2.GetCurrentAverage();

        //    Thread.Sleep(1000);

        //    var bufferingAverage = new BufferingAverage(size);
        //    foreach (bool bit in bitArray1)
        //    {
        //        bufferingAverage.Add(bit);
        //    }

            
        //    decimal buffering = bufferingAverage.GetCurrentAverage();

        //    Trace.WriteLine("Total number of bits: " + bitArray1.Count);
        //    Trace.WriteLine("Non-buffering probability: " + nonBuffering1);
        //    Trace.WriteLine("Buffering probability: " + buffering);
        //    Trace.WriteLine("Both provide same result: " + (nonBuffering1 == buffering));
        //} 
    }
}