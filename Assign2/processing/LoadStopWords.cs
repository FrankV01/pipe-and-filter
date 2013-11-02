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
