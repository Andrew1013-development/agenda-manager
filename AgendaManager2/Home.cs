using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgendaManager2
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void Home_Load(object sender, EventArgs e)
        {

        }

        private void ListView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Bounds.Contains(e.Location))
                {
                    MessageBox.Show(item.Text);
                    /*
                    switch (item.Text)
                    {
                        case "Upload":
                            MessageBox.Show("he clicked on upload");
                            break;
                        case "Receive":
                            MessageBox.Show("he clicked on receive");
                            break;
                        case "Prune":
                            MessageBox.Show("he clicked on prune");
                            break;
                    }
                    */
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About aboutWindow = new About();
            aboutWindow.Show();
        }
    }
}
