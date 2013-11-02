using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assign2_POC.processing
{
    /// <summary>
    /// Removes duplicate words from the list.
    /// </summary>
    class DeDup : IProcessingStep
    {
        public event EventHandler ProcessingCompleted;
        public event EventHandler ProcessingStarted;
        public event RequestSuspend SuspendProcessing;

        //Following collections shall hold the Word Lists (In and Out)
        private BlockingCollection<string> _wordListIn;
        private BlockingCollection<string> _wordListOut;

        public DeDup(ref BlockingCollection<string> WordListIn, ref BlockingCollection<string> WordListOut)
        {
            _wordListIn = WordListIn;
            _wordListOut = WordListOut;

            //Use the processing completed event to report that the list is completly filled.
            this.ProcessingCompleted += delegate(object sender, EventArgs evt)
            {
                this._wordListOut.CompleteAdding();
            };
        }

        /// <summary>
        /// Remove dupliate words from the word list.
        /// 
        /// Note: this isn't used in the final product. 
        /// </summary>
        public void executeStep()
        {
            if (ProcessingStarted != null)
                ProcessingStarted(this, new EventArgs());

            //Remove duplicate words... 
            while (!_wordListIn.IsCompleted)
            {
                string nextItem = string.Empty;
                bool hasNext = _wordListIn.TryTake(out nextItem);
                if (hasNext)
                {
                    //Interal work may be more complex than just this.... 
                    bool NotInOutQueue = (!_wordListOut.Contains(nextItem));
                    if (NotInOutQueue)
                    {
                        _wordListOut.Add(nextItem);
                    }
                }
            }

            if (ProcessingCompleted != null)
                ProcessingCompleted(this, new EventArgs());
        }
    }
}
