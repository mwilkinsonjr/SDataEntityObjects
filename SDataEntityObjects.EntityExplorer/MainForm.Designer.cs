namespace SDataEntityObjects.EntityExplorer
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
            this.grpboxConnection = new System.Windows.Forms.GroupBox();
            this.chkEnableLINQ = new System.Windows.Forms.CheckBox();
            this.cbxInterface = new System.Windows.Forms.ComboBox();
            this.lblInterface = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.splitLeftRight = new System.Windows.Forms.SplitContainer();
            this.entityTreeView = new System.Windows.Forms.TreeView();
            this.dgvEntity = new System.Windows.Forms.DataGridView();
            this.lblBreadCrumb = new System.Windows.Forms.Label();
            this.splitTopBottom = new System.Windows.Forms.SplitContainer();
            this.txtLINQ = new System.Windows.Forms.TextBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblOtherInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.cmdAbout = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.cmdClearResults = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.cmdConnectionSettings = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.cmdLaunchSDataPad = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.grpboxConnection.SuspendLayout();
            this.splitLeftRight.Panel1.SuspendLayout();
            this.splitLeftRight.Panel2.SuspendLayout();
            this.splitLeftRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEntity)).BeginInit();
            this.splitTopBottom.Panel1.SuspendLayout();
            this.splitTopBottom.Panel2.SuspendLayout();
            this.splitTopBottom.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpboxConnection
            // 
            this.grpboxConnection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpboxConnection.Controls.Add(this.chkEnableLINQ);
            this.grpboxConnection.Controls.Add(this.cbxInterface);
            this.grpboxConnection.Controls.Add(this.lblInterface);
            this.grpboxConnection.Controls.Add(this.btnConnect);
            this.grpboxConnection.Location = new System.Drawing.Point(12, 28);
            this.grpboxConnection.Name = "grpboxConnection";
            this.grpboxConnection.Size = new System.Drawing.Size(700, 59);
            this.grpboxConnection.TabIndex = 1;
            this.grpboxConnection.TabStop = false;
            this.grpboxConnection.Text = "Connection Information";
            // 
            // chkEnableLINQ
            // 
            this.chkEnableLINQ.AutoSize = true;
            this.chkEnableLINQ.Checked = true;
            this.chkEnableLINQ.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnableLINQ.Location = new System.Drawing.Point(397, 25);
            this.chkEnableLINQ.Name = "chkEnableLINQ";
            this.chkEnableLINQ.Size = new System.Drawing.Size(130, 17);
            this.chkEnableLINQ.TabIndex = 15;
            this.chkEnableLINQ.Text = "Enable LINQ Complier";
            this.chkEnableLINQ.UseVisualStyleBackColor = true;
            this.chkEnableLINQ.CheckedChanged += new System.EventHandler(this.chkEnableLINQ_CheckedChanged);
            // 
            // cbxInterface
            // 
            this.cbxInterface.FormattingEnabled = true;
            this.cbxInterface.Location = new System.Drawing.Point(84, 23);
            this.cbxInterface.Name = "cbxInterface";
            this.cbxInterface.Size = new System.Drawing.Size(304, 21);
            this.cbxInterface.TabIndex = 12;
            this.cbxInterface.SelectedIndexChanged += new System.EventHandler(this.cbxInterface_SelectedIndexChanged);
            // 
            // lblInterface
            // 
            this.lblInterface.AutoSize = true;
            this.lblInterface.Location = new System.Drawing.Point(6, 26);
            this.lblInterface.Name = "lblInterface";
            this.lblInterface.Size = new System.Drawing.Size(52, 13);
            this.lblInterface.TabIndex = 11;
            this.lblInterface.Text = "Interface:";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(542, 21);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(136, 23);
            this.btnConnect.TabIndex = 10;
            this.btnConnect.Text = "Connect && Load";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // splitLeftRight
            // 
            this.splitLeftRight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitLeftRight.Location = new System.Drawing.Point(0, 21);
            this.splitLeftRight.Name = "splitLeftRight";
            // 
            // splitLeftRight.Panel1
            // 
            this.splitLeftRight.Panel1.Controls.Add(this.entityTreeView);
            // 
            // splitLeftRight.Panel2
            // 
            this.splitLeftRight.Panel2.Controls.Add(this.dgvEntity);
            this.splitLeftRight.Size = new System.Drawing.Size(700, 270);
            this.splitLeftRight.SplitterDistance = 350;
            this.splitLeftRight.TabIndex = 2;
            // 
            // entityTreeView
            // 
            this.entityTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.entityTreeView.Location = new System.Drawing.Point(0, 0);
            this.entityTreeView.Name = "entityTreeView";
            this.entityTreeView.Size = new System.Drawing.Size(350, 270);
            this.entityTreeView.TabIndex = 1;
            this.entityTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.entityTreeView_AfterSelect);
            // 
            // dgvEntity
            // 
            this.dgvEntity.AllowUserToAddRows = false;
            this.dgvEntity.AllowUserToDeleteRows = false;
            this.dgvEntity.AllowUserToOrderColumns = true;
            this.dgvEntity.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvEntity.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            this.dgvEntity.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEntity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEntity.Location = new System.Drawing.Point(0, 0);
            this.dgvEntity.Name = "dgvEntity";
            this.dgvEntity.ReadOnly = true;
            this.dgvEntity.Size = new System.Drawing.Size(346, 270);
            this.dgvEntity.TabIndex = 0;
            // 
            // lblBreadCrumb
            // 
            this.lblBreadCrumb.Location = new System.Drawing.Point(3, 0);
            this.lblBreadCrumb.Name = "lblBreadCrumb";
            this.lblBreadCrumb.Size = new System.Drawing.Size(694, 18);
            this.lblBreadCrumb.TabIndex = 3;
            // 
            // splitTopBottom
            // 
            this.splitTopBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitTopBottom.Location = new System.Drawing.Point(12, 93);
            this.splitTopBottom.Name = "splitTopBottom";
            this.splitTopBottom.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitTopBottom.Panel1
            // 
            this.splitTopBottom.Panel1.Controls.Add(this.txtLINQ);
            // 
            // splitTopBottom.Panel2
            // 
            this.splitTopBottom.Panel2.Controls.Add(this.splitLeftRight);
            this.splitTopBottom.Panel2.Controls.Add(this.lblBreadCrumb);
            this.splitTopBottom.Size = new System.Drawing.Size(700, 357);
            this.splitTopBottom.SplitterDistance = 47;
            this.splitTopBottom.TabIndex = 4;
            // 
            // txtLINQ
            // 
            this.txtLINQ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLINQ.Location = new System.Drawing.Point(0, 0);
            this.txtLINQ.Multiline = true;
            this.txtLINQ.Name = "txtLINQ";
            this.txtLINQ.Size = new System.Drawing.Size(700, 47);
            this.txtLINQ.TabIndex = 0;
            this.txtLINQ.Text = "from a in IAccount\r\nwhere a.AccountName.StartsWith(\"C\")\r\norderby a.AccountName se" +
                "lect a";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.lblOtherInfo});
            this.statusStrip.Location = new System.Drawing.Point(0, 440);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(728, 22);
            this.statusStrip.TabIndex = 5;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblStatus.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(613, 17);
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
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdAbout,
            this.toolStripSeparator4,
            this.cmdClearResults,
            this.toolStripSeparator5,
            this.cmdConnectionSettings,
            this.toolStripSeparator6,
            this.cmdLaunchSDataPad,
            this.toolStripSeparator1});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(728, 25);
            this.toolStrip.TabIndex = 6;
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
            // cmdLaunchSDataPad
            // 
            this.cmdLaunchSDataPad.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cmdLaunchSDataPad.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cmdLaunchSDataPad.Image = ((System.Drawing.Image)(resources.GetObject("cmdLaunchSDataPad.Image")));
            this.cmdLaunchSDataPad.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdLaunchSDataPad.Name = "cmdLaunchSDataPad";
            this.cmdLaunchSDataPad.Size = new System.Drawing.Size(95, 22);
            this.cmdLaunchSDataPad.Text = "Launch SDataPad";
            this.cmdLaunchSDataPad.Click += new System.EventHandler(this.cmdLaunchSDataPad_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 462);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.splitTopBottom);
            this.Controls.Add(this.grpboxConnection);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "SData Entity Explorer";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.grpboxConnection.ResumeLayout(false);
            this.grpboxConnection.PerformLayout();
            this.splitLeftRight.Panel1.ResumeLayout(false);
            this.splitLeftRight.Panel2.ResumeLayout(false);
            this.splitLeftRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEntity)).EndInit();
            this.splitTopBottom.Panel1.ResumeLayout(false);
            this.splitTopBottom.Panel1.PerformLayout();
            this.splitTopBottom.Panel2.ResumeLayout(false);
            this.splitTopBottom.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpboxConnection;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ComboBox cbxInterface;
        private System.Windows.Forms.Label lblInterface;
        private System.Windows.Forms.SplitContainer splitLeftRight;
        private System.Windows.Forms.TreeView entityTreeView;
        private System.Windows.Forms.DataGridView dgvEntity;
        private System.Windows.Forms.Label lblBreadCrumb;
        private System.Windows.Forms.SplitContainer splitTopBottom;
        private System.Windows.Forms.TextBox txtLINQ;
        private System.Windows.Forms.CheckBox chkEnableLINQ;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblOtherInfo;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton cmdAbout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton cmdClearResults;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton cmdConnectionSettings;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton cmdLaunchSDataPad;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}

