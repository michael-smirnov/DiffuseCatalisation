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

        // Для сравнения решений с разными НУ
        List<NU_Holder> NUs = new List<NU_Holder>();
        Ur_Dif dif1;
        Ur_Dif dif2;

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
            cbNumFunc.SelectedIndex = 0;
           
        }

        private void setDiffParameters(Ur_Dif _dif)
        {
            _dif.set_c(Convert.ToDouble(TB_c.Text));
            _dif.set_y(Convert.ToDouble(TB_y.Text));
            _dif.set_v(Convert.ToDouble(TB_v.Text));
            _dif.set_t(Convert.ToDouble(TB_t.Text));
            _dif.set_p(Convert.ToDouble(TB_p.Text));
            _dif.set_k(Convert.ToDouble(TB_k.Text));
            _dif.set_lymb1(Convert.ToDouble(TB_lymb1.Text));
            _dif.set_lymb2(Convert.ToDouble(TB_lymb2.Text));
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
            setDiffParameters(dif);
            dif.set_n(Convert.ToInt32(NUD_n.Text));
            timer1.Interval = Convert.ToInt32(NUD_Tick.Text);
            t = Convert.ToDouble(TB_t.Text);
            time = 0;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            dif.start(Convert.ToInt32(NUD_mem_step.Text));
            dif.plot(zedGraphControl1);
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
                setDiffParameters(dif);
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

        private void cbNumFunc_SelectedIndexChanged(object sender, EventArgs e)
        {
            numNU2.Enabled = false;
            tbNU1_2.Enabled = false;
            tbNU2_2.Enabled = false;

            numNU3.Enabled = false;
            tbNU1_3.Enabled = false;
            tbNU2_3.Enabled = false;

            numNU4.Enabled = false;
            tbNU1_4.Enabled = false;
            tbNU2_4.Enabled = false;

            numNU5.Enabled = false;
            tbNU1_5.Enabled = false;
            tbNU2_5.Enabled = false;

            if (cbNumFunc.SelectedIndex > 0)
            {
                numNU2.Enabled = true;
                tbNU1_2.Enabled = true;
                tbNU2_2.Enabled = true;
                if (cbNumFunc.SelectedIndex > 1)
                {
                    numNU3.Enabled = true;
                    tbNU1_3.Enabled = true;
                    tbNU2_3.Enabled = true;
                    if (cbNumFunc.SelectedIndex > 2)
                    {
                        numNU4.Enabled = true;
                        tbNU1_4.Enabled = true;
                        tbNU2_4.Enabled = true;
                        if (cbNumFunc.SelectedIndex > 3)
                        {
                            numNU5.Enabled = true;
                            tbNU1_5.Enabled = true;
                            tbNU2_5.Enabled = true;
                        }
                    }
                }
            }
        }

        private int getIndexWithSameValue(int[] value, int curIndex)
        {
            int indSameValue = -1;
            for (int j = 0; j < value.Length; j++)
            {
                if (value[curIndex] == value[j] && (curIndex != j))
                {
                    indSameValue = j;
                    break;
                }
            }
            return indSameValue;
        }

        private int[] changeSameNU(int[] values, int unchangingIndex)
        {
            int[] res = new int[4];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = values[i];
            }
            for (int i = 0; i < res.Length; i++)
            {
                if (i == unchangingIndex)
                    continue;

                int newVal = 1;
                while (getIndexWithSameValue(res, i) != -1)
                {
                    res[i] = newVal++;
                }
            }
            return res;
        }

        private void numNU2_ValueChanged(object sender, EventArgs e)
        {
            int[] valuesNU = new int[4];
            valuesNU[0] = Convert.ToInt32(numNU2.Value);
            valuesNU[1] = Convert.ToInt32(numNU3.Value);
            valuesNU[2] = Convert.ToInt32(numNU4.Value);
            valuesNU[3] = Convert.ToInt32(numNU5.Value);

            int[] newValues = changeSameNU(valuesNU, 0);
            numNU2.Value = newValues[0];
            numNU3.Value = newValues[1];
            numNU4.Value = newValues[2];
            numNU5.Value = newValues[3];
        }

        private void numNU3_ValueChanged(object sender, EventArgs e)
        {
            int[] valuesNU = new int[4];
            valuesNU[0] = Convert.ToInt32(numNU2.Value);
            valuesNU[1] = Convert.ToInt32(numNU3.Value);
            valuesNU[2] = Convert.ToInt32(numNU4.Value);
            valuesNU[3] = Convert.ToInt32(numNU5.Value);

            int[] newValues = changeSameNU(valuesNU, 1);
            numNU2.Value = newValues[0];
            numNU3.Value = newValues[1];
            numNU4.Value = newValues[2];
            numNU5.Value = newValues[3];
        }

        private void numNU4_ValueChanged(object sender, EventArgs e)
        {
            int[] valuesNU = new int[4];
            valuesNU[0] = Convert.ToInt32(numNU2.Value);
            valuesNU[1] = Convert.ToInt32(numNU3.Value);
            valuesNU[2] = Convert.ToInt32(numNU4.Value);
            valuesNU[3] = Convert.ToInt32(numNU5.Value);

            int[] newValues = changeSameNU(valuesNU, 2);
            numNU2.Value = newValues[0];
            numNU3.Value = newValues[1];
            numNU4.Value = newValues[2];
            numNU5.Value = newValues[3];
        }

        private void numNU5_ValueChanged(object sender, EventArgs e)
        {
            int[] valuesNU = new int[4];
            valuesNU[0] = Convert.ToInt32(numNU2.Value);
            valuesNU[1] = Convert.ToInt32(numNU3.Value);
            valuesNU[2] = Convert.ToInt32(numNU4.Value);
            valuesNU[3] = Convert.ToInt32(numNU5.Value);

            int[] newValues = changeSameNU(valuesNU, 3);
            numNU2.Value = newValues[0];
            numNU3.Value = newValues[1];
            numNU4.Value = newValues[2];
            numNU5.Value = newValues[3];
        }

        private void addNU_Click(object sender, EventArgs e)
        {
            NU_Holder holder = new NU_Holder();

            holder.countL = Convert.ToInt32(cbNumFunc.SelectedItem);
            holder.l[0] = Convert.ToDouble(numNU1.Value);
            holder.l[1] = Convert.ToDouble(numNU2.Value);
            holder.l[2] = Convert.ToDouble(numNU3.Value);
            holder.l[3] = Convert.ToDouble(numNU4.Value);
            holder.l[4] = Convert.ToDouble(numNU5.Value);

            holder.coef_u1[0] = Convert.ToDouble(tbNU1_1.Text);
            holder.coef_u1[1] = Convert.ToDouble(tbNU1_2.Text);
            holder.coef_u1[2] = Convert.ToDouble(tbNU1_3.Text);
            holder.coef_u1[3] = Convert.ToDouble(tbNU1_4.Text);
            holder.coef_u1[4] = Convert.ToDouble(tbNU1_5.Text);

            holder.coef_u2[0] = Convert.ToDouble(tbNU2_1.Text);
            holder.coef_u2[1] = Convert.ToDouble(tbNU2_2.Text);
            holder.coef_u2[2] = Convert.ToDouble(tbNU2_3.Text);
            holder.coef_u2[3] = Convert.ToDouble(tbNU2_4.Text);
            holder.coef_u2[4] = Convert.ToDouble(tbNU2_5.Text);

            NUs.Add(holder);
            string name = "Начальные условия " + (listNU.Items.Count + 1);
            listNU.Items.Add(name);
            cbNU1.Items.Add(name);
            cbNU2.Items.Add(name);
        }

        private void listNU_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listNU.SelectedItems.Count == 1)
            {
                deleteNU.Enabled = true;
                NU_Holder nh = NUs[listNU.SelectedIndex];

                for (int i = 0; i < cbNumFunc.Items.Count; i++)
                {
                    if (String.Compare(cbNumFunc.Items[i].ToString(), nh.countL.ToString()) == 0)
                    {
                        cbNumFunc.SelectedIndex = i;
                        break;
                    }
                }
                numNU1.Value = Convert.ToDecimal(nh.l[0]);
                numNU2.Value = Convert.ToDecimal(nh.l[1]);
                numNU3.Value = Convert.ToDecimal(nh.l[2]);
                numNU4.Value = Convert.ToDecimal(nh.l[3]);
                numNU5.Value = Convert.ToDecimal(nh.l[4]);

                tbNU1_1.Text = Convert.ToString(nh.coef_u1[0]);
                tbNU1_2.Text = Convert.ToString(nh.coef_u1[1]);
                tbNU1_3.Text = Convert.ToString(nh.coef_u1[2]);
                tbNU1_4.Text = Convert.ToString(nh.coef_u1[3]);
                tbNU1_5.Text = Convert.ToString(nh.coef_u1[4]);

                tbNU2_1.Text = Convert.ToString(nh.coef_u2[0]);
                tbNU2_2.Text = Convert.ToString(nh.coef_u2[1]);
                tbNU2_3.Text = Convert.ToString(nh.coef_u2[2]);
                tbNU2_4.Text = Convert.ToString(nh.coef_u2[3]);
                tbNU2_5.Text = Convert.ToString(nh.coef_u2[4]);
            }
        }

        private void deleteNU_Click(object sender, EventArgs e)
        {
            int index = listNU.SelectedIndex;
            listNU.Items.RemoveAt(index);
            cbNU1.Items.RemoveAt(index);
            cbNU2.Items.RemoveAt(index);
            NUs.RemoveAt(index);

            if (listNU.Items.Count == 0)
            {
                deleteNU.Enabled = false;
                cbNU1.Text = "";
                cbNU2.Text = "";
            }
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            if(cbNU1.SelectedIndex > -1 && cbNU2.SelectedIndex > -1)
            {
                NU_Holder nu1 = NUs[cbNU1.SelectedIndex];
                NU_Holder nu2 = NUs[cbNU2.SelectedIndex];

                zedGraphControl1.GraphPane.YAxis.Scale.Min = Convert.ToDouble(TB_min_Y.Text);
                zedGraphControl1.GraphPane.YAxis.Scale.Max = Convert.ToDouble(TB_max_Y.Text);

                dif1 = new Ur_Dif(nu1.l, nu1.coef_u1, nu1.coef_u2, 5);
                dif2 = new Ur_Dif(nu2.l, nu2.coef_u1, nu2.coef_u2, 5);
                setDiffParameters(dif1);
                setDiffParameters(dif2);
                dif1.set_n(Convert.ToInt32(NUD_n.Text));
                dif2.set_n(Convert.ToInt32(NUD_n.Text));

                timer2.Interval = Convert.ToInt32(NUD_Tick.Text);
                t = Convert.ToDouble(TB_t.Text);
                time = 0;
                timer2.Start();
            }
            else
            {
                MessageBox.Show("Необходимо выбрать н.у. для сравнения");
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            dif1.start(Convert.ToInt32(NUD_mem_step.Text));
            dif2.start(Convert.ToInt32(NUD_mem_step.Text));

            zedGraphControl1.GraphPane.CurveList.Clear();
            PointPairList u1 = new PointPairList();
            PointPairList v1 = new PointPairList();

            PointPairList u2 = new PointPairList();
            PointPairList v2 = new PointPairList();

            PointPairList raznAct = new PointPairList();
            PointPairList raznIng = new PointPairList();
            double maxRaznAct = 0;
            double maxRaznIng = 0;
            for (int i = 0; i < dif1.get_n() + 1; i++)
            {
                u1.Add(dif1.Points[i], dif1.CurrentU1[i]);
                v1.Add(dif1.Points[i], dif1.CurrentU2[i]);

                u2.Add(dif2.Points[i], dif2.CurrentU1[i]);
                v2.Add(dif2.Points[i], dif2.CurrentU2[i]);

                double raznCurAct = Math.Abs(dif2.CurrentU1[i] - dif1.CurrentU1[i]);
                double raznCurIng = Math.Abs(dif2.CurrentU2[i] - dif1.CurrentU2[i]);
                maxRaznAct = Math.Max(maxRaznAct, raznCurAct);
                maxRaznIng = Math.Max(maxRaznIng, raznCurIng);
                raznAct.Add(dif1.Points[i], raznCurAct);
                raznIng.Add(dif1.Points[i], raznCurIng);
            }
            lbRaznNormAct.Text = "||u1-v1|| = " + maxRaznAct;
            lbRaznNormIng.Text = "||u2-v2|| = " + maxRaznIng;

            if (chbFirst.Checked)
            {
                zedGraphControl1.GraphPane.AddCurve("u1(x) - активатор, н.у. 1", u1, System.Drawing.Color.Green, SymbolType.None);
                zedGraphControl1.GraphPane.AddCurve("u2(x) - ингибитор, н.у. 1", v1, System.Drawing.Color.DarkRed, SymbolType.None);
            }

            if (chbSecond.Checked)
            {
                zedGraphControl1.GraphPane.AddCurve("u1(x) - активатор, н.у. 2", u2, System.Drawing.Color.LightGreen, SymbolType.None);
                zedGraphControl1.GraphPane.AddCurve("u2(x) - ингибитор, н.у. 2", v2, System.Drawing.Color.Red, SymbolType.None);
            }

            if (chbRazn.Checked)
            {
                zedGraphControl1.GraphPane.AddCurve("Модуль разности активаторов", raznAct, System.Drawing.Color.DarkBlue, SymbolType.None);
                zedGraphControl1.GraphPane.AddCurve("Модуль разности ингибиторов", raznIng, System.Drawing.Color.DarkGoldenrod, SymbolType.None);
            }
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();

            time += t * Convert.ToDouble(NUD_mem_step.Text);
            LB_Time.Text = "Текущее время = " + Convert.ToString(time);
            LB_Step.Text = "Слой = " + dif1.get_layerNum();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            timer2.Stop();
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            if (dif1 == null || dif2 == null)
            {
                btnSolve_Click(sender, e);
            }
            else
            {
                zedGraphControl1.GraphPane.YAxis.Scale.Min = Convert.ToDouble(TB_min_Y.Text);
                zedGraphControl1.GraphPane.YAxis.Scale.Max = Convert.ToDouble(TB_max_Y.Text);
                setDiffParameters(dif1);
                setDiffParameters(dif2);
                timer2.Interval = Convert.ToInt32(NUD_Tick.Text);
                timer2.Start();
            }
        }     
    }
}
