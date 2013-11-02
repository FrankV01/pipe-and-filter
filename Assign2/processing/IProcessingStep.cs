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
