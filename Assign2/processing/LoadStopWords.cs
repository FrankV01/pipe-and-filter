using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Collections.Concurrent;

namespace Assign2_POC.processing
{
    /// <summary>
    /// Loads the stop words from a flat text file.
    /// </summary>
    class LoadStopWords : IProcessingStep
    {
        public event EventHandler ProcessingCompleted;
        public event EventHandler ProcessingStarted;
        public event RequestSuspend SuspendProcessing;

        /// <summary>
        /// Path to the stop word file
        /// </summary>
        private string _stopWordFile;

        /// <summary>
        /// List to contine the stop word list, as it's built.
        /// </summary>
        private BlockingCollection<string> _stopWordList;

        public LoadStopWords(string StopWordFile, ref BlockingCollection<string> StopWordList)
        {
            this._stopWordFile = StopWordFile;
            this._stopWordList = StopWordList;

            //Use the processing completed event to report that the list is completly filled.
            ProcessingCompleted += delegate(object sender, EventArgs e)
            {
                this._stopWordList.CompleteAdding();
            };
        }

        /// <summary>
        /// Fills the list with the stop words. Stop words are
        ///  presumed to come in one word per line. This is
        ///  not verified.
        /// Processing Started and completed are implemented.
        /// </summary>
        public void executeStep()
        {
            if( ProcessingStarted != null )
                ProcessingStarted(this, new EventArgs());

            //Open file, read in conttents. Put in _stopWordList.
            using (StreamReader _file2 = File.OpenText(this._stopWordFile))
            {
                while (!_file2.EndOfStream)
                {
                    string line = _file2.ReadLine();
                    this._stopWordList.Add(line);
                }
                _file2.Close();
            }

            if (ProcessingCompleted != null) 
                ProcessingCompleted(this, new EventArgs());
        }
    }
}
