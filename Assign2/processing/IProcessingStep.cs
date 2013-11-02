using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assign2_POC.processing
{
    /// <summary>
    /// A custom Event Args class which allows
    ///  including the Suspend time in millaseconds.
    /// </summary>
    class SuspendRequestEventArgs : EventArgs 
    {
        /// <summary>
        /// The time in millseconds the thread should be suspended for.
        /// </summary>
        public int SuspendTimeMillaSeconds { get; set; }
    }

    /// <summary>
    /// Delegate for the SuspendProcessing event. Defines the method
    ///  interface which uses a custom EventArgs object.
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">Event Args Instance</param>
    delegate void RequestSuspend(object sender, SuspendRequestEventArgs e);

    /// <summary>
    /// Represents a step in the pipline.
    /// </summary>
    interface IProcessingStep
    {
        /// <summary>
        /// Executed when processing is completed.
        /// </summary>
        event EventHandler ProcessingCompleted;

        /// <summary>
        /// Executed when processing is started.
        /// </summary>
        event EventHandler ProcessingStarted;

        /// <summary>
        /// Executed should the processing need to be suspended
        ///  temporarly. Implementation details is up to the 
        ///  implementor
        /// </summary>
        event RequestSuspend SuspendProcessing;
        
        /// <summary>
        /// The logic to be implemented by the processing step
        /// </summary>
        void executeStep();
    }
}
