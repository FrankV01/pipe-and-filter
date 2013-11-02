/**
 * A pedagogical Pipe-And-Filter architectural implementation
 * Copyright (C) 2012-2013 Frank Villasenor <frank.villasenor [at] gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program (COPYING.LESSER.txt). 
 * If not, see <http://www.gnu.org/licenses/>.
 * 
 */

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
