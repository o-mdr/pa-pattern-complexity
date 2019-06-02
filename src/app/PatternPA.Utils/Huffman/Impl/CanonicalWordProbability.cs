using System;
using System.Collections.Generic;
using System.Linq;
using PatternPA.Core.Extensions;
using PatternPA.Utils.Huffman.Interface;

namespace PatternPA.Utils.Huffman.Impl
{
    public class CanonicalWordProbability : IWordProbability
    {
        public IEnumerable<string> MapToWords(string sequence, int wordSize)
        {
            IEnumerable<string> words = sequence.SplitBy(wordSize);

            return words.Select(word => 
                String.Concat(
                    word
                    .ToArray()
                    .OrderBy(c => c)));
        }

        public IDictionary<string, decimal> ReduceToWordsProbability(IEnumerable<string> words)
        {
            long totalWordCount = words.Count();

            //get distinct words and count
            var wordProbability = (from word in words
                              group word by word
                                  into distinctWord // select distinct
                                  let count = distinctWord.Count() // remember count of each distinct
                                  select new
                                  {
                                      Word = distinctWord.Key,
                                      Probability = (decimal)count / totalWordCount
                                  })
                            .ToDictionary(row => row.Word, row => row.Probability);
            
            decimal probabilitySum = wordProbability.Sum(pair => pair.Value);
            int precision = 10;
            decimal rounded = Math.Round(probabilitySum, 10);

            if (rounded != 1)
                throw new InvalidOperationException("Rounded sum of calculated probabilities is not 1" +
                                "Rounded with precision of: " + precision +
                                "Received value was: " + rounded);

            return wordProbability;
        }
    }
}