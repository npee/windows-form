using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Omok
{
    public partial class BoardForm : Form
    {
        enum Phase
        {
            Black,
            White
        }
        const int DIA = 40;
        const int BOARD_ROW = 18;
        const int BOARD_COL = 18;
        Pen boardPen1 = new Pen(Brushes.Black);
        Pen boardPen2 = new Pen(Brushes.White);

        class Stone
        {
            public Rectangle StoneRect { get; set; }
            public Brush StoneBrush { get; set; }
            public Pen StonePen { get; set; }

            public Stone(int x, int y)
            {
                this.StoneRect = new Rectangle(x, y, DIA, DIA);
            }

            public Stone(Stone stone, Brush brush, Brush penColor)
            {
                this.StoneRect = stone.StoneRect;
                this.StoneBrush = brush;
                this.StonePen = new Pen(penColor);
            }
        }

        List<Stone> stones = new List<Stone>();
        Phase phase = Phase.Black;
        bool state = true;

        public BoardForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.BackColor = Color.Orange;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.ClientSize = new Size(760, 760);
            this.Paint += BoardForm_Paint;
            this.MouseClick += BoardForm_MouseClick;
        }

        private void BoardForm_MouseClick(object sender, MouseEventArgs e)
        {
            Graphics g = CreateGraphics();
            bool isExists = false;
            int x = e.X / DIA * DIA;
            int y = e.Y / DIA * DIA;
            
            Stone tempStone = new Stone(x, y);
            stones.ForEach(stone =>
            {
                if (stone.StoneRect.Contains(x, y))
                {
                    MessageBox.Show("Stone already exists.");
                    isExists = true;
                }
            });

            if (!isExists)
            {
                if (phase == Phase.Black)
                {
                    tempStone = new Stone(tempStone, Brushes.Black, Brushes.White);
                    stones.Add(tempStone);
                    Console.WriteLine("Judgement for Black");
                    judge();
                    phase = Phase.White;
                }
                else
                {
                    tempStone = new Stone(tempStone, Brushes.White, Brushes.Black);
                    stones.Add(tempStone);
                    phase = Phase.Black;
                }
            }

            stones.ForEach(stone =>
            {
                g.FillEllipse(stone.StoneBrush, stone.StoneRect);
                g.DrawEllipse(stone.StonePen, stone.StoneRect);
            });

            

            g.Dispose();
        }

        public void judge()
        {
            List<Stone> checkList;
            int connection = 0;
            Stone temp = null;
            List<Stone> blacks = stones.FindAll(stone => stone.StoneBrush == Brushes.Black);
            for (int i = 0; i < BOARD_COL; i++)
            {
                checkList = blacks.FindAll(stone => stone.StoneRect.X / 40 == i);
                if (checkList.Count < 5)
                    continue;

                checkList.Sort((prev, next) => prev.StoneRect.Y.CompareTo(next.StoneRect.Y));
                temp = checkList[0];
                foreach(Stone stone in checkList)
                {
                    if (stone.StoneRect.Y / 40 == temp.StoneRect.Y / 40 + 1)
                    {
                        connection++;
                        if (connection == 4)
                        {
                            MessageBox.Show("checked");
                            // state == false;
                        }
                    } else
                    {
                        connection = 0;
                    }
                    Console.WriteLine("X: " + i + " -> " + "Y: " + stone.StoneRect.Y / 40);
                    temp = stone;
                }
                connection = 0;
            }
            
            // blacks.ForEach(black => Console.WriteLine("Black X: " + black.StoneRect.X/40 + ", Black Y: " + black.StoneRect.Y/40));


            // List<Stone> whites = stones.FindAll(stone => stone.StoneBrush == Brushes.White);
            // MessageBox.Show("blacks: " + blacks.Count + ", whites: " + whites.Count);
            // blacks.ForEach(stone => )

        }

        private void BoardForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int i, j;
            for (i = 0; i < BOARD_ROW; i++)
            {
                for (j = 0; j < BOARD_COL; j++)
                {
                    Rectangle blackEdge = new Rectangle(i * DIA + 20, j * DIA + 20, DIA, DIA);
                    Rectangle whiteEdge = new Rectangle(i * DIA + 21, j * DIA + 21, DIA, DIA);
                    g.DrawRectangle(boardPen1, blackEdge);
                    g.DrawRectangle(boardPen2, whiteEdge);
                }
            }

            for (int k = 3; k < BOARD_ROW; k += 6)
            {
                for (int l = 3; l < BOARD_COL; l += 6)
                {
                    g.FillEllipse(Brushes.Black, k * DIA + 17, l * DIA + 17, 6, 6);
                }
            }
        }
    }
}
