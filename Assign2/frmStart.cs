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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Assign2_POC.processing;
using Assign2_POC.processing.util;
using System.Collections;
using System.Collections.Concurrent;
using System.Threading;
using System.Diagnostics;

namespace Assign2_POC
{
    /// <summary>
    /// Main UI form. Supports taking in the the parameters and getting
    /// processing going. It also contains the different processing elements (threads and such)
    /// </summary>
    public partial class frmStart : Form
    {
        //The following are a special type of arrays which block until there is an item
        // contained within. This allows us to move forward with processing only if there
        // is an item in the queue.
        private BlockingCollection<string> _wordList_step1;
        private BlockingCollection<string> _wordList_step2;
        private BlockingCollection<string> _wordList_step3;
        private BlockingCollection<string> _wordList_step4;
        private BlockingCollection<string> _wordList_step5;

        // Same data type as above but this is for the stop list. 
        private BlockingCollection<string> _stopList;

        private IDictionary<object, Stopwatch> _instr;

        /// <summary>
        /// Used for UI manipulation to make the Button State
        /// change thread safe.
        /// </summary>
        /// <param name="IsWorking">See the 'changeState' method</param>
        delegate void ChangeStateCallback(bool IsWorking);

        public frmStart()
        {
            InitializeComponent();
        }

        // Change the prevent duplicate requests
        //  and kick off the background worker which will setup
        //  the threads that will manage the pipeline.
        private void button1_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                this.changeState(true);
                backgroundWorker1.RunWorkerAsync();
            }
        }


        /// <summary>
        /// Changes the state of the button as approriate, 
        /// but supports a thread safe method of completing 
        /// the task.
        /// </summary>
        /// <param name="IsWorking">See the 'changeState' method</param>
        private void changeState(bool IsWorking)
        {
            if (btnGo.InvokeRequired)
            {
                ChangeStateCallback d = new ChangeStateCallback(changeState);
                this.Invoke(d, new object[] { IsWorking });
            }
            else
            {
                btnGo.Enabled = (!IsWorking);
                btnGo.Text = (IsWorking) ? "Working, please wait..." : "Go";
            }
        }

        // This does the setup of the pipeline (backgroundWorker is another thread)
        //  This setups the word list objects (new objects) and then builds a list
        //  of threads to be executed. Each thread is a step in the pipeline. The 
        //  order started doesn't much matter. New steps can be added by
        //  inserting a step {anywhere} within the thread list. Once all threads
        //  are ready to go, all are started. Threads are smart enough to process
        //  when they can and wait when they shouldn't be processing.
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            this._wordList_step1 = new BlockingCollection<string>();
            this._wordList_step2 = new BlockingCollection<string>();
            this._wordList_step3 = new BlockingCollection<string>();
            this._wordList_step4 = new BlockingCollection<string>();
            this._wordList_step5 = new BlockingCollection<string>();

            this._stopList = new BlockingCollection<string>();

            List<Thread> threadList2 = new List<Thread>();

            // Load the stop words. Nothing blocks on start
            IProcessingStep stepStopWord = new LoadStopWords(txtStopListFile.Text, ref _stopList);
#if DEBUG
            stepStopWord.ProcessingStarted += startInstrumentationTimer;
            stepStopWord.ProcessingCompleted += stopInstrumentionTimer;
#endif
            Thread stopWordThread = new Thread(new ThreadStart(stepStopWord.executeStep));
            stopWordThread.Name = "Load Stop Words";
            threadList2.Add(stopWordThread);

            // Load the input. Nothing blocks on start
            IProcessingStep stepInputFile = new DataInput(txtInputFile.Text, ref _wordList_step1);
#if DEBUG
            stepInputFile.ProcessingStarted += startInstrumentationTimer;
            stepInputFile.ProcessingCompleted += stopInstrumentionTimer;
#endif
            Thread inputThread = new Thread(new ThreadStart(stepInputFile.executeStep));
            inputThread.Name = "Load Input";
            threadList2.Add(inputThread);

            //Remove punctuation. 
            IProcessingStep stepRemovePunctuation = new RemovePunctuation(ref _wordList_step1, ref _wordList_step2);
#if DEBUG
            stepRemovePunctuation.ProcessingStarted += startInstrumentationTimer;
            stepRemovePunctuation.ProcessingCompleted += stopInstrumentionTimer;
#endif
            Thread removePunctuationThread = new Thread(new ThreadStart(stepRemovePunctuation.executeStep));
            removePunctuationThread.Name = "Remove Punctation";
            threadList2.Add(removePunctuationThread);

            //We should de-dup the list...
            //IProcessingStep stepDeDup = new DeDup(ref _wordList_step2, ref _wordList_step3);
            //Thread deDupThread = new Thread(new ThreadStart(stepDeDup.executeStep));
            //threadList2.Add(deDupThread);

            //Remove stop words - At this point, the stop list must be ready to go (100% loaded). There is logic in the 
            // 'RemoveStopWords' Processing Step to assure this.
            IProcessingStep stepRemoveStopWords = new RemoveStopWords(_stopList, ref _wordList_step2, ref _wordList_step4);
#if DEBUG
            stepRemoveStopWords.ProcessingStarted += startInstrumentationTimer;
            stepRemoveStopWords.ProcessingCompleted += stopInstrumentionTimer;
#endif
            Thread removeStopWords = new Thread(new ThreadStart(stepRemoveStopWords.executeStep));
            removeStopWords.Name = "Remove Stop Words";
            stepRemoveStopWords.SuspendProcessing += delegate(object delSender, SuspendRequestEventArgs evtArg) {
                System.Diagnostics.Debug.WriteLine("Hit Suspend Processing. Waiting...");
                Thread.Sleep(evtArg.SuspendTimeMillaSeconds);
                removeStopWords = new Thread(new ThreadStart(stepRemoveStopWords.executeStep));
                removeStopWords.Name = "Remove Stop Words re-created thread";
                removeStopWords.Start();
            };
            threadList2.Add(removeStopWords);
            
            //Stemming algorthum.
            IProcessingStep stepStemming = new ApplyStemming(ref _wordList_step4, ref _wordList_step5);
#if DEBUG
            stepStemming.ProcessingStarted += startInstrumentationTimer;
            stepStemming.ProcessingCompleted += stopInstrumentionTimer;
#endif
            Thread StemmingThread = new Thread(new ThreadStart(stepStemming.executeStep));
            StemmingThread.Name = "Applying Stemming";
            threadList2.Add(StemmingThread);

            //20 most frequently occurring terms - DeDupping may need to be removed.. huh.
            // Return data via completed event handler? Seems reasonable.
            IProcessingStep stepOutput = new DataOutput_TwentyCommon(ref _wordList_step5);
#if DEBUG
            stepOutput.ProcessingStarted += startInstrumentationTimer;
            stepOutput.ProcessingCompleted += stopInstrumentionTimer;
#endif
            Thread outputThread = new Thread(new ThreadStart(stepOutput.executeStep));
            outputThread.Name = "Preparing Final Output";
            threadList2.Add(outputThread);

            //Start all the threads.
            foreach (var th in threadList2)
            {
                th.Start();
            }

            //Clean up processsing (as a delegate); show the results and reset the main form.
            stepOutput.ProcessingCompleted += delegate(object sender3, EventArgs evt3)
            {
                //MessageBox.Show("stepOutput Done.");
                TwentyCommonEventArgs evtArgs = (TwentyCommonEventArgs)evt3;
                ArrayList _terms = evtArgs.Terms;
                frmResults _results = new frmResults(_terms);
                _results.ShowDialog();

                this.changeState(false);
            };
        }

        private void startInstrumentationTimer(object sender, EventArgs e)
        {
            if (_instr == null)
            {
                _instr = new Dictionary<object, Stopwatch>();
            }


            if (!_instr.ContainsKey(sender))
            {
                _instr[sender] = new Stopwatch();
                _instr[sender].Start();
            }
            else
            {
                _instr[sender].Start();
            }
        }
        private void stopInstrumentionTimer(object sender, EventArgs e)
        {
            if (_instr == null)
            {
                return;
            }

            if (_instr[sender] != null)
            {
                _instr[sender].Stop();
                TimeSpan ts = _instr[sender].Elapsed;
                string sSender = sender.ToString();

                StringBuilder _sb = new StringBuilder();
                _sb.Append("  -->  InstrumentionTimer for ").Append(sSender).Append(": ");
                _sb.Append(ts.Milliseconds).Append(" milliseconds");

                System.Diagnostics.Debug.WriteLine( _sb.ToString() );
                

                _instr[sender].Reset();
            }
        }

        //Handles the user picking the stop list file
        private void btnPickStopListFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = ""; //Clear previous selection (we're reusing this)
            openFileDialog1.Title = "Select the Stop Word List...";
            DialogResult result = openFileDialog1.ShowDialog();
            if( result == DialogResult.OK )
                txtStopListFile.Text = openFileDialog1.FileName;
        }

        //Handles the user picking the input list file
        private void btnPickInputFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = ""; //Clear previous selection (we're reusing this)
            openFileDialog1.Title = "Select the File to Process...";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
                txtInputFile.Text = openFileDialog1.FileName;
        }

        private void onLoad(object sender, EventArgs e)
        {
            
#if DEBUG
            return;   
#endif
            //If not compiled in debug mode, set the path to the preferred location of the Prof.
            string path = @"C:\SE480\DataFiles\";
            this.txtInputFile.Text = path;
            this.txtStopListFile.Text = path;
        }
    }
}
