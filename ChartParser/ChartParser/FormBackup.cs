using Common;
using Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ChartParser
{
    public partial class FormBackup : Form
    {
        public FormBackup()
        {
            InitializeComponent();
        }

        private readonly string _activeRoot = Constant.ITunesMusicDirectory;
        private readonly string _backupRoot = Constant.BackupMusicDirectory;

        private readonly List<FileCompareLine> _fileCompareLines = new List<FileCompareLine>();

        private void FormBackup_Shown(object sender, EventArgs e)
        {
            //int count1 = 0;
            //GetFileCount(activeRoot, true, ref count1);

            //int count2 = 0;
            //GetFileCount(backupRoot, false, ref count2);

            //this.dataGridViewFileCompare.DataSource = null;
            //this.dataGridViewFileCompare.DataSource = fileCompareLines;
        }

        private void GetFileCount(string dir, bool checkBackup, ref int count)
        {
            string[] files = Directory.GetFiles(dir);

            if (checkBackup)
            {
                foreach (string file in files)
                {
                    string backupFileName = file.Replace(_activeRoot, _backupRoot);
                    if (!File.Exists(backupFileName))
                    {
                        _fileCompareLines.Add(new FileCompareLine
                        {
                            ActiveFileName = file,
                            BackupFileName = backupFileName,
                            BackupMissing = true
                        });
                    }
                }
            }
            else
            {
                foreach (string file in files)
                {
                    string activeFileName = file.Replace(_backupRoot, _activeRoot);
                    if (!File.Exists(activeFileName))
                    {
                        _fileCompareLines.Add(new FileCompareLine
                        {
                            ActiveFileName = activeFileName,
                            BackupFileName = file,
                            ActiveMissing = true
                        });
                    }
                }
            }

            count += files.Length;

            string[] dirs = Directory.GetDirectories(dir);

            if (dirs.Length == 0)
                return;
            else
            {
                foreach (string d in dirs)
                {
                    GetFileCount(d, checkBackup, ref count);
                }
                return;
            }
        }

        private void dataGridViewFileCompare_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int colIndex = e.ColumnIndex;

            if (rowIndex >= 0 && colIndex >= 0)
            {
                DataGridViewRow theRow = this.dataGridViewFileCompare.Rows[rowIndex];

                var fileCompareLine = (FileCompareLine)theRow.DataBoundItem;

                if (fileCompareLine == null)
                    return;

                if (fileCompareLine.BackupMissing)
                {
                    if (colIndex == 0)
                        e.CellStyle.ForeColor = Color.Red;
                }

                if (fileCompareLine.ActiveMissing)
                {
                    if (colIndex == 1)
                        e.CellStyle.ForeColor = Color.Red;
                }
            }
        }

        private void toolStripButtonSync_Click(object sender, EventArgs e)
        {
            var fileCompareSelectedLines = new List<FileCompareLine>();

            if (this.dataGridViewFileCompare.SelectedRows.Count != 0)
            {
                for (var idx = 0; idx < this.dataGridViewFileCompare.SelectedRows.Count; idx++)
                {
                    fileCompareSelectedLines.Add((FileCompareLine)this.dataGridViewFileCompare.SelectedRows[idx].DataBoundItem);
                }
            }

            foreach (FileCompareLine line in fileCompareSelectedLines)
            {
                if (line.BackupMissing)
                {
                    string directoryName = Path.GetDirectoryName(line.BackupFileName);
                    Directory.CreateDirectory(directoryName);
                    File.Copy(line.ActiveFileName, line.BackupFileName);
                    _fileCompareLines.Remove(line);
                }

                if (line.ActiveMissing)
                {
                    File.Delete(line.BackupFileName);
                    string directoryName = Path.GetDirectoryName(line.BackupFileName);
                    if (Directory.GetFiles(directoryName).Length == 0)
                        Directory.Delete(directoryName);
                    _fileCompareLines.Remove(line);
                }
            }

            this.dataGridViewFileCompare.DataSource = null;
            this.dataGridViewFileCompare.DataSource = _fileCompareLines;
        }

        private void toolStripButtonCopy_Click(object sender, EventArgs e)
        {
            var fileCompareSelectedLines = new List<FileCompareLine>();

            if (this.dataGridViewFileCompare.SelectedRows.Count != 0)
            {
                for (var idx = 0; idx < this.dataGridViewFileCompare.SelectedRows.Count; idx++)
                {
                    fileCompareSelectedLines.Add((FileCompareLine)this.dataGridViewFileCompare.SelectedRows[idx].DataBoundItem);
                }
            }


            //string dest = "C:\\music\\";
            //Directory.CreateDirectory(dest);


            foreach (FileCompareLine line in fileCompareSelectedLines)
            {
                if (line.ActiveMissing)
                {
                    string directoryName = Path.GetDirectoryName(line.ActiveFileName);
                    Directory.CreateDirectory(directoryName);
                    File.Copy(line.BackupFileName, line.ActiveFileName, true);

                    //string dfile = Path.GetFileName(line.BackupFileName);
                    //File.Copy(line.BackupFileName, Path.Combine(dest, dfile), true);
                }
            }

            this.dataGridViewFileCompare.DataSource = null;
            this.dataGridViewFileCompare.DataSource = _fileCompareLines;
        }

        private void FormBackup_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
