using System;
using System.Text.RegularExpressions;

namespace ChangingPropositions
{
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

        public Quantity Quantity { get; set; }
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
            MainForm.DisplayForm.listBox1.Items.Add(final.ToString());
            return final;
        }

        public Proposition Convert()
        {
            Proposition con = this.Clone();

            if (con.Type == PredicateType.O)
            {
                throw new InvalidOperationException();
            }
            
            string subject = "";
            if (!string.IsNullOrEmpty(con.twi))
            {
                subject += con.twi + " ";
            }
            if (con.nonPredicate)
            {
                if (!string.IsNullOrEmpty(con.twi))
                {
                    subject += " ";
                }
                subject += "non-";
            }
            subject += con.Predicate;

            con.Predicate = con.Subject;
            con.Subject = subject;

            con.nonPredicate = false;
            con.twi = "";

            if (con.Type == PredicateType.A)
            {
                con.Type = PredicateType.I;
                con.Quantity = Quantity.Some;
            }
            Proposition final = Proposition.Parse(con.ToString());
            (MainForm.DisplayForm).listBox1.Items.Add(final.ToString());
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
