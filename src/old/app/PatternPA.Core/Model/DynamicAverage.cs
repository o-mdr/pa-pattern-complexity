using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PatternPA.Core.Model
{
    public class BlockingDoubleAverage
    {
        private double sum;
        private int count;
        private readonly object syncRoot = new object();

        public void Add(double value)
        {
            lock (syncRoot)
            {
                sum += value;
                count++;
            }
        }

        public double GetCurrentAverage()
        {
            lock (syncRoot)
            {
                //escape devide by 0 exception
                if (count == 0)
                    return 0;
                return sum / count;
            }
        }
    }

    // DynamicDecimalAverage
    // OnTheFlyDecimalAverage
    public class NonBufferingAverage
    {
        private decimal sum;
        private long count;

        public void Add(bool bit)
        {
            if (bit)
            {
                sum++;
            }
            count++;
        }

        public decimal GetCurrentAverage()
        {
            //escape devide by 0 exception
            if (count == 0)
                return 0;
            return sum / count;
        }
    }

    public class BufferingAverage
    {
        private List<bool> data;

        public BufferingAverage(int size)
        {
            data = new List<bool>(size);
        }

        public void Add(bool bit)   
        {
            data.Add(bit);
        }

        public decimal GetCurrentAverage()
        {
            int countOfTrue = data.AsParallel()
                            .Where(bit => bit == true)
                            .Count();
            return (decimal) countOfTrue / data.Count;
        }
    }
}