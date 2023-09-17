using AgendaLibrary;
using System.Reflection;

namespace AgendaManager2
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void About_Load(object sender, EventArgs e)
        {
            label3.Text = $"v{Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString()}";
            label4.Text = $"v{ExposeVersioning.LibraryVersion()}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
