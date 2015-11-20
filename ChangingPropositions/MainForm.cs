using System;
using System.Windows.Forms;

namespace ChangingPropositions
{
    public partial class MainForm : Form
    {
        public static MainForm DisplayForm;

        public MainForm()
        {
            InitializeComponent();
            DisplayForm = this;
        }

        private void setInputData(Proposition input)
        {
            dgvInput.Rows.Clear();

            string sprop = (!string.IsNullOrEmpty(input.twi) ? input.twi + " " : "");
            sprop += input.nonPredicate ? "non-" : "";
            sprop += input.Predicate;

            dgvInput.Rows.Add("Type", input.Type.ToString());
            dgvInput.Rows.Add("Quantity", (input.Quantity == Quantity.Some ? "Particular" : "Universal") + " (" + input.Quantity.ToString() + ")");
            dgvInput.Rows.Add("Quality", input.Quality ? "Positive" : "Negitive");
            dgvInput.Rows.Add("Inverted Predicate", input.nonPredicate ? "Yes (\"non-\")" : "No");
            dgvInput.Rows.Add("Subject", input.Subject);
            dgvInput.Rows.Add("Predicate", sprop);
        }

        private void setOutputData(Proposition output)
        {
            dgvOutput.Rows.Clear();

            string sprop = (!string.IsNullOrEmpty(output.twi) ? output.twi + " " : "");
            sprop += output.nonPredicate ? "non-" : "";
            sprop += output.Predicate;

            dgvOutput.Rows.Add("Type", output.Type.ToString());
            dgvOutput.Rows.Add("Quantity", (output.Quantity == Quantity.Some ? "Particular" : "Universal") + " (" + output.Quantity.ToString() + ")");
            dgvOutput.Rows.Add("Quality", output.Quality ? "Positive" : "Negitive");
            dgvOutput.Rows.Add("Inverted Predicate", output.nonPredicate ? "Yes (\"non-\")" : "No");
            dgvOutput.Rows.Add("Subject", output.Subject);
            dgvOutput.Rows.Add("Predicate", sprop);
        }

        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            ProcessAndGetIncoming();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Examples:\nAll [goofy people] are [tw are non-serious]\nSome [beagles] are not [tw is cute]\nNo [ducks] are [geese]");
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            Proposition Incoming = ProcessAndGetIncoming();
            try
            {
                Proposition Outgoing = Incoming.Convert();
                txtOutput.Text = Outgoing.ToString();
                setOutputData(Outgoing);
            }
            catch (Exception E)
            {
                MessageBox.Show("Can't Convert a " + Incoming.Type.ToString() + " proposition!");
            }
        }

        private Proposition ProcessAndGetIncoming()
        {
            Proposition Incoming = new Proposition();
            try
            {
                Incoming = Proposition.Parse(txtInput.Text);
                txtOutput.Text = Incoming.ToString();
                setInputData(Incoming);
            }
            catch (FormatException E)
            {
                MessageBox.Show(E.Message, "Error");
            }
            listBox1.Items.Clear();
            listBox1.Items.Add(Incoming.ToString());
            return Incoming;
        }

        private void btnObvert_Click(object sender, EventArgs e)
        {
            Proposition Incoming = ProcessAndGetIncoming();

            try
            {
                Proposition Outgoing = Incoming.Obvert();
                txtOutput.Text = Outgoing.ToString();
                setOutputData(Outgoing);
            }
            catch (Exception E)
            {
                MessageBox.Show("Can't Obvert a " + Incoming.Type.ToString() + " proposition!");
            }
        }

        private void btnContrapose_Click(object sender, EventArgs e)
        {
            Proposition Incoming = ProcessAndGetIncoming();

            try
            {
                Proposition Outgoing = Incoming.Contrapose();
                txtOutput.Text = Outgoing.ToString();
                setOutputData(Outgoing);
            }
            catch (Exception E)
            {
                MessageBox.Show("Can't Contrapose a " + Incoming.Type.ToString() + " proposition!");
            }
        }

        private void btnPartialContrapose_Click(object sender, EventArgs e)
        {
            Proposition Incoming = ProcessAndGetIncoming();

            try
            {
                Proposition Outgoing = Incoming.PartialContrapose();
                txtOutput.Text = Outgoing.ToString();
                setOutputData(Outgoing);
            }
            catch (Exception E)
            {
                MessageBox.Show("Can't Partially Contrapose a " + Incoming.Type.ToString() + " proposition!");
            }
        }

        private void btnInvert_Click(object sender, EventArgs e)
        {
            Proposition Incoming = ProcessAndGetIncoming();

            try
            {
                Proposition Outgoing = Incoming.Invert();
                txtOutput.Text = Outgoing.ToString();
                setOutputData(Outgoing);
            }
            catch (Exception E)
            {
                MessageBox.Show("Can't Invert a " + Incoming.Type.ToString() + " proposition!");
            }
        }

        private void btnPartialInvert_Click(object sender, EventArgs e)
        {
            Proposition Incoming = ProcessAndGetIncoming();

            try
            {
                Proposition Outgoing = Incoming.PartialInvert();
                txtOutput.Text = Outgoing.ToString();
                setOutputData(Outgoing);
            }
            catch (Exception E)
            {
                MessageBox.Show("Can't Partially Invert a " + Incoming.Type.ToString() + " proposition!");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void chkShowSteps_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowSteps.Checked)
            {
                this.Width += 300;
            }
            else
            {
                this.Width -= 300;
            }
        }

        private void listBox1_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            double multiplier = (double)e.ItemWidth / (double)listBox1.Width;
            if (multiplier > 1)
            {
                double newheightnum = Math.Truncate(multiplier);
                if (newheightnum != multiplier)
                    newheightnum += 1;
                e.ItemHeight = e.ItemHeight * (int)newheightnum;
            }
        }
    }
}

