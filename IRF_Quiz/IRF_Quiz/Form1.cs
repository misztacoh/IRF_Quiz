using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IRF_Quiz
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            string[] selectorElements = new string[2];
            selectorElements[0] = "Quiz";
            selectorElements[1] = "Statisztika";

            cbSelector.DataSource = selectorElements;
        }

        private void cbSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSelector.Text == "Quiz")
            {
                Quizes uc = new Quizes();
                panel1.Controls.Clear();
                panel1.Controls.Add(uc);
                uc.Dock = DockStyle.Fill;
            }
            else if (cbSelector.Text == "Statisztika")
            {
                Statistics uc = new Statistics();
                panel1.Controls.Clear();
                panel1.Controls.Add(uc);
                uc.Dock = DockStyle.Fill;
            }
        }


    }
}
