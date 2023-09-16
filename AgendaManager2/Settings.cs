using AgendaLibrary.Definitions;
using AgendaLibrary.Libraries;
using System;
using System.Windows.Forms;

namespace AgendaManager2
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = (int)LanguageLibrary.CultureToLP(Properties.Settings.Default.language);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.language = LanguageLibrary.LPToCulture((LanguagePreference)comboBox1.SelectedIndex);
            Properties.Settings.Default.Save();
        }
    }
}
