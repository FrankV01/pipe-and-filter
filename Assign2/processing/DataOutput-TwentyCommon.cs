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
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assign2_POC.processing.util;

namespace Assign2_POC.processing
{
    /// <summary>
    /// Data Sink. Produces final output. 
    /// Reduces the list to the top 20 most common terms.
    /// </summary>
    class DataOutput_TwentyCommon : IProcessingStep
    {
        public event EventHandler ProcessingCompleted;
        public event EventHandler ProcessingStarted;
        public event RequestSuspend SuspendProcessing;

        /// <summary>
        /// List to analyze.
        /// </summary>
        private BlockingCollection<string> _wordListIn;

        
        public DataOutput_TwentyCommon(ref BlockingCollection<string> WordListIn)
        {
            this._wordListIn = WordListIn;
            //No data comes out here. Result returned via Event Handler.
        }

        /// <summary>
        /// Performs the work to reduce the list. Counts the 
        ///  terms and the sorts it and reduces it to twenty entries.
        ///  
        /// Processing Started and completed are implemented.
        /// </summary>
        public void executeStep()
        {
            if (ProcessingStarted != null)
                ProcessingStarted(this, new EventArgs());

            Dictionary<string, int> _info = new Dictionary<string, int>();
            //Gather the 20 most common terms and return the results via the event args.
            while (!_wordListIn.IsCompleted)
            {
                string nextItem = string.Empty;
                bool hasNext = _wordListIn.TryTake(out nextItem);
                if (hasNext)
                {
                    if (_info.ContainsKey(nextItem))
                    {
                        _info[nextItem]++;
                    }
                    else
                    {
                        _info[nextItem] = 1;
                    }
                }
            }

            //Next, par down to just top 20 (note, we could probably sperate this...)
            ArrayList _finalList = new ArrayList();
            foreach (var itm in _info)
            {
                _finalList.Add(new OrderedWord(itm.Value, itm.Key));
            }
            _finalList.Sort();
            
            if( _finalList.Count > 20 )
                _finalList.RemoveRange(20, _finalList.Count - 20);

            TwentyCommonEventArgs e = new TwentyCommonEventArgs();
            e.Terms = _finalList;

            if (ProcessingCompleted != null)
                ProcessingCompleted(this, e);
        }
    }
}
