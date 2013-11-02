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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Assign2_POC.processing
{
    /// <summary>
    /// The processing step to remove the stop words from the list.
    /// </summary>
    class RemoveStopWords : IProcessingStep
    {
        public event EventHandler ProcessingCompleted;
        public event EventHandler ProcessingStarted;
        public event RequestSuspend SuspendProcessing;

        //Following collections shall hold the Word Lists (Stop, In and Out)
        private BlockingCollection<string> _stopWordList;
        private BlockingCollection<string> _wordListIn;
        private BlockingCollection<string> _wordListOut;

        public RemoveStopWords(BlockingCollection<string> StopWordList, ref BlockingCollection<string> WordListIn, ref BlockingCollection<string> WordListOut)
        {
            this._stopWordList = StopWordList;
            this._wordListIn = WordListIn;
            this._wordListOut = WordListOut;

            //Use the processing completed event to report that the WordListOut is completly filled.
            this.ProcessingCompleted += delegate(object sender, EventArgs evt) {
                this._wordListOut.CompleteAdding();
            };
        }

        /// <summary>
        /// Once the stop word list is 100% completes (thread is blocked until then) this 
        /// will go through the wordlist and remove the stop word. They are removed by not
        /// being added to the 'out' list.
        /// 
        /// Design: Consider removing the Suspending of the event since we have a blocking queue?
        /// </summary>
        public void executeStep()
        {
            if (ProcessingStarted != null)
                ProcessingStarted(this, new EventArgs());

            //First this MUST wait for _stopWordList to be complete. No choice there.

            if (_stopWordList.IsAddingCompleted)
            {
                while (!_wordListIn.IsCompleted)
                {
                    string nextItem = _wordListIn.Take();
                    if (!_stopWordList.Contains(nextItem))
                    {
                        _wordListOut.Add(nextItem);
                    }
                }
                
                if (ProcessingCompleted != null)
                    ProcessingCompleted(this, new EventArgs());
            }
            else
            {
                if (SuspendProcessing != null)
                {
                    SuspendRequestEventArgs arg = new SuspendRequestEventArgs();
                    arg.SuspendTimeMillaSeconds = 3600 / 16; //4 seconds
                    SuspendProcessing(this, arg);
                }
                else
                {
                    throw new Exception("Need to suspend but there is no method of restarting the thread");
                }
            }
        }   
    }
}
