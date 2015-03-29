using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RNUT
{
    public partial class Tabl : Form
    {
        int n;
        double[] u1, u2;
        public Tabl(int n_,double[] u1_,double[] u2_)
        {
            InitializeComponent();
            n = n_ + 1;
            u1 = new double[n];
            u2 = new double[n];
            for (int i = 0; i < n; i++)
            {
                u1[i] = u1_[i];
                u2[i] = u2_[i];
            }
            plot();
        }
        private void plot()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.ColumnCount = n + 1;
            string[] tmp = new string[n + 1];
            string[] tmp1 = new string[n + 1];
                for (int i = 0; i < n; i++)
                {
                    tmp[i] = Convert.ToString(u1[i]);
                    tmp1[i] = Convert.ToString(u2[i]);
                }
                dataGridView1.Rows.Add(tmp);
                dataGridView1.Rows.Add(tmp1);
            
        }
    }
}
