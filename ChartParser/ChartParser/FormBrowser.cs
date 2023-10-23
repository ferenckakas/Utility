using System.Windows.Forms;

namespace ChartParser
{
    public partial class FormBrowser : Form
    {
        public FormBrowser(string url)
        {
            InitializeComponent();

            this.webBrowser.Navigate(url);
        }
    }
}
