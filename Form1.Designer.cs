namespace lidwave
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tabControl1 = new TabControl();
            tabPage2 = new TabPage();
            IscTv = new Label();
            loadICSTV = new Button();
            tabPage1 = new TabPage();
            procBox = new GroupBox();
            label6 = new Label();
            peaksView = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            therr = new ListView();
            columnHeader1 = new ColumnHeader();
            fail = new Label();
            pass = new Label();
            label5 = new Label();
            nBins = new NumericUpDown();
            label4 = new Label();
            procBuff = new Button();
            gBins = new NumericUpDown();
            edges = new CheckBox();
            tBins = new NumericUpDown();
            label2 = new Label();
            label1 = new Label();
            label3 = new Label();
            fftPlot = new ScottPlot.FormsPlot();
            buffNum = new NumericUpDown();
            tvName = new Label();
            loadCFRTV = new Button();
            groupBox1 = new GroupBox();
            fromTV = new RadioButton();
            fromSum = new RadioButton();
            tabControl1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage1.SuspendLayout();
            procBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)peaksView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nBins).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gBins).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tBins).BeginInit();
            ((System.ComponentModel.ISupportInitialize)buffNum).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1447, 993);
            tabControl1.TabIndex = 0;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(IscTv);
            tabPage2.Controls.Add(loadICSTV);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Size = new Size(1439, 960);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "ICS";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // IscTv
            // 
            IscTv.AutoSize = true;
            IscTv.Location = new Point(177, 48);
            IscTv.Name = "IscTv";
            IscTv.Size = new Size(18, 20);
            IscTv.TabIndex = 4;
            IscTv.Text = "...";
            // 
            // loadICSTV
            // 
            loadICSTV.Location = new Point(49, 37);
            loadICSTV.Name = "loadICSTV";
            loadICSTV.Size = new Size(103, 43);
            loadICSTV.TabIndex = 3;
            loadICSTV.Text = "load TV";
            loadICSTV.UseVisualStyleBackColor = true;
            loadICSTV.Click += loadICSTV_Click;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(procBox);
            tabPage1.Controls.Add(label3);
            tabPage1.Controls.Add(fftPlot);
            tabPage1.Controls.Add(buffNum);
            tabPage1.Controls.Add(tvName);
            tabPage1.Controls.Add(loadCFRTV);
            tabPage1.Controls.Add(groupBox1);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1439, 960);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "CFAR";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // procBox
            // 
            procBox.Controls.Add(label6);
            procBox.Controls.Add(peaksView);
            procBox.Controls.Add(therr);
            procBox.Controls.Add(fail);
            procBox.Controls.Add(pass);
            procBox.Controls.Add(label5);
            procBox.Controls.Add(nBins);
            procBox.Controls.Add(label4);
            procBox.Controls.Add(procBuff);
            procBox.Controls.Add(gBins);
            procBox.Controls.Add(edges);
            procBox.Controls.Add(tBins);
            procBox.Controls.Add(label2);
            procBox.Controls.Add(label1);
            procBox.Enabled = false;
            procBox.Location = new Point(23, 220);
            procBox.Name = "procBox";
            procBox.Size = new Size(284, 661);
            procBox.TabIndex = 12;
            procBox.TabStop = false;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(17, 464);
            label6.Name = "label6";
            label6.Size = new Size(116, 20);
            label6.TabIndex = 17;
            label6.Text = "Threshold errors";
            // 
            // peaksView
            // 
            peaksView.AllowUserToAddRows = false;
            peaksView.AllowUserToDeleteRows = false;
            peaksView.AllowUserToResizeColumns = false;
            peaksView.AllowUserToResizeRows = false;
            peaksView.ColumnHeadersHeight = 29;
            peaksView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            peaksView.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3 });
            peaksView.Location = new Point(17, 319);
            peaksView.Name = "peaksView";
            peaksView.ReadOnly = true;
            peaksView.RowHeadersVisible = false;
            peaksView.RowHeadersWidth = 51;
            peaksView.ScrollBars = ScrollBars.None;
            peaksView.Size = new Size(261, 125);
            peaksView.TabIndex = 16;
            // 
            // Column1
            // 
            Column1.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Column1.HeaderText = "Peak";
            Column1.MinimumWidth = 6;
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Width = 50;
            // 
            // Column2
            // 
            Column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Column2.HeaderText = "TV";
            Column2.MinimumWidth = 6;
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            Column2.Width = 125;
            // 
            // Column3
            // 
            Column3.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Column3.HeaderText = "Calc";
            Column3.MinimumWidth = 6;
            Column3.Name = "Column3";
            Column3.ReadOnly = true;
            Column3.Width = 125;
            // 
            // therr
            // 
            therr.Columns.AddRange(new ColumnHeader[] { columnHeader1 });
            therr.Location = new Point(17, 487);
            therr.Name = "therr";
            therr.Size = new Size(160, 150);
            therr.TabIndex = 15;
            therr.UseCompatibleStateImageBehavior = false;
            therr.View = View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Index   TV   Calc";
            columnHeader1.Width = 150;
            // 
            // fail
            // 
            fail.AutoSize = true;
            fail.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            fail.ForeColor = Color.Red;
            fail.Location = new Point(154, 280);
            fail.Name = "fail";
            fail.Size = new Size(44, 28);
            fail.TabIndex = 14;
            fail.Text = "Fail";
            fail.Visible = false;
            // 
            // pass
            // 
            pass.AutoSize = true;
            pass.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            pass.ForeColor = Color.Green;
            pass.Location = new Point(150, 280);
            pass.Name = "pass";
            pass.Size = new Size(53, 28);
            pass.TabIndex = 14;
            pass.Text = "Pass";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Underline);
            label5.Location = new Point(17, 284);
            label5.Name = "label5";
            label5.Size = new Size(116, 20);
            label5.TabIndex = 13;
            label5.Text = "Compare results";
            // 
            // nBins
            // 
            nBins.Location = new Point(106, 212);
            nBins.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            nBins.Name = "nBins";
            nBins.Size = new Size(71, 27);
            nBins.TabIndex = 12;
            nBins.TextAlign = HorizontalAlignment.Right;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(17, 217);
            label4.Name = "label4";
            label4.Size = new Size(204, 40);
            label4.TabIndex = 11;
            label4.Text = "Ignore first                        Bins\r\n(Low freq. noise)";
            // 
            // procBuff
            // 
            procBuff.Location = new Point(44, 26);
            procBuff.Name = "procBuff";
            procBuff.Size = new Size(159, 31);
            procBuff.TabIndex = 5;
            procBuff.Text = "Process buffer";
            procBuff.UseVisualStyleBackColor = true;
            procBuff.Click += procBuff_Click;
            // 
            // gBins
            // 
            gBins.Location = new Point(56, 164);
            gBins.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            gBins.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            gBins.Name = "gBins";
            gBins.Size = new Size(71, 27);
            gBins.TabIndex = 9;
            gBins.TextAlign = HorizontalAlignment.Right;
            gBins.Value = new decimal(new int[] { 4, 0, 0, 0 });
            // 
            // edges
            // 
            edges.AutoSize = true;
            edges.Checked = true;
            edges.CheckState = CheckState.Checked;
            edges.Location = new Point(31, 79);
            edges.Name = "edges";
            edges.Size = new Size(163, 24);
            edges.TabIndex = 10;
            edges.Text = "One sided on edges";
            edges.UseVisualStyleBackColor = true;
            edges.Visible = false;
            // 
            // tBins
            // 
            tBins.Location = new Point(56, 123);
            tBins.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            tBins.Name = "tBins";
            tBins.Size = new Size(71, 27);
            tBins.TabIndex = 9;
            tBins.TextAlign = HorizontalAlignment.Right;
            tBins.Value = new decimal(new int[] { 32, 0, 0, 0 });
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(31, 167);
            label2.Name = "label2";
            label2.Size = new Size(134, 20);
            label2.TabIndex = 8;
            label2.Text = "G                      Bins";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(31, 126);
            label1.Name = "label1";
            label1.Size = new Size(136, 20);
            label1.TabIndex = 8;
            label1.Text = "T                       Bins";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(24, 142);
            label3.Name = "label3";
            label3.Size = new Size(176, 20);
            label3.TabIndex = 11;
            label3.Text = "Buffer number to process";
            // 
            // fftPlot
            // 
            fftPlot.Location = new Point(332, 128);
            fftPlot.Margin = new Padding(5, 4, 5, 4);
            fftPlot.Name = "fftPlot";
            fftPlot.Size = new Size(1057, 654);
            fftPlot.TabIndex = 6;
            // 
            // buffNum
            // 
            buffNum.Location = new Point(54, 174);
            buffNum.Name = "buffNum";
            buffNum.Size = new Size(96, 27);
            buffNum.TabIndex = 3;
            buffNum.ValueChanged += buffNum_ValueChanged;
            // 
            // tvName
            // 
            tvName.AutoSize = true;
            tvName.Location = new Point(332, 44);
            tvName.Name = "tvName";
            tvName.Size = new Size(18, 20);
            tvName.TabIndex = 2;
            tvName.Text = "...";
            // 
            // loadCFRTV
            // 
            loadCFRTV.Location = new Point(204, 33);
            loadCFRTV.Name = "loadCFRTV";
            loadCFRTV.Size = new Size(103, 43);
            loadCFRTV.TabIndex = 1;
            loadCFRTV.Text = "load TV";
            loadCFRTV.UseVisualStyleBackColor = true;
            loadCFRTV.Click += loadTV_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(fromTV);
            groupBox1.Controls.Add(fromSum);
            groupBox1.Location = new Point(23, 21);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(166, 105);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Input type";
            // 
            // fromTV
            // 
            fromTV.AutoSize = true;
            fromTV.Checked = true;
            fromTV.Location = new Point(17, 63);
            fromTV.Name = "fromTV";
            fromTV.Size = new Size(91, 24);
            fromTV.TabIndex = 0;
            fromTV.TabStop = true;
            fromTV.Text = "Power TV";
            fromTV.UseVisualStyleBackColor = true;
            // 
            // fromSum
            // 
            fromSum.AutoSize = true;
            fromSum.Enabled = false;
            fromSum.Location = new Point(17, 31);
            fromSum.Name = "fromSum";
            fromSum.Size = new Size(128, 24);
            fromSum.TabIndex = 0;
            fromSum.Text = "Previous phase";
            fromSum.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1447, 993);
            Controls.Add(tabControl1);
            Name = "Form1";
            Text = "Lidwave";
            tabControl1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            procBox.ResumeLayout(false);
            procBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)peaksView).EndInit();
            ((System.ComponentModel.ISupportInitialize)nBins).EndInit();
            ((System.ComponentModel.ISupportInitialize)gBins).EndInit();
            ((System.ComponentModel.ISupportInitialize)tBins).EndInit();
            ((System.ComponentModel.ISupportInitialize)buffNum).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private GroupBox groupBox1;
        private RadioButton fromTV;
        private RadioButton fromSum;
        private Button loadCFRTV;
        public Label tvName;
        private Button procBuff;
        public NumericUpDown buffNum;
        private ScottPlot.FormsPlot fftPlot;
        public NumericUpDown tBins;
        private Label label2;
        private Label label1;
        public NumericUpDown gBins;
        private CheckBox edges;
        private Label label3;
        public GroupBox procBox;
        public NumericUpDown nBins;
        private Label label4;
        private Label label5;
        public Label pass;
        public Label fail;
        public ListView therr;
        public DataGridView peaksView;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private Label label6;
        private ColumnHeader columnHeader1;
        private Label IscTv;
        private Button loadICSTV;
    }
}
