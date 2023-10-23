namespace ChartParser
{
    partial class FormBackup
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBackup));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSync = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonCopy = new System.Windows.Forms.ToolStripButton();
            this.dataGridViewFileCompare = new System.Windows.Forms.DataGridView();
            this.backupFileNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.activeFileNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fileCompareLineBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFileCompare)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileCompareLineBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSync,
            this.toolStripSeparator1,
            this.toolStripButtonCopy});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1301, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripButtonSync
            // 
            this.toolStripButtonSync.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSync.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSync.Image")));
            this.toolStripButtonSync.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSync.Name = "toolStripButtonSync";
            this.toolStripButtonSync.Size = new System.Drawing.Size(36, 22);
            this.toolStripButtonSync.Text = "Sync";
            this.toolStripButtonSync.Click += new System.EventHandler(this.toolStripButtonSync_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonCopy
            // 
            this.toolStripButtonCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonCopy.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCopy.Image")));
            this.toolStripButtonCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCopy.Name = "toolStripButtonCopy";
            this.toolStripButtonCopy.Size = new System.Drawing.Size(39, 22);
            this.toolStripButtonCopy.Text = "Copy";
            this.toolStripButtonCopy.Click += new System.EventHandler(this.toolStripButtonCopy_Click);
            // 
            // dataGridViewFileCompare
            // 
            this.dataGridViewFileCompare.AutoGenerateColumns = false;
            this.dataGridViewFileCompare.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridViewFileCompare.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewFileCompare.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.backupFileNameDataGridViewTextBoxColumn,
            this.activeFileNameDataGridViewTextBoxColumn});
            this.dataGridViewFileCompare.DataSource = this.fileCompareLineBindingSource;
            this.dataGridViewFileCompare.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewFileCompare.Location = new System.Drawing.Point(0, 25);
            this.dataGridViewFileCompare.Name = "dataGridViewFileCompare";
            this.dataGridViewFileCompare.Size = new System.Drawing.Size(1301, 632);
            this.dataGridViewFileCompare.TabIndex = 1;
            this.dataGridViewFileCompare.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridViewFileCompare_CellFormatting);
            // 
            // backupFileNameDataGridViewTextBoxColumn
            // 
            this.backupFileNameDataGridViewTextBoxColumn.DataPropertyName = "BackupFileName";
            this.backupFileNameDataGridViewTextBoxColumn.HeaderText = "BackupFileName";
            this.backupFileNameDataGridViewTextBoxColumn.Name = "backupFileNameDataGridViewTextBoxColumn";
            this.backupFileNameDataGridViewTextBoxColumn.Width = 113;
            // 
            // activeFileNameDataGridViewTextBoxColumn
            // 
            this.activeFileNameDataGridViewTextBoxColumn.DataPropertyName = "ActiveFileName";
            this.activeFileNameDataGridViewTextBoxColumn.HeaderText = "ActiveFileName";
            this.activeFileNameDataGridViewTextBoxColumn.Name = "activeFileNameDataGridViewTextBoxColumn";
            this.activeFileNameDataGridViewTextBoxColumn.Width = 106;
            // 
            // fileCompareLineBindingSource
            // 
            this.fileCompareLineBindingSource.DataSource = typeof(Helper.FileCompareLine);
            // 
            // FormBackup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1301, 657);
            this.Controls.Add(this.dataGridViewFileCompare);
            this.Controls.Add(this.toolStrip);
            this.Name = "FormBackup";
            this.Text = "Backup";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormBackup_FormClosing);
            this.Shown += new System.EventHandler(this.FormBackup_Shown);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFileCompare)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileCompareLineBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.DataGridView dataGridViewFileCompare;
        private System.Windows.Forms.DataGridViewTextBoxColumn backupFileNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn activeFileNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource fileCompareLineBindingSource;
        private System.Windows.Forms.ToolStripButton toolStripButtonSync;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonCopy;
    }
}