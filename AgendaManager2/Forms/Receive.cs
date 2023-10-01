using AgendaLibrary.Types;
using AgendaLibrary.Libraries;
using MongoDB.Driver;

namespace AgendaManager2.Forms
{
    public partial class Receive : Form
    {
        private IMongoCollection<Agenda> agenda_collection = Home.database.GetCollection<Agenda>("agenda");
        private List<Agenda> agenda_search;

        public Receive()
        {
            InitializeComponent();
        }

        private void Receive_Load(object sender, EventArgs e)
        {
            agenda_search = ReceiveLibraryGraphics.ReceiveAgenda(agenda_collection);
            agenda_search.Sort();
            label5.Text += $" {agenda_search.Count}";
            string prev_deadline = "1/1/1970"; // only for the first comparison
            int current_parent_node = 0;
            foreach (Agenda agenda in agenda_search)
            {
                if (agenda.deadline != prev_deadline)
                {
                    treeView1.Nodes.Add(agenda.deadline.ToString());
                    treeView1.SelectedNode = treeView1.Nodes[current_parent_node]; // update selected parent node
                    prev_deadline = agenda.deadline;
                    current_parent_node++; // update index for next node insertion
                }
                treeView1.SelectedNode.Nodes.Add(agenda.subject);
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Parent != null)
            {
                Tuple<bool, Agenda> tmp = searchAgenda(e.Node.Text.ToString(), e.Node.Parent.Text, agenda_search);
                label1.Text = $"Subject: {tmp.Item2.subject}";
                label2.Text = $"Deadline: {tmp.Item2.deadline}";
                label3.Text = $"Content:\n{tmp.Item2.content}";
                label4.Text = $"Notes:\n{tmp.Item2.notes}";
            }
        }

        internal Tuple<bool, Agenda> searchAgenda(string subject_search, string deadline_search, List<Agenda> agenda_list)
        {
            foreach (Agenda agenda_search in agenda_list)
            {
                if (subject_search == agenda_search.subject && deadline_search == agenda_search.deadline)
                {
                    return Tuple.Create(true, agenda_search);
                }
            }
            // guaranteed to find a element but add this to not get error
            return Tuple.Create(false, new Agenda("", "", "", ""));
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
