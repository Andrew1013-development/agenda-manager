using AgendaLibrary.Libraries;
using Octokit;
using System.Reflection;

namespace AgendaManager2.Forms
{
    public partial class Update : Form
    {
        public Update()
        {
            InitializeComponent();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void Update_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Checking for updates.....";
            button1.Enabled = false;
            button2.Enabled = false;

            // check for latest release
            Tuple<short, Release> latest = await UpdateLibraryGraphics.ReturnLatestUpdate(Assembly.GetExecutingAssembly().GetName().Version.ToString());
            switch (latest.Item1)
            {
                case 0:
                    label9.Text = "Your version is up-to-date.";
                    toolStripStatusLabel1.Text = "Up-to-date.";
                    toolStripProgressBar1.Value = 3;
                    break;
                case 1:
                    label9.Text = "";
                    button1.Enabled = true;
                    button2.Enabled = true;
                    toolStripStatusLabel1.Text = "Waiting for user input.....";
                    toolStripProgressBar1.Value += 1;
                    break;
                case 2:
                    label9.Text = "Unreleased version detected.";
                    toolStripStatusLabel1.Text = "Unreleased version";
                    toolStripProgressBar1.Value = 3;
                    break;
            }
            // display update information
            label4.Text = latest.Item2.TagName;
            label5.Text = TimeZoneInfo.ConvertTime(latest.Item2.PublishedAt.Value.UtcDateTime, TimeZoneInfo.Local).ToString(); //convert UTC time to local time
            textBox1.Text = latest.Item2.Body;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Downloading updater.....";
            
        }
    }
}
