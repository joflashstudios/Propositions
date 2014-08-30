using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ChangingPropositions
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
            dgvOutput.Rows.Add("Quality", output.Quality ? "Positive"  : "Negitive");
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

    public class Proposition
    {
        private Proposition Clone()
        {
            Proposition P = new Proposition();
            P.Quality = this.Quality;
            P.Quantity = this.Quantity;
            P.CopulaWord = this.CopulaWord;
            P.nonPredicate = this.nonPredicate;
            P.Predicate = this.Predicate;
            P.Subject = this.Subject;
            P.twi = this.twi;
            P.Type = this.Type;
            return P;
        }

        public Quantity Quantity {get; set;}
        public string CopulaWord;
        public bool Quality;
        public bool nonPredicate;
        public string Subject;
        public string Predicate;
        public PredicateType Type;
        public string twi;

        public static int NthIndexOf(string target, string value, int n)
        {
            Match m = Regex.Match(target, "((" + value + ").*?){" + n + "}");

            if (m.Success)
                return m.Groups[2].Captures[n - 1].Index;
            else
                return -1;
        }

        public static Proposition Parse(string Proposition)
        {
            Proposition Prop = new Proposition();
            string sprop = Proposition;

            #region Quality and Type if Possible
            if (sprop.StartsWith("All"))
            {
                Prop.Quantity = Quantity.All;
                Prop.Type = PredicateType.A;
                sprop = sprop.Substring(4);
            }
            else if (sprop.StartsWith("No"))
            {
                Prop.Quantity = Quantity.No;
                Prop.Type = PredicateType.E;
                Prop.Quality = false;
                sprop = sprop.Substring(3);
            }
            else if (sprop.StartsWith("Some"))
            {
                Prop.Quantity = Quantity.Some;
                Prop.Type = PredicateType.Undefined;
                sprop = sprop.Substring(5);
            }
            else
            {
                throw new FormatException("Proposition must start with \"All\", \"Some\", or \"No\"\nCheck your proposition for errors.");
            }
            #endregion

            if (sprop.StartsWith("["))
            {
                sprop = sprop.Substring(1);
                Prop.Subject = sprop.Substring(0, sprop.IndexOf(']'));
                sprop = sprop.Substring(sprop.IndexOf(']') + 2);
            }
            else { throw new FormatException("Proposition does not follow correct input format.\nCheck your proposition for errors."); }

            if (sprop.StartsWith("are"))
            {
                Prop.CopulaWord = "are";
                sprop = sprop.Substring(4);
            }
            else if (sprop.StartsWith("is"))
            {
                Prop.CopulaWord = "is";
                sprop = sprop.Substring(3);
            }
            else { throw new FormatException("Copula must start with \"is\" or \"are\".\nCheck your proposition for errors."); }

            if (sprop.StartsWith("not"))
            {
                Prop.Quality = false;
                if (Prop.Type == PredicateType.Undefined)
                {
                    Prop.Type = PredicateType.O;
                }
                sprop = sprop.Substring(4);
            }
            else
            {
                if (Prop.Type == PredicateType.Undefined)
                {
                    Prop.Type = PredicateType.I;
                }
                if (Prop.Type != PredicateType.E)
                {
                    Prop.Quality = true;
                }
            }

            if (sprop.StartsWith("["))
            {
                sprop = sprop.Substring(1);                
            }
            else { throw new FormatException("Proposition does not follow correct input format.\nCheck your proposition for errors."); }

            if (sprop.StartsWith("tw are"))
            {
                Prop.twi = "tw are";
                sprop = sprop.Substring(7);
            }
            else if (sprop.StartsWith("twi a"))
            {
                Prop.twi = "twi a";
                sprop = sprop.Substring(6);
            }
            else if (sprop.StartsWith("twi"))
            {
                Prop.twi = "twi";
                sprop = sprop.Substring(4);
            }            
            else if (sprop.StartsWith("tw"))
            {
                Prop.twi = "tw";
                sprop = sprop.Substring(3);
            }            

            if (sprop.StartsWith("non-"))
            {
                Prop.nonPredicate = true;
                sprop = sprop.Substring(4);
            }
            else
            {
                Prop.nonPredicate = false;
            }

            Prop.Predicate = sprop.Substring(0, sprop.IndexOf(']'));
            sprop = sprop.Substring(sprop.IndexOf(']') + 1);
            return Prop;
        }

        public override string ToString()
        {
            string sprop = "";
            sprop += this.Quantity.ToString();
                sprop += " [";
            sprop += this.Subject;
                sprop += "] ";
            sprop += this.CopulaWord;

            sprop += " "; if (this.Type != PredicateType.E) //doesn't follow the rules - leading "No"
            {
                sprop += this.Quality ? "" : "not ";
            }
                sprop += "[";
            
            sprop += (!string.IsNullOrEmpty(twi) ? this.twi + " " : "");
            sprop += this.nonPredicate ? "non-" : "";
            sprop += this.Predicate;
                sprop += "]";

            return sprop;
        }

        public Proposition Contrapose()
        {
            Proposition ob = this.Clone();
            return ob.Obvert().Convert().Obvert();
        }

        public Proposition PartialContrapose()
        {
            Proposition ob = this.Clone();
            return ob.Obvert().Convert();
        }

        public Proposition Invert()
        {
            Proposition ob = this.Clone();
            return ob.Convert().Obvert().Convert();
        }

        public Proposition PartialInvert()
        {
            Proposition ob = this.Clone();
            return ob.Convert().Obvert();
        }

        public Proposition Obvert()
        {
            Proposition ob = this.Clone();

            ob.nonPredicate = !ob.nonPredicate;
            if (ob.Type == PredicateType.A)
            {
                ob.Quantity = ChangingPropositions.Quantity.No;                
            }
            if (ob.Type == PredicateType.E)
            {
                ob.Quantity = ChangingPropositions.Quantity.All;
            }
            if (ob.Type == PredicateType.I || ob.Type == PredicateType.O)
            {
                ob.Quality = !ob.Quality;
            }
            Proposition final = Proposition.Parse(ob.ToString());
            ((Form1)(Application.OpenForms[0])).listBox1.Items.Add(final.ToString());
            return final;
        }

        public Proposition Convert()
        {
            Proposition con = this.Clone();

            if (con.Type == PredicateType.O)
            {
                throw new InvalidOperationException();
            }

            string SubHolder = con.Subject;

            string subject = "";
            if (!string.IsNullOrEmpty(con.twi))
            {
                subject += con.twi + " ";
            }     
            if (con.nonPredicate)
            {
                if(!string.IsNullOrEmpty(con.twi))
                {
                    subject += " ";
                }
                subject += "non-";
            }
            subject += con.Predicate;

            con.Subject = subject;
            con.Predicate = SubHolder;

            con.nonPredicate = false;
            con.twi = "";

            if (con.Type == PredicateType.A)
            {
                con.Type = PredicateType.I;
                con.Quantity = Quantity.Some;
            }
            Proposition final = Proposition.Parse(con.ToString());
            ((Form1)(Application.OpenForms[0])).listBox1.Items.Add(final.ToString());
            return final;
        }
    }

    public enum Quantity
    {
        Undefined,
        All,
        No,
        Some
    }
    
    public enum PredicateType
    {
        Undefined,
        A,
        E,
        I,
        O
    }
}
