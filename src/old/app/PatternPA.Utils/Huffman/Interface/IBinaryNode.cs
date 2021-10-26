using System.Collections.Generic;
using PatternPA.Utils.Huffman.Impl;

namespace PatternPA.Utils.Huffman.Interface
{
    /// <summary>
    /// Internal data structure for Huffman coding
    /// </summary>
    public interface IBinaryNode
    {
        /// <summary>
        /// Value of the word
        /// </summary>
        string Word { get; set; }

        /// <summary>
        /// Probability of occurance
        /// </summary>
        decimal Probability { get; set; }

        /// <summary>
        /// Reference to right child. Can be null.
        /// </summary>
        BinaryNode RightChild { get; set; }

        /// <summary>
        /// Reference to left child. Can be null.
        /// </summary>
        BinaryNode LeftChild { get; set; }

        /// <summary>
        /// Recursion depth-first graph traversal to find bit representation of a word
        /// </summary>
        /// <param name="word">string for which to find bit representation</param>
        /// <param name="recursionData">temporary recursion data, 
        /// initial data structure must be empty enumerable, with every recursion call, 
        /// the size will grow by one bit - 0 or 1</param>
        /// <returns>binary sequence - representation of a string</returns>
        IEnumerable<bool> DepthTraverse(string word, IEnumerable<bool> recursionData);
    }
}