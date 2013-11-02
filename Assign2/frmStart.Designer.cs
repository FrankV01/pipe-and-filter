namespace Assign2_POC
{
    partial class frmStart
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnGo = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnPickStopListFile = new System.Windows.Forms.Button();
            this.txtStopListFile = new System.Windows.Forms.TextBox();
            this.txtInputFile = new System.Windows.Forms.TextBox();
            this.btnPickInputFile = new System.Windows.Forms.Button();
            this.lblStopList = new System.Windows.Forms.Label();
            this.lblInput = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnGo
            // 
            this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGo.Location = new System.Drawing.Point(12, 71);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(234, 34);
            this.btnGo.TabIndex = 0;
            this.btnGo.Text = "Go!";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.button1_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "*.txt";
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Text Files|*.txt|All Files|*.*";
            this.openFileDialog1.SupportMultiDottedExtensions = true;
            // 
            // btnPickStopListFile
            // 
            this.btnPickStopListFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPickStopListFile.Location = new System.Drawing.Point(215, 10);
            this.btnPickStopListFile.Name = "btnPickStopListFile";
            this.btnPickStopListFile.Size = new System.Drawing.Size(31, 23);
            this.btnPickStopListFile.TabIndex = 1;
            this.btnPickStopListFile.Text = "...";
            this.btnPickStopListFile.UseVisualStyleBackColor = true;
            this.btnPickStopListFile.Click += new System.EventHandler(this.btnPickStopListFile_Click);
            // 
            // txtStopListFile
            // 
            this.txtStopListFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStopListFile.Location = new System.Drawing.Point(66, 13);
            this.txtStopListFile.Name = "txtStopListFile";
            this.txtStopListFile.Size = new System.Drawing.Size(143, 20);
            this.txtStopListFile.TabIndex = 2;
            this.txtStopListFile.Text = "C:\\Users\\Frank\\hg-repos\\DePaul-wrk\\Classwork\\Term-14-SE480\\Assignments\\2\\data-fil" +
    "es\\stopwords.txt";
            // 
            // txtInputFile
            // 
            this.txtInputFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInputFile.Location = new System.Drawing.Point(66, 39);
            this.txtInputFile.Name = "txtInputFile";
            this.txtInputFile.Size = new System.Drawing.Size(143, 20);
            this.txtInputFile.TabIndex = 4;
            this.txtInputFile.Text = "C:\\Users\\Frank\\hg-repos\\DePaul-wrk\\Classwork\\Term-14-SE480\\Assignments\\2\\data-fil" +
    "es\\Text1.txt";
            // 
            // btnPickInputFile
            // 
            this.btnPickInputFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPickInputFile.Location = new System.Drawing.Point(215, 42);
            this.btnPickInputFile.Name = "btnPickInputFile";
            this.btnPickInputFile.Size = new System.Drawing.Size(31, 23);
            this.btnPickInputFile.TabIndex = 3;
            this.btnPickInputFile.Text = "...";
            this.btnPickInputFile.UseVisualStyleBackColor = true;
            this.btnPickInputFile.Click += new System.EventHandler(this.btnPickInputFile_Click);
            // 
            // lblStopList
            // 
            this.lblStopList.AutoSize = true;
            this.lblStopList.Location = new System.Drawing.Point(9, 16);
            this.lblStopList.Name = "lblStopList";
            this.lblStopList.Size = new System.Drawing.Size(51, 13);
            this.lblStopList.TabIndex = 5;
            this.lblStopList.Text = "Stop List:";
            // 
            // lblInput
            // 
            this.lblInput.AutoSize = true;
            this.lblInput.Location = new System.Drawing.Point(26, 42);
            this.lblInput.Name = "lblInput";
            this.lblInput.Size = new System.Drawing.Size(34, 13);
            this.lblInput.TabIndex = 6;
            this.lblInput.Text = "Input:";
            this.lblInput.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // frmStart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(258, 117);
            this.Controls.Add(this.lblInput);
            this.Controls.Add(this.lblStopList);
            this.Controls.Add(this.txtInputFile);
            this.Controls.Add(this.btnPickInputFile);
            this.Controls.Add(this.txtStopListFile);
            this.Controls.Add(this.btnPickStopListFile);
            this.Controls.Add(this.btnGo);
            this.Name = "frmStart";
            this.Text = "Assignment 2";
            this.Load += new System.EventHandler(this.onLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGo;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnPickStopListFile;
        private System.Windows.Forms.TextBox txtStopListFile;
        private System.Windows.Forms.TextBox txtInputFile;
        private System.Windows.Forms.Button btnPickInputFile;
        private System.Windows.Forms.Label lblStopList;
        private System.Windows.Forms.Label lblInput;
    }
}

