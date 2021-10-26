using System.Threading;
using System.Threading.Tasks;

namespace PatternPA.Utils.Correlation.Auto
{
    public class Autocorrelation<T> 
    {
        private readonly T[] _sequence; //data

        /// <summary>
        /// Creates new autocorrelation
        /// </summary>
        /// <param name="sequence">data for which autocorrelation will be computed</param>
        public Autocorrelation(T[] sequence)
        {
            _sequence = sequence;
        }

        /// <summary>
        /// Computes autocorrelation based on paper. Array is not copied but 
        /// rather a circular index is created and the pointers to data are rotated
        /// 
        /// Sample rotation explained by array indeces
        /// data idx: 0 1 2 3 4 5
        /// shift 1:  1 2 3 4 5 0
        /// shift 2:  2 3 4 5 0 1
        /// shift 3:  3 4 5 0 1 2
        /// etc.
        /// 
        /// 
        /// Fourier analysis of symbolic data: A brief review.
        /// Vera Afreixo, Paulo J.S.G. Ferreira *, Dorabella Santos
        /// Departamento Electrónica e Telecomunicações/IEETA, 
        /// Universidade de Aveiro, 3810-193 Aveiro, Portugal
        /// Available online 11 September 2004
        /// </summary>
        /// <param name="shiftBy">Index to shift data in the array </param>
        /// <returns></returns>
        public double Compute(int shiftBy)
        {
            int lenght = _sequence.Length;
            int total = 0;

            //Parallel calculations
            Parallel.For(
                0/*from*/,
                lenght /*to*/,
                () => 0 /*init thread-local variable to 0*/,
                /*action*/
                (i /*index*/, opt /*options*/, subtotal /*thread local var*/) =>
                {
                    //circular index
                    int circularIdx = ((i + shiftBy) % lenght);

                    //if comparison if the value is equal
                    //to the shifted item value
                    //to speed it up there is only one array of data
                    //in which we update only indeces pointers
                    if (_sequence[i].Equals(_sequence[circularIdx]))
                    {
                        subtotal++;
                    }

                    return subtotal;
                },

                /*add to total result*/
                result => Interlocked.Add(ref total, result)
            );

            double normalizedAutoCorrelation = (double)total / _sequence.Length;

            //Sequential execution
            //int sum = 0;
            //int circularIdx = shiftBy;
            //for (int i = lenght - 1; i >= 0; i--)
            //{
            //    circularIdx = ((circularIdx + (lenght - 1)) % lenght);
            //    if (_sequence[i].Equals(_sequence[circularIdx]))
            //    {
            //        sum++;
            //    }
            //}
            //double normalizedAutoCorrelation = (double)sum / _sequence.Length;

            return normalizedAutoCorrelation;
        }
    }
}
