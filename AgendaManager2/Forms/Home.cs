using AgendaLibrary.Types;
using AgendaManager2.Forms;
using MongoDB.Driver;
using System.Net;

namespace AgendaManager2
{
    public partial class Home : Form
    {
        public MongoClient client;
        public static IMongoDatabase database;
        private static string connectionString = "mongodb+srv://homework-database:homework123@cluster0.ygoqv7l.mongodb.net/";
        public Thread initThread;

        public Home()
        {
            initThread = new Thread(InitializeDatabase);
            InitializeComponent();
            initThread.Start();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Bounds.Contains(e.Location))
                {
                    //MessageBox.Show(item.Text);
                    switch (item.Text)
                    {
                        case "Upload":
                            Upload uploadWindow = new Upload();
                            uploadWindow.ShowDialog(); // prevent switching focus back to main program
                            break;
                        case "Receive":
                            Receive receiveWindow = new Receive();
                            receiveWindow.ShowDialog(); // prevent switching focus back to main program
                            break;
                        case "Prune":
                            Prune pruneWindow = new Prune();
                            pruneWindow.ShowDialog(); // prevent switching focus back to main program
                            break;
                        case "Settings":
                            Settings settingsWindow = new Settings();
                            settingsWindow.ShowDialog(); // prevent switching focus back to main program
                            break;
                    }
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About aboutWindow = new About();
            aboutWindow.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Database not initalized.";
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            initThread.Join();
        }

        internal void InitializeDatabase()
        {
            toolStripStatusLabel1.Text = "Initializing database.....";
            MongoClientSettings settings = MongoClientSettings.FromConnectionString(connectionString);
            settings.IPv6 = false;
            settings.ConnectTimeout = TimeSpan.FromSeconds(5);
            MongoClient client = new MongoClient(settings);
            database = client.GetDatabase("homework-database");
            // set complete text
            toolStripStatusLabel1.Text = "Database initialized.";
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Update updateWindow = new Update();
            updateWindow.ShowDialog();
        }
    }
}