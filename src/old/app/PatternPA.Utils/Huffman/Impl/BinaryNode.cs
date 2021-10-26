using System.Collections.Generic;
using PatternPA.Utils.Huffman.Interface;

namespace PatternPA.Utils.Huffman.Impl
{
    public class BinaryNode : IBinaryNode
    {
        public string Word { get; set; }
        public decimal Probability { get; set; }
        public BinaryNode RightChild { get; set; }
        public BinaryNode LeftChild { get; set; }

        public BinaryNode() { }

        public BinaryNode(string word, decimal probability)
        {
            Word = word;
            Probability = probability;
        }

        public IEnumerable<bool> DepthTraverse(string word, IEnumerable<bool> recursionData)
        {
            // no children found
            if (RightChild == null && LeftChild == null)
            {
                //correct guess BinaryNode has the needed char
                if (word.Equals(Word))
                {
                    return recursionData;
                }

                //wrong char - try another leg
                return null;
            }

            //there are children
            IEnumerable<bool> left = null; //tmp left storage
            IEnumerable<bool> right = null; //tmp right storage

            //start with the left child
            //and travers in depth by left leg
            //encode as 0
            if (LeftChild != null)
            {
                //go in depth through the left child 
                var leftPath = new List<bool>();
                //add previously gathered recursionData
                leftPath.AddRange(recursionData);
                //add current value of 0
                leftPath.Add(false);
                //recursion call by rigth leg
                left = LeftChild.DepthTraverse(word, leftPath);
            }

            //no left children found
            //travers by right leg in depth now
            //encode as 1
            if (RightChild != null)
            {
                //go in depth through the right child 
                var rightPath = new List<bool>();
                //add previously gathered recursionData
                rightPath.AddRange(recursionData);
                //add current value of 1
                rightPath.Add(true);
                //recursion call by rigth leg
                right = RightChild.DepthTraverse(word, rightPath);
            }

            //return collected value of left or right
            if (left != null)
            {
                return left;
            }

            return right;
        }
    }
}