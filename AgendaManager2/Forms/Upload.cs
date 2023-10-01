using AgendaLibrary.Libraries;
using AgendaLibrary.Types;
using MongoDB.Driver;
using System.Windows.Forms;

namespace AgendaManager2.Forms
{
    public partial class Upload : Form
    {
        private IMongoCollection<Agenda> agenda_collection;
        private IMongoCollection<Bug> bug_collection;
        private IMongoCollection<Telemetry> telemetry_collection;
        private Thread initThread;

        public Upload()
        {
            initThread = new Thread(InitializeCollection);
            InitializeComponent();
            initThread.Start();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value <= DateTime.Now)
            {
                MessageBox.Show("Date cannot be earlier than today", "Date error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox3.Text) && !String.IsNullOrEmpty(textBox1.Text))
            {
                Agenda newAgenda = new Agenda(textBox3.Text, dateTimePicker1.Value.ToShortDateString(), textBox1.Text, textBox2.Text);
                UploadLibraryGraphics.UploadAgenda(newAgenda, agenda_collection);
                MessageBox.Show("Agenda uploaded successfully", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } 
            else
            {
                if (String.IsNullOrEmpty(textBox3.Text))
                {
                    MessageBox.Show("Subject field cannot be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } 
                else if (String.IsNullOrEmpty(textBox1.Text))
                {
                    MessageBox.Show("Content field cannot be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }   
        }

        private void Upload_Load(object sender, EventArgs e)
        {
            while (initThread.ThreadState == ThreadState.Running)
            {
                button1.Enabled = false;
                if (initThread.ThreadState != ThreadState.Running)
                {
                    button1.Enabled = true;
                }
            }
            
        }

        internal void InitializeCollection()
        {
            IMongoDatabase database = Home.database;
            agenda_collection = database.GetCollection<Agenda>("agenda");
            bug_collection = database.GetCollection<Bug>("bug");
            telemetry_collection = database.GetCollection<Telemetry>("telemetry");
        }
    }
}
