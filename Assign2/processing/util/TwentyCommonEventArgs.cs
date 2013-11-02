using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assign2_POC.processing.util
{
    /// <summary>
    /// An Event Args class which can contain 
    ///  an ArrayList
    /// </summary>
    class TwentyCommonEventArgs : EventArgs
    {
        public TwentyCommonEventArgs()
        {
            this.Terms = null;
        }

        /// <summary>
        /// Array list of terms perhaps using a dictionary where key=word and val = occurances.
        /// </summary>
        public ArrayList Terms { get; set; }
    }
}
