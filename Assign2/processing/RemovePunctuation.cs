using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Assign2_POC.processing
{
    /// <summary>
    /// Processing step to remove all of the punctuation.
    /// </summary>
    class RemovePunctuation : IProcessingStep
    {
        public event EventHandler ProcessingCompleted;
        public event EventHandler ProcessingStarted;
        public event RequestSuspend SuspendProcessing;

        //Following collections shall hold the Word Lists (In and Out)
        private BlockingCollection<string> _wordListIn;
        private BlockingCollection<string> _wordListOut;

        public RemovePunctuation(ref BlockingCollection<string> WordListIn, ref BlockingCollection<string> WordListOut)
        {
            _wordListIn = WordListIn;
            _wordListOut = WordListOut;

            //Use the processing completed event to report that the WordListOut is completly filled.
            this.ProcessingCompleted += delegate(object sender, EventArgs evt)
            {
                this._wordListOut.CompleteAdding();
            };
        }

        /// <summary>
        /// Removes the punctation from each item in the
        ///  word list. Processing Started and completed are 
        ///  implemented.
        /// </summary>
        public void executeStep()
        {
            if (ProcessingStarted != null)
                ProcessingStarted(this, new EventArgs());

            //Work - remove anything that is not letters. No spaces. Numbers don't help either. What if 
            // something is like "aslksfd#32sdsdf"? Not sure.
            while (!_wordListIn.IsCompleted)
            {
                string nextItem = string.Empty;
                bool hasNext = _wordListIn.TryTake(out nextItem);
                if (hasNext)
                {
                    nextItem = Regex.Match(nextItem, @"^([A-Za-z']+)\W?$").Groups[1].Value;
                    if (nextItem != null && nextItem.Length > 0)
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
