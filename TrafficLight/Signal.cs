using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrafficLight
{
    public partial class Signal : Form
    {
        const int DIA = 100;
        int x = 10;
        int y = 10;
        int width = DIA;
        int height = DIA;
        Timer timer = new Timer();
        int cycle = 0;
        Brush color = Brushes.Red;

        Rectangle[] rectangles = new Rectangle[3];

        public Signal()
        {
            InitializeComponent();
            this.BackColor = Color.Black;
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            this.Paint += Signal_Paint;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {

            if (cycle == 0)
            {
                color = Brushes.Red;
            } else if (cycle == 1)
            {
                color = Brushes.Yellow;
            } else if (cycle == 2)

            {
                color = Brushes.Green;
            }
            
            Invalidate();
        }

        private void Signal_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen pen = new Pen(Brushes.White);
            rectangles[0] = new Rectangle(x, y, DIA, DIA);
            for (int i = 0; i < 3; i++)
            {
                rectangles[i] = new Rectangle(x, y + i * (width + 10), width, height);
                g.DrawEllipse(pen, rectangles[i]);
            }

            g.FillEllipse(color, rectangles[cycle]);
            cycle = ++cycle % 3;
        }
    }
}
