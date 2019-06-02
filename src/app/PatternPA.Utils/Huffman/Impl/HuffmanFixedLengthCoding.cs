using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PatternPA.Utils.Huffman.Interface;

namespace PatternPA.Utils.Huffman.Impl
{
    public class HuffmanFixedLengthCoding : IHuffmanFixedLengthCoding
    {
        private readonly List<BinaryNode> nodes;
        
        public BinaryNode Root { get; set; }
        public IDictionary<string, decimal> WordsProbability { get; private set; }
        public IDictionary<string, IEnumerable<bool>> WordsRepresentation { get; set; }
        public IEnumerable<string> SequenceInWords { get; set; }
        

        public HuffmanFixedLengthCoding()
        {
            nodes = new List<BinaryNode>();
            SequenceInWords = new List<string>();
            WordsRepresentation = new Dictionary<string, IEnumerable<bool>>();
        }

        public bool IsLeaf(BinaryNode binaryNode)
        {
            return (binaryNode.LeftChild == null && binaryNode.RightChild == null);
        }

        public BitArray Encode()
        {
            if (Root == null)
            {
                throw new InvalidOperationException("Root is null." +
                    " Check that a tree was built before encoding");
            }
            
            var encodedSource = new List<bool>();
            
            foreach (string word in SequenceInWords)
            {
                bool isCached = WordsRepresentation.ContainsKey(word);
                IEnumerable<bool> encodedSymbol;
                
                if (isCached) //return from cache
                {
                    encodedSymbol = WordsRepresentation[word];
                }
                else //otherwise find representation in cache
                {
                    encodedSymbol = Root.DepthTraverse(word, new List<bool>());
                    WordsRepresentation.Add(word, encodedSymbol);
                }
                
                //add to output
                encodedSource.AddRange(encodedSymbol);
            }

            var bits = new BitArray(encodedSource.ToArray());
            return bits;
        }

        public string Decode(BitArray encodedData)
        {
            BinaryNode current = Root;
            string decoded = "";

            //go through the encoded data
            foreach (bool bit in encodedData)
            {
                //select node
                if (bit)
                {
                    if (current.RightChild != null)
                    {
                        current = current.RightChild;
                    }
                }
                else
                {
                    if (current.LeftChild != null)
                    {
                        current = current.LeftChild;
                    }
                }

                //check if it is the last one
                if (IsLeaf(current))
                {
                    decoded += current.Word;
                    current = Root;
                }
            }

            return decoded;
        }

        public void Build(string source, int wordLength)
        {
            if (String.IsNullOrEmpty(source))
                throw new ArgumentException("Sequence is empty");
            if (wordLength < 0)
                throw new ArgumentException("Word lenght must be a positive number");

            //map
            IWordProbability wp = new WordProbability();
            SequenceInWords = wp.MapToWords(source, wordLength);

            //reduce
            WordsProbability = wp.ReduceToWordsProbability(SequenceInWords);

            //create nodes
            foreach (var word in WordsProbability)
            {
                var node = new BinaryNode(word.Key, word.Value);
                nodes.Add(node);
            }

            //construct a tree
            while (nodes.Count > 1) //while more than one node in a list
            {
                //sort available nodes ascending
                //lowest proability first
                var orderedNodes = nodes.OrderBy(node => node.Probability);

                var taken = orderedNodes.Take(2).ToList();

                // Create a parent BinaryNode and sum probabilities
                var parent = new BinaryNode
                {
                    Word = "Parent Binary Node",
                    Probability = taken.Sum(node => node.Probability),
                    LeftChild = taken[0],
                    RightChild = taken[1]
                };

                //this has been added so remove from the topmost list
                nodes.Remove(taken[0]);
                nodes.Remove(taken[1]);
                nodes.Add(parent);
            }

            Root = nodes.FirstOrDefault();
        }

        public IDictionary<string, decimal> CalculateCanonicalWordsProbability(string source, int wordLength)
        {
            if (String.IsNullOrEmpty(source))
                throw new ArgumentException("Sequence is empty");
            if (wordLength < 0)
                throw new ArgumentException("Word lenght must be a positive number");

            //map
            IWordProbability wp = new CanonicalWordProbability();
            var words = wp.MapToWords(source, wordLength);

            //reduce
            return wp.ReduceToWordsProbability(words);
        }
    }
}