namespace ChartParser
{
    partial class FormYoutubeParseTest
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripTextBox2 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripTextBox3 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripTextBox4 = new System.Windows.Forms.ToolStripTextBox();
            this.dataGridViewYoutubeResults = new System.Windows.Forms.DataGridView();
            this.ordinalDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.videoIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.thumbnailUrlDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.thumbnailDataGridViewImageColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.titleDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.artistDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.featDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.songVersionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.videoVersionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.officialDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isMixDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isOfficialDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.channelIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.channelTitleDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.youtubeTrackBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewYoutubeResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.youtubeTrackBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox1,
            this.toolStripSeparator1,
            this.toolStripTextBox2,
            this.toolStripSeparator2,
            this.toolStripTextBox3,
            this.toolStripSeparator3,
            this.toolStripTextBox4});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1301, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(200, 25);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripTextBox2
            // 
            this.toolStripTextBox2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripTextBox2.Name = "toolStripTextBox2";
            this.toolStripTextBox2.Size = new System.Drawing.Size(200, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripTextBox3
            // 
            this.toolStripTextBox3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripTextBox3.Name = "toolStripTextBox3";
            this.toolStripTextBox3.Size = new System.Drawing.Size(200, 25);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripTextBox4
            // 
            this.toolStripTextBox4.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripTextBox4.Name = "toolStripTextBox4";
            this.toolStripTextBox4.Size = new System.Drawing.Size(500, 25);
            this.toolStripTextBox4.KeyUp += new System.Windows.Forms.KeyEventHandler(this.toolStripTextBox4_KeyUp);
            // 
            // dataGridViewYoutubeResults
            // 
            this.dataGridViewYoutubeResults.AutoGenerateColumns = false;
            this.dataGridViewYoutubeResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridViewYoutubeResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewYoutubeResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ordinalDataGridViewTextBoxColumn,
            this.videoIdDataGridViewTextBoxColumn,
            this.thumbnailUrlDataGridViewTextBoxColumn,
            this.thumbnailDataGridViewImageColumn,
            this.titleDataGridViewTextBoxColumn,
            this.artistDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.featDataGridViewTextBoxColumn,
            this.songVersionDataGridViewTextBoxColumn,
            this.videoVersionDataGridViewTextBoxColumn,
            this.officialDataGridViewTextBoxColumn,
            this.isMixDataGridViewCheckBoxColumn,
            this.isOfficialDataGridViewCheckBoxColumn,
            this.channelIdDataGridViewTextBoxColumn,
            this.channelTitleDataGridViewTextBoxColumn});
            this.dataGridViewYoutubeResults.DataSource = this.youtubeTrackBindingSource;
            this.dataGridViewYoutubeResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewYoutubeResults.Location = new System.Drawing.Point(0, 25);
            this.dataGridViewYoutubeResults.Name = "dataGridViewYoutubeResults";
            this.dataGridViewYoutubeResults.Size = new System.Drawing.Size(1301, 632);
            this.dataGridViewYoutubeResults.TabIndex = 1;
            // 
            // ordinalDataGridViewTextBoxColumn
            // 
            this.ordinalDataGridViewTextBoxColumn.DataPropertyName = "Ordinal";
            this.ordinalDataGridViewTextBoxColumn.HeaderText = "Ordinal";
            this.ordinalDataGridViewTextBoxColumn.Name = "ordinalDataGridViewTextBoxColumn";
            this.ordinalDataGridViewTextBoxColumn.Width = 65;
            // 
            // videoIdDataGridViewTextBoxColumn
            // 
            this.videoIdDataGridViewTextBoxColumn.DataPropertyName = "VideoId";
            this.videoIdDataGridViewTextBoxColumn.HeaderText = "VideoId";
            this.videoIdDataGridViewTextBoxColumn.Name = "videoIdDataGridViewTextBoxColumn";
            this.videoIdDataGridViewTextBoxColumn.Width = 68;
            // 
            // thumbnailUrlDataGridViewTextBoxColumn
            // 
            this.thumbnailUrlDataGridViewTextBoxColumn.DataPropertyName = "ThumbnailUrl";
            this.thumbnailUrlDataGridViewTextBoxColumn.HeaderText = "ThumbnailUrl";
            this.thumbnailUrlDataGridViewTextBoxColumn.Name = "thumbnailUrlDataGridViewTextBoxColumn";
            this.thumbnailUrlDataGridViewTextBoxColumn.Width = 94;
            // 
            // thumbnailDataGridViewImageColumn
            // 
            this.thumbnailDataGridViewImageColumn.DataPropertyName = "Thumbnail";
            this.thumbnailDataGridViewImageColumn.HeaderText = "Thumbnail";
            this.thumbnailDataGridViewImageColumn.Name = "thumbnailDataGridViewImageColumn";
            this.thumbnailDataGridViewImageColumn.Width = 62;
            // 
            // titleDataGridViewTextBoxColumn
            // 
            this.titleDataGridViewTextBoxColumn.DataPropertyName = "Title";
            this.titleDataGridViewTextBoxColumn.HeaderText = "Title";
            this.titleDataGridViewTextBoxColumn.Name = "titleDataGridViewTextBoxColumn";
            this.titleDataGridViewTextBoxColumn.Width = 52;
            // 
            // artistDataGridViewTextBoxColumn
            // 
            this.artistDataGridViewTextBoxColumn.DataPropertyName = "Artist";
            this.artistDataGridViewTextBoxColumn.HeaderText = "Artist";
            this.artistDataGridViewTextBoxColumn.Name = "artistDataGridViewTextBoxColumn";
            this.artistDataGridViewTextBoxColumn.Width = 55;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.Width = 60;
            // 
            // featDataGridViewTextBoxColumn
            // 
            this.featDataGridViewTextBoxColumn.DataPropertyName = "Feat";
            this.featDataGridViewTextBoxColumn.HeaderText = "Feat";
            this.featDataGridViewTextBoxColumn.Name = "featDataGridViewTextBoxColumn";
            this.featDataGridViewTextBoxColumn.Width = 53;
            // 
            // songVersionDataGridViewTextBoxColumn
            // 
            this.songVersionDataGridViewTextBoxColumn.DataPropertyName = "SongVersion";
            this.songVersionDataGridViewTextBoxColumn.HeaderText = "SongVersion";
            this.songVersionDataGridViewTextBoxColumn.Name = "songVersionDataGridViewTextBoxColumn";
            this.songVersionDataGridViewTextBoxColumn.Width = 92;
            // 
            // videoVersionDataGridViewTextBoxColumn
            // 
            this.videoVersionDataGridViewTextBoxColumn.DataPropertyName = "VideoVersion";
            this.videoVersionDataGridViewTextBoxColumn.HeaderText = "VideoVersion";
            this.videoVersionDataGridViewTextBoxColumn.Name = "videoVersionDataGridViewTextBoxColumn";
            this.videoVersionDataGridViewTextBoxColumn.Width = 94;
            // 
            // officialDataGridViewTextBoxColumn
            // 
            this.officialDataGridViewTextBoxColumn.DataPropertyName = "Official";
            this.officialDataGridViewTextBoxColumn.HeaderText = "Official";
            this.officialDataGridViewTextBoxColumn.Name = "officialDataGridViewTextBoxColumn";
            this.officialDataGridViewTextBoxColumn.Width = 64;
            // 
            // isMixDataGridViewCheckBoxColumn
            // 
            this.isMixDataGridViewCheckBoxColumn.DataPropertyName = "IsMix";
            this.isMixDataGridViewCheckBoxColumn.HeaderText = "IsMix";
            this.isMixDataGridViewCheckBoxColumn.Name = "isMixDataGridViewCheckBoxColumn";
            this.isMixDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isMixDataGridViewCheckBoxColumn.Width = 37;
            // 
            // isOfficialDataGridViewCheckBoxColumn
            // 
            this.isOfficialDataGridViewCheckBoxColumn.DataPropertyName = "IsOfficial";
            this.isOfficialDataGridViewCheckBoxColumn.HeaderText = "IsOfficial";
            this.isOfficialDataGridViewCheckBoxColumn.Name = "isOfficialDataGridViewCheckBoxColumn";
            this.isOfficialDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isOfficialDataGridViewCheckBoxColumn.Width = 53;
            // 
            // channelIdDataGridViewTextBoxColumn
            // 
            this.channelIdDataGridViewTextBoxColumn.DataPropertyName = "ChannelId";
            this.channelIdDataGridViewTextBoxColumn.HeaderText = "ChannelId";
            this.channelIdDataGridViewTextBoxColumn.Name = "channelIdDataGridViewTextBoxColumn";
            this.channelIdDataGridViewTextBoxColumn.Width = 80;
            // 
            // channelTitleDataGridViewTextBoxColumn
            // 
            this.channelTitleDataGridViewTextBoxColumn.DataPropertyName = "ChannelTitle";
            this.channelTitleDataGridViewTextBoxColumn.HeaderText = "ChannelTitle";
            this.channelTitleDataGridViewTextBoxColumn.Name = "channelTitleDataGridViewTextBoxColumn";
            this.channelTitleDataGridViewTextBoxColumn.Width = 91;
            // 
            // youtubeTrackBindingSource
            // 
            this.youtubeTrackBindingSource.DataSource = typeof(Helper.YoutubeTrack);
            // 
            // FormYoutubeParseTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1301, 657);
            this.Controls.Add(this.dataGridViewYoutubeResults);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FormYoutubeParseTest";
            this.Text = "YoutubeParseTest";
            this.Load += new System.EventHandler(this.FormYoutubeParseTest_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewYoutubeResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.youtubeTrackBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.DataGridView dataGridViewYoutubeResults;
        private System.Windows.Forms.DataGridViewTextBoxColumn ordinalDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn videoIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn thumbnailUrlDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewImageColumn thumbnailDataGridViewImageColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn titleDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn artistDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn featDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn songVersionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn videoVersionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn officialDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isMixDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isOfficialDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn channelIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn channelTitleDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource youtubeTrackBindingSource;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox4;
    }
}