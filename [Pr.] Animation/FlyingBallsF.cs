using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace _Pr.__Animation
{
    public partial class FlyingBallsF : Form
    {
        private bool TimerStarted = false, flag;
        private double nowYd = 0, nowXd = 0, prevYd;
        private double velX, velY = 0, dY, dX, Y0, X0;
        private int x0 = 50, y0 = 300, ex = 50, ey = 300;
        private Bitmap bmp;
        private Graphics g;
        private SolidBrush br = new SolidBrush(Color.Red);
        private Rectangle r;
        private Timer tmr = new Timer();

        public FlyingBallsF()
        {
            InitializeComponent();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bmp);
            pictureBox1.Image = bmp;
            g.SmoothingMode = SmoothingMode.HighQuality;
            r = new Rectangle(x0 - 13, y0 - 13, 26, 26);
            g.FillEllipse(br, r);
            g.DrawEllipse(Pens.Black, r);
        }

        private void сменитьToolStripMenuItem_Click(object sender, EventArgs e){}

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Разработчик: Токарев А.А.");
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (TimerStarted == false && e.Button == MouseButtons.Left)
            {
                g.Clear(Color.White);
                g.FillEllipse(br, r);
                g.DrawEllipse(Pens.Black, r);
                ex = e.X;
                ey = e.Y;
                Pen mypen = new Pen(Color.Black, 5);
                mypen.StartCap = LineCap.RoundAnchor;
                mypen.EndCap = LineCap.ArrowAnchor;
                g.DrawLine(mypen, x0, y0, ex, ey);
                pictureBox1.Refresh();
            }
            if (TimerStarted == false && e.Button == MouseButtons.Right)
            {
                x0 = e.X;
                y0 = e.Y;
                g.Clear(Color.White);
                r = new Rectangle(x0 - 13, y0 - 13, 26, 26);
                g.FillEllipse(br, r);
                g.DrawEllipse(Pens.Black, r);
                pictureBox1.Refresh();
            }
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            TimerStarted = false;
            tmr.Stop();
            ex = x0;
            ey = y0;
            g.Clear(Color.White);
            r = new Rectangle(x0 - 13, y0 - 13, 26, 26);
            g.FillEllipse(br, r);
            g.DrawEllipse(Pens.Black, r);
            pictureBox1.Refresh();
        }
        

        private int NowX ()
        {
            nowXd = X0 + 1.7 * dX / 200.0;
            return (int)Math.Truncate(nowXd);
        }
        
        private int NowY ()
        {
            prevYd = nowYd;
            nowYd = Y0 + 1.7 * dY / 8000;
            return (int)Math.Truncate(nowYd);
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            if (TimerStarted == true)
            {
                TimerStarted = false;
                tmr.Stop();
            }
            flag = false;
            tmr = new Timer();
            g.Clear(Color.White);
            tmr.Interval = (int)UpdFreq.Value;
            int x = x0;
            int y = y0;
            int tx = 0, ty = 0;
            Y0 = y0;
            X0 = x0;
            velX = ex - x0;
            velY = (ey - y0) * 40;
            r = new Rectangle(x0 - 13, y0 - 13, 26, 26);
            g.FillEllipse(br, r);
            g.DrawEllipse(Pens.Black, r);
            tmr.Tick += new EventHandler((o, ev) =>
            {
                if (y >= pictureBox1.Height - 13 || y <= 0)
                {
                    velY = (-1 * velY - 9.81 * 10 * (ty - tmr.Interval)) * (double)ElastCoef.Value;
                    ty = 0;
                    if (y >= pictureBox1.Height - 13)
                    {
                        Y0 = prevYd;
                        if (prevYd >= pictureBox1.Height - 13) flag = true;
                    }
                    else Y0 = prevYd;
                }
                if (x >= pictureBox1.Width || x <= 13)
                {
                    velX = -velX * (double)ElastCoef.Value;
                    tx = 0;
                    if (x >= pictureBox1.Width) X0 = pictureBox1.Width - 13;
                    else X0 = 13;
                }
                ty += tmr.Interval;
                tx += tmr.Interval;
                dX = velX * tx;
                dY = 10 * (9.81 / 2) * ty * ty + velY * ty;
                x = NowX();
                y = NowY();
                g.Clear(Color.White);
                if (flag == false) r = new Rectangle(x - 13, y - 13, 26, 26);
                else r = new Rectangle(x - 13, pictureBox1.Height - 26, 26, 26);
                g.FillEllipse(br, r);
                g.DrawEllipse(Pens.Black, r);
                pictureBox1.Refresh();
            });
            TimerStarted = true;
            tmr.Start();
        }
    }
}
