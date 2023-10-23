using System;
using System.Windows.Forms;

namespace ChartParser
{
    public partial class FormMain : Form
    {
        private readonly FormConsole _formConsole;
        private FormChartParser _formChartParser;
        private FormLibrary _formLibrary;
        //private readonly FormYoutubeParseTest _formYoutubeParseTest = new FormYoutubeParseTest();
        private FormBackup _formBackup;

        //private readonly bool _test = false;

        public FormMain()
        {
            InitializeComponent();

            //_formYoutubeParseTest.MdiParent = this;

            //if (_test)
            //    _formYoutubeParseTest.Show();
            //else

            _formConsole = new FormConsole();
            _formConsole.MdiParent = this;
            _formConsole.Show();
        }

        private void toolStripButtonLibrary_Click(object sender, EventArgs e)
        {
            if (_formLibrary == null)
            {
                _formLibrary = new FormLibrary();
                _formLibrary.MdiParent = this;
            }

            _formLibrary.Show();
            _formLibrary.WindowState = FormWindowState.Maximized;
        }

        private void toolStripButtonChart_Click(object sender, EventArgs e)
        {
            if (_formChartParser == null)
            {
                _formChartParser = new FormChartParser();
                _formChartParser.MdiParent = this;
            }

            _formChartParser.Show();
            _formChartParser.WindowState = FormWindowState.Maximized;
        }

        private void toolStripButtonBackup_Click(object sender, EventArgs e)
        {
            if (_formBackup == null)
            {
                _formBackup = new FormBackup();
                _formBackup.MdiParent = this;
            }

            _formBackup.Show();
            _formBackup.WindowState = FormWindowState.Maximized;
        }
    }
}
