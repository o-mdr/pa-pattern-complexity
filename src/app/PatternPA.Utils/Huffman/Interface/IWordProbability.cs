using System.Collections.Generic;

namespace PatternPA.Utils.Huffman.Interface
{
    public interface IWordProbability
    {
        /// <summary>
        /// Maps sequence into words of defined lenght
        /// </summary>
        /// <param name="sequence">input string of characters</param>
        /// <param name="wordSize">lenght of the word</param>
        /// <returns>sequence of words in exatly the same order as in the input string</returns>
        IEnumerable<string>  MapToWords(string sequence, int wordSize);

        /// <summary>
        /// Removes duplicate words and calculates probability of occurance of each word
        /// </summary>
        /// <param name="words">sequence of words</param>
        /// <returns>unsorted dictionary of pairs: words=probability</returns>
        IDictionary<string, decimal> ReduceToWordsProbability(IEnumerable<string> words);
    }
}