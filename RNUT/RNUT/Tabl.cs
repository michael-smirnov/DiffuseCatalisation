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
        double[] u;
        string uName;

        public Tabl(int n_, double[] u_, string uName_)
        {
            InitializeComponent();
            uName = uName_;
            Text = "Таблица значений функции " + uName;
            n = n_;
            u = new double[n + 1];
            for (int i = 0; i < n + 1; i++)
            {
                u[i] = u_[i];
            }
            plot();
        }
        private void plot()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            dataGridView1.Columns.Add("colX", "x");
            dataGridView1.Columns.Add("colU", uName);
            string[] tmp = new string[2];
            double h = 1.0 / n;
            double x = 0;
            for (int i = 0; i < n + 1; i++)
            {
                tmp[0] = Convert.ToString(x);
                tmp[1] = Convert.ToString(u[i]);

                x += h;
                dataGridView1.Rows.Add(tmp);
            }
        }
    }
}
