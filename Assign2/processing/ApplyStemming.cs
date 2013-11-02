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

using Assign2_POC.processing.porter;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assign2_POC.processing
{
    /// <summary>
    /// Processing step to apply the stemming algorthum.
    /// </summary>
    class ApplyStemming : IProcessingStep
    {
        public event EventHandler ProcessingCompleted;
        public event EventHandler ProcessingStarted;
        public event RequestSuspend SuspendProcessing;

        //Following collections shall hold the Word Lists (In and Out)
        private BlockingCollection<string> _wordListIn;
        private BlockingCollection<string> _wordListOut;

        public ApplyStemming(ref BlockingCollection<string> WordListIn, ref BlockingCollection<string> WordListOut)
        {
            this._wordListIn = WordListIn;
            this._wordListOut = WordListOut;

            //Use the processing completed event to report that the WordListOut is completly filled.
            this.ProcessingCompleted += delegate(object sender, EventArgs evt) {
                this._wordListOut.CompleteAdding();
            };
        }

        /// <summary>
        /// Applies the stemming algorthium word-by-word as the pipeline is filled.
        /// Processing Started and completed are implemented.
        /// </summary>
        public void executeStep()
        {
            if (ProcessingStarted != null)
                ProcessingStarted(this, new EventArgs());

            //Find and apply algorthum.
            while (!_wordListIn.IsCompleted)
            {
                string nextItem = string.Empty;
                bool hasNext = _wordListIn.TryTake(out nextItem);
                if (hasNext)
                {
                    Stemmer st = new Stemmer();

                    char[] itmAsChar = nextItem.ToCharArray();
                    st.add(itmAsChar, itmAsChar.Length);
                    st.stem();
                    nextItem = st.ToString();

                    //Stemming seems correct, despite words comming out funny. Online tester:http://text-processing.com/demo/stem/

                    _wordListOut.Add(nextItem);
                }
            }

            if (ProcessingCompleted != null)
                ProcessingCompleted(this, new EventArgs());
        }
    }
}
