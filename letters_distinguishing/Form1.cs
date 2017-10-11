using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace letters_distinguishing
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.TextBox result_tb;
        private System.Windows.Forms.GroupBox template;

        // control[number] -> Pair(seq of 0s and 1s, result)
        private Dictionary<List<int>, string> _X;
        private List<List<int>> _D;

        public int pointsX { get; private set; }
        public int pointsY { get; private set; }

        private List<Neuron> perseptron;

        public Form1()
        {
            pointsX = 3;
            pointsY = 4;

            InitializeComponent();
            CreateFormElements(pointsX, pointsY);

            GetKnowledgeBase();
            PerseptronInitialization();

            int steps = Educate();
        }

        private void CreateFormElements(int pointsX, int pointsY)
        {
            int x0 = 15, y0 = 15;
            int size = 20;

            template = new GroupBox
            {
                Location = new System.Drawing.Point(10, 10),
                Width = x0 * 2 + size * pointsX - x0 / 2,
                Height = y0 * 2 + size * pointsY - y0 / 2,
                Name = "template"
            };

            List<CheckBox> number = new List<CheckBox>();

            for (int i = 0; i < pointsY; i++)
            {
                for (int j = 0; j < pointsX; j++)
                {
                    number.Add(new CheckBox
                    {
                        Location = new System.Drawing.Point(x0 + j * size, y0 + i * size),
                        Width = size,
                        Height = size
                    });

                    template.Controls.Add(number.Last());
                }
            }
            this.Controls.Add(template);

            Button check_btn = new Button
            {
                Text = "Check",
                Name = "check_btn",
                Width = template.Width,
                Location = new System.Drawing.Point(template.Location.X, template.Location.Y + template.Height + 10)
            };
            this.Controls.Add(check_btn);

            check_btn.Click += new System.EventHandler(this.Check_btn_click);

            result_tb = new TextBox
            {
                Name = "result_tb",
                Width = template.Width,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Center,
                Font = new System.Drawing.Font("Times New Roman", 10.0F),
                Location = new System.Drawing.Point(check_btn.Location.X, check_btn.Location.Y + check_btn.Height + 10),
            };
            this.Controls.Add(result_tb);

            this.Height = 80 + template.Height + check_btn.Height + result_tb.Height;
            this.Width = template.Width + 10;
        }

        private void Check_btn_click(object sender, EventArgs e)
        {
            result_tb.Clear();
            for (int i = 0; i < 33; i++)
            {
                perseptron[i].SetX(template);
                if (perseptron[i].Y == 1)
                    result_tb.Text += perseptron[i].letter;
            }
        }

        private void PerseptronInitialization()
        {
            Neuron n;
            perseptron = new List<Neuron>();
            
            for (int i = 0; i < 33; i++)
            {
                n = new Neuron(pointsX * pointsY + 1, _X.ElementAt(i).Value);
                perseptron.Add(n);
            }
        }

        private void GetKnowledgeBase()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = "txt";
            dialog.Filter = "Text files (*.txt)|*txt";
            dialog.InitialDirectory = Environment.CurrentDirectory;
            dialog.ShowDialog();

            StreamReader f;

            if (dialog.FileName.Length != 0)
                f = new StreamReader(dialog.FileName, Encoding.Default);
            else
                return;


            List<int> _x;
            List<int> _d;
            _X = new Dictionary<List<int>, string>();
            _D = new List<List<int>>();

            for (int i = 0; i < 33 || f.EndOfStream == false; i++)
            {
                string line = f.ReadLine();

                string[] tmp = line.Split(' ');

                _x = new List<int>();
                for (int j = 0; j < 12; j++)
                {
                    _x.Add(int.Parse(tmp[j]));
                }
                _X.Add(_x, tmp.Last());

                _d = new List<int>();
                for (int j = 0; j < 33; j++)
                {
                    if (j == i)
                        _d.Add(1);
                    else
                        _d.Add(0);
                }
                _D.Add(_d);
            }

            f.Close();
        }

        private int Educate()
        {
            bool wrong_answ;
            bool tmp;
            int count = 0;

            do
            {
                wrong_answ = false;
                for (int l = 0; l < 33; l++)
                {
                    for (int i = 0; i < 33; i++)
                    {
                        perseptron[i].X = _X.ElementAt(l).Key;
                        perseptron[i].D = _D[l][i];
                        tmp = perseptron[i].CorrectW();
                        if (tmp)
                            count ++;
                        wrong_answ |= tmp;
                    }
                }
            } while (wrong_answ);

            return count;        
        }
    }
}
