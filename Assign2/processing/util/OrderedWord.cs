using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assign2_POC.processing.util
{
    /// <summary>
    /// Data structure to help order the words. Implements 
    /// helpful .NET interfaces to leverage built-in .NET
    /// algorithms.
    /// </summary>
    class OrderedWord : IComparable<OrderedWord>, IComparable
    {
        public OrderedWord(int numOccurances, string Word)
        {
            this.NumOccurances = numOccurances;
            this.stemmedWord = Word;
        }

        /// <summary>
        /// Number of occurances the word appeared
        /// </summary>
        public int NumOccurances { get; set; }

        /// <summary>
        /// The word itself.
        /// </summary>
        public string stemmedWord { get; set; }

        /// <summary>
        /// A basic implementation of the CompareTo method
        ///  to support the IComparable interfaces
        /// </summary>
        /// <param name="other">Object to compare to</param>
        /// <returns>see MSDN</returns>
        public int CompareTo(OrderedWord other)
        {
            if (this.NumOccurances == other.NumOccurances)
            {
                //If equal occurances, alpha sort.
                return this.stemmedWord.CompareTo(other.stemmedWord) ;
            }
            else if (this.NumOccurances < other.NumOccurances)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// An implementation of the CompareTo method to support the IComparable 
        /// interface
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>see MSDN</returns>
        public int CompareTo(object obj)
        {
            if (obj is OrderedWord)
            {
                OrderedWord _orderedWord = (OrderedWord)obj;
                return this.CompareTo(_orderedWord);
            }
            else
            {
                throw new InvalidOperationException("Invalid type to compare to");
            }
        }
    }
}
