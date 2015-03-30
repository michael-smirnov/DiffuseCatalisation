using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZedGraph;

namespace RNUT
{
    class Ur_Dif
    {
        List<double[]> u1_list, u2_list;
        double p, k, y, v, c, lymb1, lymb2, t, h;
        double a, b;
        double[] star_u1, star_u2;
            
        double[] l, coef_u1,coef_u2;
        int count_fun;
        int n;
        int step = 0;
        ulong layerNum = 0;
        double[] points;
        double cos(double x, int num_fun)
        {
            double res = 0;
            if (num_fun == 0)
            {
                for (int i = 0; i < count_fun; i++)
                {
                    res += coef_u1[i] * Math.Cos(Math.PI * x * l[i]);
                }
            }
            else
            {
                for (int i = 0; i < count_fun; i++)
                {
                    res += coef_u2[i] * Math.Cos(Math.PI * x * l[i]);
                }
            }
            return res;
        }
        double sin(double x)
        {
            return Math.Pow(Math.Sin(Math.PI * x), 2) + 1;
        }
        public Ur_Dif(double[] l_, double[] coef_u1_, double[] coef_u2_, int count_fun_)
        {
            count_fun = count_fun_;
            l = new double[count_fun];
            coef_u1 = new double[count_fun];
            coef_u2 = new double[count_fun];
            for (int i = 0; i < count_fun; i++ )
            {
                l[i] = l_[i];
                coef_u1[i] = coef_u1_[i];
                coef_u2[i] = coef_u2_[i];
            }
            a = 0;
            b = 1;
        }
        public void set_p(double p_)
        {
            p = p_;
        }
        public void set_k(double k_)
        {
            k = k_;
        }
        public void set_y(double y_)
        {
            y = y_;
        }
        public void set_c(double c_)
        {
            c = c_;
        }
        public void set_lymb1(double lymb1_)
        {
            lymb1 = lymb1_;
        }
        public void set_lymb2(double lymb2_)
        {
            lymb2 = lymb2_;
        }
        public void set_t(double t_)
        {
            t = t_;
        }
        public void set_v(double v_)
        {
            v = v_;
        }
        public void set_n(int n_)
        {
            n = n_;
            h = (b - a) / (double)n;
            points = new double[n + 1];
            for (int i = 0; i < n + 1; i++)
            {
                points[i] = a + h * i;
            }
            set_u();
        }
        public int get_n()
        {
            return n;
        }
        public ulong get_layerNum()
        {
            return layerNum;
        }
        void set_u()
        {
            star_u1 = new double[n + 1];
            star_u2 = new double[n + 1];
            for (int i = 0; i < n + 1; i++)
            {
                star_u1[i] = cos(a + h * i, 0);
                star_u2[i] = cos(a + h * i, 1);
            }
            u1_list = new List<double[]>();
            u2_list =new List<double[]>();
            u1_list.Add(star_u1);
            u2_list.Add(star_u2);
        }
        public void start(int m)
        {
            double[] u1, u2, u1_old, u2_old;
            u1 = new double[n + 1];
            u2 = new double[n + 1];
            u1_old = new double[n + 1];
            u2_old = new double[n + 1];
            for (int i = 0; i < n + 1; i++)
            {
                u1_old[i] = u1_list[step][i];
                u2_old[i] = u2_list[step][i];
            }
            for (int j = 0; j < m; j++)
            {
                for (int i = 1; i < n; i++)
                {
                    u1[i] = t * (lymb1 * (u1_old[i + 1] - 2 * u1_old[i] + u1_old[i - 1]) / (h * h)
                        + k * u1_old[i] * u1_old[i] / u2_old[i] - y * u1_old[i] + p)
                        + u1_old[i];
                    u2[i] = t * (lymb2 * (u2_old[i + 1] - 2 * u2_old[i] + u2_old[i - 1]) / (h * h)
                        + c * u1_old[i] * u1_old[i] - v * u2_old[i])
                        + u2_old[i];
                }
                u1[0] = u1[1];
                u1[n] = u1[n - 1];
                u2[0] = u2[1];
                u2[n] = u2[n - 1];
                for (int i = 0; i < n + 1; i++)
                {
                    u1_old[i] = u1[i];
                    u2_old[i] = u2[i];
                }
            }
            
            u1_list.Add(u1);
            u2_list.Add(u2);
            step++;
            layerNum += Convert.ToUInt64(m);
        }

        public bool isDivirge()
        {
            bool result = false;
            double maxLymbd = lymb1;
            if (lymb2 > maxLymbd) maxLymbd = lymb2;

            if (t > h * h / (2.0 * maxLymbd))
                result = true;

            return result;
        }

        public double[] CurrentU1
        {
            get
            {
                return u1_list[step];
            }
        }

        public double[] CurrentU2
        {
            get
            {
                return u2_list[step];
            }
        }

        public double[] Points
        {
            get
            {
                return points;
            }
        }

        public void plot(ZedGraphControl zGraph, System.Windows.Forms.Label LB_norm1, System.Windows.Forms.Label LB_norm2, bool visibleStatSolution)
        {
            zGraph.GraphPane.CurveList.Clear();
            Double max_1 = 0, max_2 = 0;
            PointPairList u = new PointPairList();
            PointPairList v = new PointPairList();
            for (int i = 0; i < n + 1; i++)
            {
                max_1 = Math.Max(max_1, u1_list[step][i]);
                max_2 = Math.Max(max_2, u2_list[step][i]);
                u.Add(points[i], u1_list[step][i]);
                v.Add(points[i], u2_list[step][i]);
            }
            LB_norm1.Text = "Норма u1 = " + max_1.ToString();
            LB_norm2.Text = "Норма u2 = " + max_2.ToString();
            zGraph.GraphPane.AddCurve("u1(x) - активатор", u, System.Drawing.Color.Green, SymbolType.None);
            zGraph.GraphPane.AddCurve("u2(x) - ингибитор", v, System.Drawing.Color.Red, SymbolType.None);

            if (visibleStatSolution)
            {
                double u1Stat = (this.k * this.v / this.c + this.p) / this.y;
                double u2Stat = u1Stat * u1Stat * this.c / this.v;

                PointPairList uStat = new PointPairList();
                PointPairList vStat = new PointPairList();
                for (int i = 0; i < n + 1; i++)
                {
                    uStat.Add(points[i], u1Stat);
                    vStat.Add(points[i], u2Stat);
                }
                var u1Curve = zGraph.GraphPane.AddCurve("u1* - стац. решение для активатора", uStat, System.Drawing.Color.Green, SymbolType.None);
                u1Curve.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;
                var u2Curve = zGraph.GraphPane.AddCurve("u2* - стац. решение для ингибитора", vStat, System.Drawing.Color.Red, SymbolType.None);
                u2Curve.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;
            }

            zGraph.AxisChange();
            zGraph.Invalidate();
        }
        public void plot_star(ZedGraphControl zGraph)
        {
            
            PointPairList u = new PointPairList();
            PointPairList v = new PointPairList();
            for (int i = 0; i < n + 1; i++)
            {              
                u.Add(points[i], star_u1[i]);
                v.Add(points[i], star_u2[i]);
            }
            LineItem i1 = zGraph.GraphPane.AddCurve("u1(0) - активатор", u, System.Drawing.Color.DarkGreen, SymbolType.None);
            i1.Line.Style = System.Drawing.Drawing2D.DashStyle.DashDotDot;
            i1.Line.IsSmooth = true;

            LineItem i2 = zGraph.GraphPane.AddCurve("u2(0) - ингибитор", v, System.Drawing.Color.DarkRed, SymbolType.None);
            i2.Line.Style = System.Drawing.Drawing2D.DashStyle.DashDotDot;
            i2.Line.IsSmooth = true;
    
            zGraph.AxisChange();
            zGraph.Invalidate();
        }
        public void Show_tablU1()
        {
            Tabl tmp = new Tabl(n, u1_list[step], "u1(x)");
            tmp.Show();
        }
        public void Show_tablU2()
        {
            Tabl tmp = new Tabl(n, u2_list[step], "u2(x)");
            tmp.Show();
        }
    }
}
