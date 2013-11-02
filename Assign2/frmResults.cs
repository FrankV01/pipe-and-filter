using Assign2_POC.processing.util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Assign2_POC
{
    /// <summary>
    /// The result form - shows the results to the user.
    /// </summary>
    public partial class frmResults : Form
    {
        /// <summary>
        /// The results to display passed in via the constructor.
        /// </summary>
        private ArrayList _results;

        public frmResults(ArrayList alResults)
        {
            InitializeComponent();
            _results = alResults;
        }

        /// <summary>
        /// Builds a simple visual output of the data with a string builder.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onLoad(object sender, EventArgs e)
        {
            if (_results != null && _results.Count > 0)
            {
                txtResults.Font = new System.Drawing.Font(txtResults.Font, FontStyle.Regular);

                StringBuilder _sb = new StringBuilder();
                foreach (OrderedWord itm in _results)
                {
                    _sb.Append(itm.NumOccurances);
                    _sb.Append(" - ");
                    _sb.Append(itm.stemmedWord);
                    _sb.Append(Environment.NewLine);
                }
                txtResults.Text = _sb.ToString();
            }
            else
            {
                txtResults.Font = new System.Drawing.Font(txtResults.Font, FontStyle.Bold);
                txtResults.Text = "[No results]";
            }
        }
    }
}
