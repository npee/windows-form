using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class SnakeBoard : Form
    {
        const float DIA = 20;

        public enum FencePosition
        {
            Top, 
            Bottom,
            Left,
            Right
        }

        class Fence
        {
            public RectangleF rect { get; set; }
            public static Pen edgePen = new Pen(Brushes.White, 1);

            public Fence(FencePosition pos, int i)
            {
                if (pos == FencePosition.Top)
                {
                    this.rect = new RectangleF(i * DIA, 0, DIA, DIA);
                }
                else if (pos == FencePosition.Bottom)
                {
                    this.rect = new RectangleF(i * DIA, DIA * 39, DIA, DIA);
                }
                else if (pos == FencePosition.Left)
                {
                    this.rect = new RectangleF(0, i * DIA, DIA, DIA);
                }
                else if (pos == FencePosition.Right)
                {
                    this.rect = new RectangleF(DIA * 39, i * DIA, DIA, DIA);
                }
            }
        }
        
        class Food {
            public RectangleF rect { get; set; }
            public static Brush foodColor = Brushes.Red;

            public Food(int x, int y)
            {
                this.rect = new RectangleF(x * DIA, y * DIA, DIA, DIA);
            }

            public float getX()
            {
                return this.rect.X / DIA;
            }

            public float getY()
            {
                return this.rect.Y / DIA;
            }
        }

        class Snake
        {
            public RectangleF rect { get; set; }
            public static Pen skinColorPen = new Pen(Brushes.Gold, 4);
            public static Brush bodyColorBrush = Brushes.Blue;

            public Snake(int x, int y)
            {
                this.rect = new RectangleF(x * DIA, y * DIA, DIA-2, DIA-2);
            }

            public float getX()
            {
                return this.rect.X / DIA;
            }

            public float getY()
            {
                return this.rect.Y / DIA;
            }
        }

        Timer timer = new Timer();
        RectangleF[] fences = new RectangleF[40];
        List<Food> feedBowl = new List<Food>();
        Random foodPosition = new Random();
        LinkedList<Snake> snake = new LinkedList<Snake>();
        Snake head = null;

        int dirX = 0;
        int dirY = 1;

        int headX = 0;
        int headY = 0;

        int score = 0;

        public SnakeBoard()
        {
            this.DoubleBuffered = true;
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.ClientSize = new Size(800, 800);
            this.BackColor = Color.Black;
            this.Paint += SnakeBoard_Paint;
            this.KeyDown += SnakeBoard_KeyDown;
            timer.Interval = 100;
            timer.Tick += Timer_Tick;
            timer.Start();

            // Initializing Snake
            head = new Snake(3, 7);
            snake.AddFirst(head);
            snake.AddLast(new Snake(3, 6));
            snake.AddLast(new Snake(3, 5));
            snake.AddLast(new Snake(3, 4));
            snake.AddLast(new Snake(3, 3));
        }

        private void Timer_Tick(object sender, EventArgs e)
        { 
            headX = (int)snake.First.Value.getX();
            headY = (int)snake.First.Value.getY();

            // Basic code for movement
            // snake.AddFirst(new Snake(headX + dirX, headY + dirY));
            // snake.RemoveLast();

            // Move
            // Make new head
            head = new Snake(headX + dirX, headY + dirY);
            
            // Snake cannot escape from this fence
            if (head.getX() < 1  || head.getX() > 38)
            {
                head = new Snake(headX, headY + dirY);
            } else if (head.getY() < 1 || head.getY() > 38)
            {
                head = new Snake(headX + dirX, headY);
            }

            // Add new head
            snake.AddFirst(head);

            // Action when snake eats
            Food eatenFood = null;
            feedBowl.ForEach(food => {
                eatenFood = feedBowl.Find(eaten => headX == (int)eaten.getX() && headY == (int)eaten.getY());
            });
            if (!(eatenFood == null))
            {
                feedBowl.Remove(eatenFood);
                score++;
                // If the snake eats food,
                // remaind old tail.
                // snake.RemoveLast();
            }
            else
            {
                // Split old tail
                snake.RemoveLast();
            }

            Invalidate();
        }

        private void SnakeBoard_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Up:
                    dirX = 0; dirY = -1;
                    break;
                case Keys.Down:
                    dirX = 0; dirY = 1;
                    break;
                case Keys.Left:
                    dirX = -1; dirY = 0;
                    break;
                case Keys.Right:
                    dirX = 1; dirY = 0;
                    break;
            }
        }

        private void SnakeBoard_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Font font = new Font("Consolas", 50);

            // Make fences
            for (FencePosition pos = FencePosition.Top; pos <= FencePosition.Right; pos++)
            {
                for (int i = 0; i < 40; i++)
                {
                    fences[i] = new Fence(pos, i).rect;
                }
                g.DrawRectangles(Fence.edgePen, fences);

            }

            // Feed
            if (feedBowl.Count == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    int posX = foodPosition.Next(1, 39);
                    int posY = foodPosition.Next(1, 39);
                    while (posX == posY)
                    {
                        posY = foodPosition.Next(1, 39);
                    }

                    feedBowl.Add(new Food(posX, posY));
                }
            }
            foreach (Food food in feedBowl)
            {
                g.FillRectangle(Food.foodColor, food.rect);
            }

            // Draw snake
            foreach (Snake body in snake)
            {
                g.DrawRectangle(Snake.skinColorPen, body.rect.X, body.rect.Y, body.rect.Width, body.rect.Height);
                g.FillRectangle(Snake.bodyColorBrush, body.rect);
            }

            // View score
            g.DrawString("" + score, font, Brushes.Gold, this.ClientRectangle.Right / 2, this.ClientRectangle.Bottom / 2);

        }
    }
}
