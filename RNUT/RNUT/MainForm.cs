using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace RNUT
{
    public partial class MainForm : Form
    {
        Ur_Dif dif;
        double t, time;
        public MainForm()
        {
            InitializeComponent();           
            zedGraphControl1.GraphPane.Title.Text = "Функции u1(x) и u2(x)";
            zedGraphControl1.GraphPane.XAxis.Title.Text = "Ось х";
            zedGraphControl1.GraphPane.YAxis.Title.Text = "Ось u";
            zedGraphControl1.GraphPane.XAxis.Scale.Min = 0;
            zedGraphControl1.GraphPane.XAxis.Scale.Max = 1;
            zedGraphControl1.GraphPane.YAxis.Scale.Min = Convert.ToDouble(TB_min_Y.Text);
            zedGraphControl1.GraphPane.YAxis.Scale.Max = Convert.ToDouble(TB_max_Y.Text);
            CB_count_GU.SelectedIndex = 0;
           
        }

        private void setDiffParameters()
        {
            dif.set_c(Convert.ToDouble(TB_c.Text));
            dif.set_y(Convert.ToDouble(TB_y.Text));
            dif.set_v(Convert.ToDouble(TB_v.Text));
            dif.set_t(Convert.ToDouble(TB_t.Text));
            dif.set_p(Convert.ToDouble(TB_p.Text));
            dif.set_k(Convert.ToDouble(TB_k.Text));
            dif.set_lymb1(Convert.ToDouble(TB_lymb1.Text));
            dif.set_lymb2(Convert.ToDouble(TB_lymb2.Text));
        }

        private void BT_GO_Click(object sender, EventArgs e)
        {
            zedGraphControl1.GraphPane.YAxis.Scale.Min = Convert.ToDouble(TB_min_Y.Text);
            zedGraphControl1.GraphPane.YAxis.Scale.Max = Convert.ToDouble(TB_max_Y.Text);
            double[] l = new double[5];
            double[] coef_u1 = new double[5];
            double[] coef_u2 = new double[5];

            l[0] = Convert.ToDouble(NUD_GU_1.Value);
            l[1] = Convert.ToDouble(NUD_GU_2.Value);
            l[2] = Convert.ToDouble(NUD_GU_3.Value);
            l[3] = Convert.ToDouble(NUD_GU_4.Value);
            l[4] = Convert.ToDouble(NUD_GU_5.Value);

            coef_u1[0] = Convert.ToDouble(TB_GU1_1.Text);
            coef_u1[1] = Convert.ToDouble(TB_GU1_2.Text);
            coef_u1[2] = Convert.ToDouble(TB_GU1_3.Text);
            coef_u1[3] = Convert.ToDouble(TB_GU1_4.Text);
            coef_u1[4] = Convert.ToDouble(TB_GU1_5.Text);

            coef_u2[0] = Convert.ToDouble(TB_GU2_1.Text);
            coef_u2[1] = Convert.ToDouble(TB_GU2_2.Text);
            coef_u2[2] = Convert.ToDouble(TB_GU2_3.Text);
            coef_u2[3] = Convert.ToDouble(TB_GU2_4.Text);
            coef_u2[4] = Convert.ToDouble(TB_GU2_5.Text);


            dif = new Ur_Dif(l,coef_u1,coef_u2,5);
            setDiffParameters();
            dif.set_n(Convert.ToInt32(NUD_n.Text));
            timer1.Interval = Convert.ToInt32(NUD_Tick.Text);
            t = Convert.ToDouble(TB_t.Text);
            time = 0;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            dif.start(Convert.ToInt32(NUD_mem_step.Text));
            dif.plot(zedGraphControl1, CB_STAT_SOL.Checked);
            time += t * Convert.ToDouble(NUD_mem_step.Text);
            LB_Time.Text = "Текущее время = " + Convert.ToString(time);
            LB_Step.Text = "Слой = " + dif.get_layerNum();
        }

        private void BT_Stop_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void BT_Start_Click(object sender, EventArgs e)
        {
            if (dif == null)
            {
                BT_GO_Click(sender, e);
            }
            else
            {
                zedGraphControl1.GraphPane.YAxis.Scale.Min = Convert.ToDouble(TB_min_Y.Text);
                zedGraphControl1.GraphPane.YAxis.Scale.Max = Convert.ToDouble(TB_max_Y.Text);
                setDiffParameters();
                timer1.Interval = Convert.ToInt32(NUD_Tick.Text);
                timer1.Start();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            NUD_GU_2.Enabled = false;
            TB_GU1_2.Enabled = false;
            TB_GU2_2.Enabled = false;

            NUD_GU_3.Enabled = false;
            TB_GU1_3.Enabled = false;
            TB_GU2_3.Enabled = false;

            NUD_GU_4.Enabled = false;
            TB_GU1_4.Enabled = false;
            TB_GU2_4.Enabled = false;

            NUD_GU_5.Enabled = false;
            TB_GU1_5.Enabled = false;
            TB_GU2_5.Enabled = false;

            if (CB_count_GU.SelectedIndex > 0)
            {
                NUD_GU_2.Enabled = true;
                TB_GU1_2.Enabled = true;
                TB_GU2_2.Enabled = true;
                if (CB_count_GU.SelectedIndex > 1)
                {
                    NUD_GU_3.Enabled = true;
                    TB_GU1_3.Enabled = true;
                    TB_GU2_3.Enabled = true;
                    if (CB_count_GU.SelectedIndex > 2)
                    {
                        NUD_GU_4.Enabled = true;
                        TB_GU1_4.Enabled = true;
                        TB_GU2_4.Enabled = true;
                        if (CB_count_GU.SelectedIndex > 3)
                        {
                            NUD_GU_5.Enabled = true;
                            TB_GU1_5.Enabled = true;
                            TB_GU2_5.Enabled = true;
                        }
                    }
                }
            }
        }

      

        private void NUD_GU_2_ValueChanged(object sender, EventArgs e)
        {
            NUD_GU_3.Minimum = NUD_GU_2.Value + 1;
            NUD_GU_4.Minimum = NUD_GU_2.Value + 2;
            NUD_GU_5.Minimum = NUD_GU_2.Value + 3;
        }

        private void NUD_GU_2_KeyUp(object sender, KeyEventArgs e)
        {
            NUD_GU_2_ValueChanged(sender, e);
        }

        private void NUD_GU_3_ValueChanged(object sender, EventArgs e)
        {
            NUD_GU_4.Minimum = NUD_GU_3.Value + 1;
            NUD_GU_5.Minimum = NUD_GU_3.Value + 2;
        }

        private void NUD_GU_3_KeyUp(object sender, KeyEventArgs e)
        {
            NUD_GU_3_ValueChanged(sender, e);
        }

        private void NUD_GU_4_ValueChanged(object sender, EventArgs e)
        {
            NUD_GU_5.Minimum = NUD_GU_4.Value + 1;
        }

        private void NUD_GU_4_KeyUp(object sender, KeyEventArgs e)
        {
            NUD_GU_4_ValueChanged(sender, e);
        }

        private void NUD_GU_5_ValueChanged(object sender, EventArgs e)
        {
            int a = (int)NUD_GU_5.Value;
        }

        private void NUD_GU_5_KeyUp(object sender, KeyEventArgs e)
        {
            NUD_GU_5_ValueChanged(sender, e);
        }

      
    }
}
