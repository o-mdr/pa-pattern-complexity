using System;
using System.IO;
using System.Text;

namespace PatternPA.Utils.Entropy.MSE
{
    /// <summary>
    /// Constructs arguments for mse.exe
    /// <see cref="http://physionet.org/physiotools/mse/tutorial/node3.html"/>
    /// </summary>
    public class MseArgumentBuilder
    {
        private readonly StringBuilder _sb;

        public MseArgumentBuilder()
        {
            _sb = new StringBuilder();
        }

        public string Build()
        {
            return _sb.ToString().Trim();
        }

        /// <summary>
        /// Set minimum pattern lenght parameter m.
        /// Default is 2.
        /// </summary>
        /// <param name="m">min lenght of the pattern (inclusive)</param>
        /// <returns>builder object</returns>
        public MseArgumentBuilder WithMinPatternLenght(int m)
        {
            if (m < 0)
                throw new ArgumentOutOfRangeException();
            
            _sb.Append(" -m " + m);
            return this;
        }

        /// <summary>
        /// Set a step for calculations of several pattern lenghtes.
        /// Default is 1.
        /// </summary>
        /// <param name="b">step value</param>
        /// <returns>builder object</returns>
        public MseArgumentBuilder HavingPatternLenghtStepOf(int b)
        {
            if (b < 0)
                throw new ArgumentOutOfRangeException();

            _sb.Append(" -b " + b);
            return this;
        }

        /// <summary>
        /// Set max pattern lenght parameter M.
        /// Default is 2.
        /// </summary>
        /// <param name="M">max lenght of the pattern (inclusive)</param>
        /// <returns>builder object</returns>
        public MseArgumentBuilder WithMaxPatternLenght(int M)
        {
            if (M < 0)
                throw new ArgumentOutOfRangeException();

            _sb.Append(" -M " + M);
            return this;
        }

        /// <summary>
        /// Set minimum pattern similarity criterion r.
        /// Default is 0.15.
        /// </summary>
        /// <param name="r">min similarity criterion (inclusive)</param>
        /// <returns>builder object</returns>
        public MseArgumentBuilder WithMinSimilarityCriterion(double r)
        {
            if (r < 0.0)
                throw new ArgumentOutOfRangeException();

            _sb.Append(" -r " + r);
            return this;
        }

        /// <summary>
        /// Set a step for calculations of several similarity criteria
        /// Default is 0.05.
        /// </summary>
        /// <param name="c">step value</param>
        /// <returns>builder object</returns>
        public MseArgumentBuilder HavingSimilarityCriterionStepOf(double c)
        {
            if (c < 0)
                throw new ArgumentOutOfRangeException();

            _sb.Append(" -c " + c);
            return this;
        }

        /// <summary>
        /// Set maximum pattern similarity criterion R
        /// Default is 0.15.
        /// </summary>
        /// <param name="R">Max similarity criterion (inclusive)</param>
        /// <returns>builder object</returns>
        public MseArgumentBuilder WithMaxSimilarityCriterion(double R)
        {
            if (R < 0.0)
                throw new ArgumentOutOfRangeException();

            _sb.Append(" -R " + R);
            return this;
        }

        /// <summary>
        /// Set minimum start index of the line from which to read data
        /// Default is 0.
        /// </summary>
        /// <param name="i">min index of data line in a file (inclusive)</param>
        /// <returns>builder object</returns>
        public MseArgumentBuilder WithMinStartIndex(int i)
        {
            if (i < 0)
                throw new ArgumentOutOfRangeException();

            _sb.Append(" -i " + i);
            return this;
        }

        /// <summary>
        /// Set maximum stop index of the line from which to read data.
        /// Default is 39999.
        ///  </summary>
        /// <param name="I">max index of data line in a file (inclusive)</param>
        /// <returns>builder object</returns>
        public MseArgumentBuilder WithMaxStopIndex(int I)
        {
            if (I < 0)
                throw new ArgumentOutOfRangeException();

            _sb.Append(" -I " + I);
            return this;
        }

        /// <summary>
        ///  Set maximum scale value. Each scale defines the length of the window 
        /// used for building the coarse-grained time series. Default is 20.
        ///  </summary>
        /// <param name="n">max scale  window size(inclusive)</param>
        /// <returns>builder object</returns>
        public MseArgumentBuilder WithMaxScaleFactor(int n)
        {
            if (n < 0)
                throw new ArgumentOutOfRangeException();

            _sb.Append(" -n " + n);
            return this;
        }

        /// <summary>
        /// Set a step for calculations of several scale  window sizes
        /// Default is 1.
        /// </summary>
        /// <param name="a">step value</param>
        /// <returns>builder object</returns>
        public MseArgumentBuilder HavingScaleWindowStepOf(int a)
        {
            if (a < 0)
                throw new ArgumentOutOfRangeException();

            _sb.Append(" -a " + a);
            return this;
        }

        /// <summary>
        /// Allows to read many files defined in a single file
        /// </summary>
        /// <param name="F">Path to a text file, in which 
        /// each line lists the name of a data file </param>
        /// <returns>builder object</returns>
        public MseArgumentBuilder ReadManyFilesFrom(string F)
        {
            if (String.IsNullOrEmpty(F))
                throw new ArgumentNullException();
            if (!File.Exists(F))
                throw new FileNotFoundException();

            _sb.Append(" -F " + F);

            return this;
        }
    }
}
