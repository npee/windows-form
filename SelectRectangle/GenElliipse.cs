using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SelectRectangle
{
    public partial class GenElliipse : Form
    {

        const int DIA = 100;
        int rectX = 0;
        int rectY = 0;
        int width = DIA;
        int height = DIA;

        int newEllipseX = 0;
        int newEllipseY = 0;

        List<Rectangle> ellipses = new List<Rectangle>();
        Rectangle[] rectangles = new Rectangle[32];

        public GenElliipse()
        {
            InitializeComponent();
            this.Paint += GenElliipse_Paint;
            this.MouseClick += GenElliipse_MouseClick;
            this.DoubleBuffered = true;
        }

        private void GenElliipse_MouseClick(object sender, MouseEventArgs e)
        {
            newEllipseX = e.X / 100 * 100; // e.X;
            newEllipseY = e.Y / 100 * 100; // e.Y;
            ellipses.Add(new Rectangle(newEllipseX, newEllipseY, width, height));
            rectX = 0;
            rectY = 0;
            
            Invalidate();
            
        }

        private void GenElliipse_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen pen = new Pen(Brushes.Black);
            Pen redpen = new Pen(Brushes.Red);
            Pen bluepen = new Pen(Brushes.Blue);

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    rectangles[i].X = rectX;
                    rectangles[i].Y = rectY;
                    rectangles[i].Width = width;
                    rectangles[i].Height = height;
                    g.DrawRectangle(pen, rectangles[i]);
                    rectX += width;
                }
                rectX = 0;
                rectY += height;
            }
            // if (ellipses.Count != 0)
            int ellipseCount = 0;

            ellipses.ForEach(ellipse =>
            {
                if (ellipseCount % 2 == 0)
                    g.DrawEllipse(redpen, new Rectangle(ellipse.X, ellipse.Y, width, height));
                else
                    g.DrawEllipse(bluepen, new Rectangle(ellipse.X, ellipse.Y, width, height));
                ellipseCount++;
            });
        }
    }
}
