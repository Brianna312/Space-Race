using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Media;

namespace Space_Race
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Rectangle Player1 = new Rectangle(100, 510, 25, 45);
        Rectangle Player2 = new Rectangle(655, 510, 25, 45);

        Rectangle Top = new Rectangle(-10, -10, 800, 20);

        List<int> AsteroidYList = new List<int>(new int [15]);
        List<int> asteroidSpeed = new List<int>(new int [15]);
        List<Rectangle> AsteroidList = new List<Rectangle>(new Rectangle[15]);

        List<int> AsteroidbYList = new List<int>(new int [15]);
        List<int> asteroidbSpeed = new List<int>( new int [15]);
        List<Rectangle> AsteroidbList = new List<Rectangle>(new Rectangle[15]);

        int asteroidXSize = 8;
        int asteroidYSize = 4;

        bool wDown = false;
        bool sDown = false;
        bool upDown = false;
        bool downDown = false;

        bool spaceDown = false;

        bool asteroidVisible = true;

        string gameState = "title";

        int p1Score = 0;
        int p2Score = 0;

        int flashy = 0;

        Random random = new Random();

        Brush greyBrush = new SolidBrush(Color.Gray);
        Brush blackBrush = new SolidBrush(Color.Black);
        Brush orangeBrush = new SolidBrush(Color.Orange);

        Pen greyPen = new Pen(Color.Gray, 2);
        Pen greenPen = new Pen(Color.Green, 2);
        Pen redPen = new Pen(Color.Red, 2);
        private void GameEngine_Tick(object sender, EventArgs e)
        {
            if (gameState == "playing")
            {
                //moving the asteroids
                for (int i = 0; i < AsteroidList.Count(); i++)
                {
                    int a = AsteroidList[i].X + asteroidSpeed[i];
                    AsteroidList[i] = new Rectangle(a, AsteroidYList[i], asteroidXSize, asteroidYSize);

                    int b = AsteroidbList[i].X + asteroidbSpeed[i];
                    AsteroidbList[i] = new Rectangle(b, AsteroidbYList[i], asteroidXSize, asteroidYSize);

                    //asteroid at wall? move to beg randomize speed
                    if (AsteroidList[i].X >= 805)
                    {
                        a = -50;
                        asteroidSpeed[i] = random.Next(2,8);
                        AsteroidYList[i] = random.Next(80, 500);
                        AsteroidList[i] = new Rectangle(a, AsteroidYList[i], asteroidXSize, asteroidYSize);                        
                    }
                    if (AsteroidbList[i].X <= -13)
                    {
                        b = 850;
                        asteroidbSpeed[i] = random.Next(-8, -2);
                        AsteroidbYList[i] = random.Next(80, 500);
                        AsteroidbList[i] = new Rectangle(b, AsteroidbYList[i], asteroidXSize, asteroidYSize);
                    }
                }

                //did rocket hit asteroid? Did it hit the edge? Did a player reach the top?
                for (int i = 0; i < AsteroidList.Count(); i++)
                {
                    if (AsteroidList[i].IntersectsWith(Player1) || AsteroidbList[i].IntersectsWith(Player1))
                    {
                        SoundPlayer sound = new SoundPlayer(Properties.Resources.crash);
                        sound.Play();
                        Player1.Y = 510;
                    }
                    else if (Player1.IntersectsWith(Top))
                    {
                        SoundPlayer sound = new SoundPlayer(Properties.Resources.point);
                        sound.Play();
                        p1Score++;
                        Player1.Y = 510;
                        Player2.Y = 510;
                        PointScored();
                        subtitleLabel.Visible = false;
                    }
                    if (AsteroidList[i].IntersectsWith(Player2) || AsteroidbList[i].IntersectsWith(Player2))
                    {
                        SoundPlayer sound = new SoundPlayer(Properties.Resources.crash);
                        sound.Play();
                        Player2.Y = 510;
                    }
                    else if (Player2.IntersectsWith(Top))
                    {
                        SoundPlayer sound = new SoundPlayer(Properties.Resources.point);
                        sound.Play();
                        p2Score++;
                        Player1.Y = 510;
                        Player2.Y = 510;

                        PointScored();
                        subtitleLabel.Visible = false;
                    }
                }

                //Moving the players
                if (wDown == true)
                {
                    Player1.Y -= 5;
                }
                else if (sDown == true)
                {
                    Player1.Y += 5;
                }
                if (upDown == true)
                {
                    Player2.Y -= 5;
                }
                else if (downDown == true)
                {
                    Player2.Y += 5;
                }
                Refresh();
            }
            else if (gameState == "win")
            {
                subtitleLabel.Visible = true; 
                p1Score = 0;
                p2Score = 0;
                if (spaceDown == true)
                {
                    p1ScoreLabel.Text = $"{p1Score}";
                    p2ScoreLabel.Text = $"{p2Score}";
                    titleLabel.Text = "Space Race!";
                    titleLabel.Location = new Point(this.Width / 2 - titleLabel.Width / 2, 225);
                    Countdown(true);
                    titleLabel.Visible = false;
                    subtitleLabel.Visible = false;

                }
                if (flashy > 0 && flashy % 20 == 0)
                {
                    if (subtitleLabel.ForeColor == Color.White)
                    {
                        subtitleLabel.ForeColor = Color.Black;
                    }
                    else
                    {
                        subtitleLabel.ForeColor = Color.White;
                    }
                    flashy = 0;
                }
                else
                {
                    flashy++;
                }
            }
            else if (gameState == "title")
            {
                titleLabel.Location = new Point(this.Width / 2 - titleLabel.Width / 2, 225);
                subtitleLabel.Location = new Point(this.Width / 2 - subtitleLabel.Width / 2, 275);
                if (spaceDown == true)
                {                    
                    Countdown(true);
                    titleLabel.Visible = false;
                    subtitleLabel.Visible = false;
                }
                if (flashy>0 && flashy%15 == 0)
                {
                    if (subtitleLabel.ForeColor == Color.White)
                    {
                        subtitleLabel.ForeColor = Color.Black;
                    }
                    else
                    {
                        subtitleLabel.ForeColor = Color.White;
                    }
                    flashy = 0;
                }
                else 
                {
                    flashy++;
                }              

            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;

                case Keys.Up:
                    upDown = true;
                    break;
                case Keys.Down:
                    downDown = true;
                    break;

                case Keys.Space:
                    spaceDown = true;
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;

                case Keys.Up:
                    upDown = false;
                    break;
                case Keys.Down:
                    downDown = false;
                    break;

                case Keys.Space:
                    spaceDown = false;
                    break;

            }
        }
        public void Countdown (bool countingDown)
        {
            if (countingDown == true)
            {                
                subtitleLabel.ForeColor = Color.White;
                subtitleLabel.Visible = true;
                subtitleLabel.Text = "3";
                subtitleLabel.Location = new Point(this.Width / 2 - subtitleLabel.Height / 2, 275);
                Thread.Sleep(1000);
                Refresh();
                SoundPlayer sound = new SoundPlayer(Properties.Resources._8NSLV3Z_count_down);
                sound.Play();
                subtitleLabel.Text = "2";
                subtitleLabel.Location = new Point(this.Width / 2 - subtitleLabel.Height / 2, 275);
                Thread.Sleep(1000);
                Refresh();
                subtitleLabel.Text = "1";
                subtitleLabel.Location = new Point(this.Width / 2 - subtitleLabel.Height / 2, 275);
                Thread.Sleep(1000);
                Refresh();
                subtitleLabel.Text = "GO!";
                subtitleLabel.Location = new Point(this.Width / 2 - subtitleLabel.Height / 2, 275);
                for (int i = 0; i < AsteroidList.Count(); i++)
                {
                    AsteroidYList[i] = random.Next(80, 500);
                    int a = random.Next(0, 800);
                    AsteroidList[i] = new Rectangle(a, AsteroidYList[i], 40, 40);
                    asteroidSpeed[i] = random.Next(1,8);

                    AsteroidbYList[i] = random.Next(80, 500);
                    int b = random.Next(0, 800);
                    AsteroidbList[i] = new Rectangle(a, AsteroidYList[i], 40, 40);
                    asteroidbSpeed[i] = random.Next(-8, -1);
                }
                gameState = "playing";
                asteroidVisible = true;
                Thread.Sleep(900);
                Refresh();
            }
        }

        public void PointScored()
        {
            
            asteroidVisible = false;
            p1ScoreLabel.Text = $"{p1Score}";
            p2ScoreLabel.Text = $"{p2Score}";
            if (p1Score == 3)
            {
                titleLabel.Visible = true;
                gameState = "win";
                titleLabel.Text = "Player 1 Wins!";
                subtitleLabel.Text = "Press SPACE to play again";
                titleLabel.Location = new Point(this.Width / 2 - titleLabel.Width / 2, 225);
                subtitleLabel.Location = new Point(this.Width / 2 - subtitleLabel.Width / 2, 275);
            }
            else if (p2Score == 3)
            {
                titleLabel.Visible = true;
                subtitleLabel.Visible = true;
                gameState = "win";
                titleLabel.Text = "Player 2 Wins!";
                subtitleLabel.Text = "Press SPACE to play again";
                titleLabel.Location = new Point(this.Width / 2 - titleLabel.Width / 2, 225);
                subtitleLabel.Location = new Point(this.Width / 2 - subtitleLabel.Width / 2, 275);
            }
            else
            {
                Countdown(true);
            }        
            
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (asteroidVisible == true)
            {
                for (int i = 0; i < AsteroidList.Count(); i++)
                {
                    e.Graphics.FillRectangle(greyBrush, AsteroidList[i]);
                    e.Graphics.FillRectangle(greyBrush, AsteroidbList[i]);
                }
            }
            else
            {
                for (int i = 0; i < AsteroidList.Count(); i++)
                {
                    e.Graphics.FillRectangle(blackBrush, AsteroidList[i]);
                    e.Graphics.FillRectangle(blackBrush, AsteroidbList[i]);
                }
            }
            //drawing players
            //Flames from engines
            if (wDown == true)
            {
                e.Graphics.FillEllipse(orangeBrush, Player1.X + 1, Player1.Y + 39, 8, 8);
                e.Graphics.DrawEllipse(redPen, Player1.X + 1, Player1.Y + 39, 8, 8);

                e.Graphics.FillEllipse(orangeBrush, Player1.X + Player1.Width - 10, Player1.Y + 39, 8, 8);
                e.Graphics.DrawEllipse(redPen, Player1.X + Player1.Width - 10, Player1.Y + 39, 8, 8);
            }

            //left side
            e.Graphics.DrawLine(greyPen, Player1.X + 2, Player1.Y + 12, Player1.X + Player1.Width / 2, Player1.Y);
            e.Graphics.DrawArc(greyPen, Player1.X + 2, Player1.Y + 8, 5, 25, -90, -185);
            e.Graphics.DrawLine(greyPen, Player1.X + 5, Player1.Y + 30, Player1.X, Player1.Y + 40);
            e.Graphics.DrawLine(greyPen, Player1.X, Player1.Y + 40, Player1.X + 10, Player1.Y + 40);
            e.Graphics.DrawLine(greyPen, Player1.X + 10, Player1.Y + 40, Player1.X + Player1.Width / 2, Player1.Y + 32);
            //right side
            e.Graphics.DrawLine(greyPen, Player1.X + Player1.Width - 4, Player1.Y + 12, Player1.X + Player1.Width / 2, Player1.Y);
            e.Graphics.DrawArc(greyPen, Player1.X + Player1.Width - 9, Player1.Y + 8, 5, 25, -90, 185);

            e.Graphics.DrawLine(greyPen, Player1.X + Player1.Width - 6, Player1.Y + 30, Player1.X + Player1.Width - 1, Player1.Y + 40);
            e.Graphics.DrawLine(greyPen, Player1.X + Player1.Width - 1, Player1.Y + 40, Player1.X + Player1.Width - 11, Player1.Y + 40);
            e.Graphics.DrawLine(greyPen, Player1.X + Player1.Width - 11, Player1.Y + 40, Player1.X + Player1.Width / 2, Player1.Y + 32);
            //central
            e.Graphics.DrawArc(greyPen, Player1.X + 2, Player1.Y + 10, 18, 6, 5, 185);
            e.Graphics.FillEllipse(greyBrush, Player1.X + Player1.Width / 2 - 3, Player1.Y + 20, 6, 6);

            //Player 2
            //Flames from engines
            if (upDown == true)
            {
                e.Graphics.FillEllipse(orangeBrush, Player2.X + 1, Player2.Y + 39, 8, 8);
                e.Graphics.DrawEllipse(redPen, Player2.X + 1, Player2.Y + 39, 8, 8);

                e.Graphics.FillEllipse(orangeBrush, Player2.X + Player2.Width - 10, Player2.Y + 39, 8, 8);
                e.Graphics.DrawEllipse(redPen, Player2.X + Player2.Width - 10, Player2.Y + 39, 8, 8);
            }

            //left side
            e.Graphics.DrawLine(greyPen, Player2.X + 2, Player2.Y + 12, Player2.X + Player2.Width / 2, Player2.Y);
            e.Graphics.DrawArc(greyPen, Player2.X + 2, Player2.Y + 8, 5, 25, -90, -185);
            e.Graphics.DrawLine(greyPen, Player2.X + 5, Player2.Y + 30, Player2.X, Player2.Y + 40);
            e.Graphics.DrawLine(greyPen, Player2.X, Player2.Y + 40, Player2.X + 10, Player2.Y + 40);
            e.Graphics.DrawLine(greyPen, Player2.X + 10, Player2.Y + 40, Player2.X + Player1.Width / 2, Player2.Y + 32);
            //right side
            e.Graphics.DrawLine(greyPen, Player2.X + Player2.Width - 4, Player2.Y + 12, Player2.X + Player1.Width / 2, Player2.Y);
            e.Graphics.DrawArc(greyPen, Player2.X + Player2.Width - 9, Player2.Y + 8, 5, 25, -90, 185);

            e.Graphics.DrawLine(greyPen, Player2.X + Player2.Width - 6, Player2.Y + 30, Player2.X + Player1.Width - 1, Player2.Y + 40);
            e.Graphics.DrawLine(greyPen, Player2.X + Player2.Width - 1, Player2.Y + 40, Player2.X + Player1.Width - 11, Player2.Y + 40);
            e.Graphics.DrawLine(greyPen, Player2.X + Player2.Width - 11, Player2.Y + 40, Player2.X + Player1.Width / 2, Player2.Y + 32);
            //central
            e.Graphics.DrawArc(greyPen, Player2.X + 2, Player2.Y + 10, 18, 6, 5, 185);
            e.Graphics.FillEllipse(greyBrush, Player2.X + Player2.Width / 2 - 3, Player2.Y + 20, 6, 6);

            //hitting asteroid explosion
            for (int i = 0; i < AsteroidList.Count(); i++)
            {
                if (Player1.IntersectsWith(AsteroidList[i]))
                {
                    e.Graphics.FillEllipse(orangeBrush, AsteroidList[i].X + 1, AsteroidList[i].Y - 1, 8, 8);
                    e.Graphics.DrawEllipse(redPen, AsteroidList[i].X + 1, AsteroidList[i].Y - 1, 8, 8);
                }
                else if (Player1.IntersectsWith(AsteroidbList[i]))
                {
                    e.Graphics.FillEllipse(orangeBrush, AsteroidbList[i].X + 1, AsteroidbList[i].Y - 1, 8, 8);
                    e.Graphics.DrawEllipse(redPen, AsteroidbList[i].X + 1, AsteroidbList[i].Y - 1, 8, 8);
                }


                if (Player2.IntersectsWith(AsteroidList[i]))
                {
                    e.Graphics.FillEllipse(orangeBrush, AsteroidList[i].X + 1, AsteroidList[i].Y - 1, 8, 8);
                    e.Graphics.DrawEllipse(redPen, AsteroidList[i].X + 1, AsteroidList[i].Y - 1, 8, 8);
                }
                else if (Player2.IntersectsWith(AsteroidbList[i]))
                {
                    e.Graphics.FillEllipse(orangeBrush, AsteroidbList[i].X + 1, AsteroidbList[i].Y - 1, 8, 8);
                    e.Graphics.DrawEllipse(redPen, AsteroidbList[i].X + 1, AsteroidbList[i].Y - 1, 8, 8);
                }
            }
        }
        
    }
}
