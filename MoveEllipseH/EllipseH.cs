using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoveEllipseH
{
    public partial class EllipseH : Form
    {
        const int DIA = 30;
        int x = 0;
        int y = 0;
        int w = DIA;
        int h = DIA;
        int M_VAL = 10;
        int directionX = 1;
        int directionY = 1;
        Timer timer = new Timer();

        int score = 0;
        string scoreStr = "" + 0;

        int blockX = 0;
        int blockY = 0;

        public EllipseH()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Paint += EllipseH_Paint;
            timer.Interval = 10;
            timer.Tick += Timer_Tick;
            timer.Start();
            this.KeyDown += EllipseH_KeyDown;
            this.blockX = this.ClientRectangle.Left + 10;
            this.blockY = this.ClientRectangle.Bottom - 20;
        }

        private void EllipseH_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    this.blockY -= 10;
                    break;
                case Keys.Down:
                    this.blockY += 10;
                    break;
                case Keys.Left:
                    this.blockX -= 10;
                    break;
                case Keys.Right:
                    this.blockX += 10;
                    break;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.x += M_VAL * directionX;
            this.y += M_VAL * directionY;
            if (this.x >= this.ClientRectangle.Right - w || this.x <= this.ClientRectangle.Left)
            {
                directionX *= -1;
            }

            if (this.x >= this.blockX && this.x <= this.blockX + 100)
            {
                if (this.y <= this.blockY)
                {
                    if (this.y + DIA >= this.blockY)
                    {
                        this.score += 1;
                        this.scoreStr = "" + this.score;
                        directionY *= -1;
                    }
                }
            }

            if (this.y >= this.ClientRectangle.Bottom - h || this.y <= this.ClientRectangle.Top)
            {
                if (this.y >= this.ClientRectangle.Bottom - h)
                {
                    this.score -= 20;
                    this.scoreStr = "" + this.score;
                }
                directionY *= -1;
            }

            Invalidate();
        }

        private void EllipseH_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rc = new Rectangle(x, y, w, h);
            Rectangle block = new Rectangle(blockX, blockY, 100, 10);

            g.FillEllipse(Brushes.Red, rc);
            g.FillRectangle(Brushes.Yellow, block);
            Font font = new Font("Consolas", 20);
            g.DrawString(scoreStr, font, Brushes.Gold, this.ClientRectangle.Right - 100, this.ClientRectangle.Top + 20);
        }

        private void EllipseH_Load(object sender, EventArgs e)
        {
            
        }
    }
}
