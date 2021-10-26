using System.Collections;
using System.Collections.Generic;
using PatternPA.Utils.Huffman.Impl;

namespace PatternPA.Utils.Huffman.Interface
{
    /// <summary>
    /// Main interface to encode and decode sequence 
    /// using adapted Huffman codeing. The main difference from Huffman coding is that
    /// the lenght of the input word is strictly fixed
    /// </summary>
    public interface IHuffmanFixedLengthCoding
    {

        /// <summary>
        /// Convert input sequence into bit array with the fixed input word lenght
        /// </summary>
        BitArray Encode();

        /// <summary>
        /// Convert bit array to the initial sequence. 
        /// This method uses pre-built graph of binary nodes
        /// </summary>
        string Decode(BitArray encodedData);

        /// <summary>
        /// Create a graph of words, each node contains words, probability of 
        /// occurrance, references to left and right sub-node 
        /// </summary>
        void Build(string source, int wordLength);

        /// <summary>
        /// Current parent node that contains all words of the sequence
        /// </summary>
        BinaryNode Root { get; set; }

        /// <summary>
        /// Set of words and probabilities of occurrance
        /// </summary>
        IDictionary<string, decimal> WordsProbability { get; }

        /// <summary>
        /// Set of words and huffman encoded representation
        /// This value is available only after encoding has finished
        /// </summary>
        IDictionary<string, IEnumerable<bool>> WordsRepresentation { get; }

        IEnumerable<string> SequenceInWords { get; set; }
        IDictionary<string, decimal> CalculateCanonicalWordsProbability(string source, int wordLength);
    }
}