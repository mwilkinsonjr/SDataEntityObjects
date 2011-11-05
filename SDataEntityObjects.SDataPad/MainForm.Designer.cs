namespace SDataEntityObjects.SDataPad
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitTopBottom = new System.Windows.Forms.SplitContainer();
            this.grpboxQuery = new System.Windows.Forms.GroupBox();
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblOtherInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.grpboxResults = new System.Windows.Forms.GroupBox();
            this.dgvResults = new System.Windows.Forms.DataGridView();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.cmdAbout = new System.Windows.Forms.ToolStripButton();
            this.cmdRunQuery = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmdClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cbxExamples = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.cmdClearResults = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.cmdConnectionSettings = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.cmdLaunchExplorer = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.splitTopBottom.Panel1.SuspendLayout();
            this.splitTopBottom.Panel2.SuspendLayout();
            this.splitTopBottom.SuspendLayout();
            this.grpboxQuery.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.grpboxResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).BeginInit();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitTopBottom
            // 
            this.splitTopBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitTopBottom.Location = new System.Drawing.Point(0, 0);
            this.splitTopBottom.Name = "splitTopBottom";
            this.splitTopBottom.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitTopBottom.Panel1
            // 
            this.splitTopBottom.Panel1.Controls.Add(this.grpboxQuery);
            // 
            // splitTopBottom.Panel2
            // 
            this.splitTopBottom.Panel2.Controls.Add(this.statusStrip);
            this.splitTopBottom.Panel2.Controls.Add(this.grpboxResults);
            this.splitTopBottom.Size = new System.Drawing.Size(675, 400);
            this.splitTopBottom.SplitterDistance = 158;
            this.splitTopBottom.TabIndex = 3;
            this.splitTopBottom.TabStop = false;
            // 
            // grpboxQuery
            // 
            this.grpboxQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpboxQuery.Controls.Add(this.txtQuery);
            this.grpboxQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpboxQuery.Location = new System.Drawing.Point(3, 28);
            this.grpboxQuery.Name = "grpboxQuery";
            this.grpboxQuery.Size = new System.Drawing.Size(669, 127);
            this.grpboxQuery.TabIndex = 0;
            this.grpboxQuery.TabStop = false;
            this.grpboxQuery.Text = "Query Pane";
            // 
            // txtQuery
            // 
            this.txtQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtQuery.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQuery.Location = new System.Drawing.Point(3, 16);
            this.txtQuery.Multiline = true;
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtQuery.Size = new System.Drawing.Size(663, 108);
            this.txtQuery.TabIndex = 0;
            this.txtQuery.WordWrap = false;
            this.txtQuery.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtQuery_KeyDown);
            this.txtQuery.Enter += new System.EventHandler(this.txtQuery_Enter);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.lblOtherInfo});
            this.statusStrip.Location = new System.Drawing.Point(0, 216);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(675, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblStatus.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(560, 17);
            this.lblStatus.Spring = true;
            this.lblStatus.Text = "Ready";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOtherInfo
            // 
            this.lblOtherInfo.AutoSize = false;
            this.lblOtherInfo.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblOtherInfo.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.lblOtherInfo.Name = "lblOtherInfo";
            this.lblOtherInfo.Size = new System.Drawing.Size(100, 17);
            this.lblOtherInfo.Text = "MaxRows: 20";
            // 
            // grpboxResults
            // 
            this.grpboxResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpboxResults.Controls.Add(this.dgvResults);
            this.grpboxResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpboxResults.Location = new System.Drawing.Point(3, 3);
            this.grpboxResults.Name = "grpboxResults";
            this.grpboxResults.Size = new System.Drawing.Size(669, 210);
            this.grpboxResults.TabIndex = 1;
            this.grpboxResults.TabStop = false;
            this.grpboxResults.Text = "Results Pane";
            // 
            // dgvResults
            // 
            this.dgvResults.AllowUserToAddRows = false;
            this.dgvResults.AllowUserToDeleteRows = false;
            this.dgvResults.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResults.Location = new System.Drawing.Point(3, 16);
            this.dgvResults.Name = "dgvResults";
            this.dgvResults.ReadOnly = true;
            this.dgvResults.Size = new System.Drawing.Size(663, 191);
            this.dgvResults.TabIndex = 1;
            this.dgvResults.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvResults_CellFormatting);
            this.dgvResults.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvResults_DataError);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdAbout,
            this.cmdRunQuery,
            this.toolStripSeparator1,
            this.cmdClear,
            this.toolStripSeparator3,
            this.cbxExamples,
            this.toolStripSeparator2,
            this.toolStripSeparator4,
            this.cmdClearResults,
            this.toolStripSeparator5,
            this.cmdConnectionSettings,
            this.toolStripSeparator6,
            this.cmdLaunchExplorer,
            this.toolStripSeparator7});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(675, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip";
            // 
            // cmdAbout
            // 
            this.cmdAbout.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cmdAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cmdAbout.Image = ((System.Drawing.Image)(resources.GetObject("cmdAbout.Image")));
            this.cmdAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdAbout.Name = "cmdAbout";
            this.cmdAbout.Size = new System.Drawing.Size(40, 22);
            this.cmdAbout.Text = "About";
            this.cmdAbout.Click += new System.EventHandler(this.cmdAbout_Click);
            // 
            // cmdRunQuery
            // 
            this.cmdRunQuery.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cmdRunQuery.Image = ((System.Drawing.Image)(resources.GetObject("cmdRunQuery.Image")));
            this.cmdRunQuery.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdRunQuery.Name = "cmdRunQuery";
            this.cmdRunQuery.Size = new System.Drawing.Size(63, 22);
            this.cmdRunQuery.Text = "Run Query";
            this.cmdRunQuery.ToolTipText = "Run Query (F5)";
            this.cmdRunQuery.Click += new System.EventHandler(this.cmdRunQuery_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // cmdClear
            // 
            this.cmdClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cmdClear.Image = ((System.Drawing.Image)(resources.GetObject("cmdClear.Image")));
            this.cmdClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdClear.Name = "cmdClear";
            this.cmdClear.Size = new System.Drawing.Size(69, 22);
            this.cmdClear.Text = "Clear Query";
            this.cmdClear.ToolTipText = "Clear Query (F6)";
            this.cmdClear.Click += new System.EventHandler(this.cmdClear_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // cbxExamples
            // 
            this.cbxExamples.Items.AddRange(new object[] {
            "Example 1",
            "Example 2",
            "Example 3",
            "Example 4"});
            this.cbxExamples.Name = "cbxExamples";
            this.cbxExamples.Size = new System.Drawing.Size(121, 25);
            this.cbxExamples.Text = "[New Query]";
            this.cbxExamples.SelectedIndexChanged += new System.EventHandler(this.cbxExamples_SelectedIndexChanged);
            this.cbxExamples.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbxExamples_KeyDown);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // cmdClearResults
            // 
            this.cmdClearResults.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cmdClearResults.Image = ((System.Drawing.Image)(resources.GetObject("cmdClearResults.Image")));
            this.cmdClearResults.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdClearResults.Name = "cmdClearResults";
            this.cmdClearResults.Size = new System.Drawing.Size(74, 22);
            this.cmdClearResults.Text = "Clear Results";
            this.cmdClearResults.ToolTipText = "Clear Results (F7)";
            this.cmdClearResults.Click += new System.EventHandler(this.cmdClearResults_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // cmdConnectionSettings
            // 
            this.cmdConnectionSettings.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cmdConnectionSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cmdConnectionSettings.Image = ((System.Drawing.Image)(resources.GetObject("cmdConnectionSettings.Image")));
            this.cmdConnectionSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdConnectionSettings.Name = "cmdConnectionSettings";
            this.cmdConnectionSettings.Size = new System.Drawing.Size(107, 22);
            this.cmdConnectionSettings.Text = "Connection Settings";
            this.cmdConnectionSettings.Click += new System.EventHandler(this.cmdConnectionSettings_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // cmdLaunchExplorer
            // 
            this.cmdLaunchExplorer.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cmdLaunchExplorer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cmdLaunchExplorer.Image = ((System.Drawing.Image)(resources.GetObject("cmdLaunchExplorer.Image")));
            this.cmdLaunchExplorer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdLaunchExplorer.Name = "cmdLaunchExplorer";
            this.cmdLaunchExplorer.Size = new System.Drawing.Size(119, 22);
            this.cmdLaunchExplorer.Text = "Launch Entity Explorer";
            this.cmdLaunchExplorer.Click += new System.EventHandler(this.cmdLaunchExplorer_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(675, 400);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.splitTopBottom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "SDataPad - Localhost:3333 [lee]";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitTopBottom.Panel1.ResumeLayout(false);
            this.splitTopBottom.Panel2.ResumeLayout(false);
            this.splitTopBottom.Panel2.PerformLayout();
            this.splitTopBottom.ResumeLayout(false);
            this.grpboxQuery.ResumeLayout(false);
            this.grpboxQuery.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.grpboxResults.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).EndInit();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitTopBottom;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton cmdRunQuery;
        private System.Windows.Forms.GroupBox grpboxQuery;
        private System.Windows.Forms.TextBox txtQuery;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.GroupBox grpboxResults;
        private System.Windows.Forms.DataGridView dgvResults;
        private System.Windows.Forms.ToolStripButton cmdConnectionSettings;
        private System.Windows.Forms.ToolStripStatusLabel lblOtherInfo;
        private System.Windows.Forms.ToolStripButton cmdClear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripComboBox cbxExamples;
        private System.Windows.Forms.ToolStripButton cmdClearResults;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripButton cmdAbout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton cmdLaunchExplorer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
    }
}

