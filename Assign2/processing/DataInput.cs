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
using System.IO;
using System.Linq;
using System.Text;

namespace Assign2_POC.processing
{
    /// <summary>
    /// Processing step to load the input data
    /// </summary>
    class DataInput : IProcessingStep
    {
        public event EventHandler ProcessingCompleted;
        public event EventHandler ProcessingStarted;
        public event RequestSuspend SuspendProcessing;

        /// <summary>
        /// File path of the incoming file.
        /// </summary>
        private string _filePath;

        /// <summary>
        /// The built word list.
        /// </summary>
        private BlockingCollection<string> _wordList;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="FilePath">The path of the file</param>
        /// <param name="WordList">A reference to the WordList object. This is how the data is returned.</param>
        public DataInput(string FilePath, ref BlockingCollection<string> WordList)
        {
            this._filePath = FilePath;
            this._wordList = WordList;

            //Use the processing completed event to report that the list is completly filled.
            this.ProcessingCompleted += delegate(object sender, EventArgs evt)
            {
                this._wordList.CompleteAdding();
            };
        }

        /// <summary>
        /// Performs the work to load the file in to the Collection.
        /// </summary>
        public void executeStep()
        {
            //Open the file...
            //Read in 'x' amount of file and put it in to the 'queue'

            if (ProcessingStarted != null)
                ProcessingStarted(this, new EventArgs());

            using (StreamReader _file2 = File.OpenText(this._filePath))
            {
                while (!_file2.EndOfStream)
                {
                    string line = _file2.ReadLine();
                    string[] words = line.Split(' ', '\t'); //This logic will get more complex.
                    foreach (string word in words)
                    {
                        _wordList.Add(word.ToLower());
                    }
                }
            }

            if (ProcessingCompleted != null) 
                ProcessingCompleted(this, new EventArgs());
        }
    }
}
