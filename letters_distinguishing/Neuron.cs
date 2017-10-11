using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace letters_distinguishing
{
    class Neuron
    {
        public List<int> x;
        public List<int> X
        {
            get
            {
                return x;
            }
            set
            {
                for (int j = 1; j < x.Count; j++)
                {
                    x[j] = value[j - 1];
                }
            }
        }
        public int D {
            get; set;
        }

        public int Y
        {
            get
            {
                if (S >= 0)
                    return 1;
                return 0;
            }
        }
        private List<double> w;

        private double s;
        private double S
        {
            get
            {
                s = 0;
                for (int j = 0; j < x.Count; j++)
                {
                    s += x[j] * w[j];
                }
                return s;
            }
        }
        public int E
        {
            get
            {
                return D - Y;
            }
        }

        public string letter;
          

        public Neuron(int entries, string _letter)
        {
            letter = _letter;

            x = new List<int>(entries);
            w = new List<double>(entries);

            for (int j = 0; j < entries; j++)
            { 
                x.Add(0);

                Random rand = new Random();
                w.Add(rand.Next(-5, 5) + rand.NextDouble());
            }

            x[0] = 1;
        }

        public void SetX(GroupBox template)
        {
            for (int i = 1; i < x.Count; i++)
            {
                if ((template.Controls[i-1] as CheckBox).Checked)
                    x[i] = 1;
                else
                    x[i] = 0;
            }
        }

        /// <summary>
        /// Correct w for each entry
        /// </summary>
        /// <returns>true if correction has been done</returns>
        public bool CorrectW()
        {
            int err = E;

            if (err == 0)
                return false;

            for (int j = 0; j < w.Count; j++)
            {
                w[j] += err * x[j];
            }
            return true;
        }
    }
}
