using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
namespace Snake_V1._3
{
    public partial class Form1 : Form
    {
        SoundPlayer musicEat = new SoundPlayer("Eat.wav");
        SoundPlayer musicGameOver = new SoundPlayer("GameOver.wav");

        bool up = false, down = false, right = false, left = false;
        Label[] body = new Label[100];
        Label head = new Label();
        Label food = new Label();
        int count_Tail = 0, score = 0, level = 1, x = 1, game = 1;

        public Form1()
        {
            InitializeComponent();

            Random rand = new Random();

            //put head and food in panel
            //**********************************************************************
            head.BackColor = Color.Black;
            head.Size = new Size(15, 15);
            head.Location = new Point(rand.Next(0, 420), rand.Next(0, 375));
            while (head.Location.X % 15 != 0 || head.Location.Y % 15 != 0)
                head.Location = new Point(rand.Next(0, 420), rand.Next(0, 375));
            body[count_Tail] = head;
            panel.Controls.Add(head);            

            food.BackColor = Color.DarkBlue;
            food.Size = head.Size;
            food.Location = new Point(rand.Next(0, 420), rand.Next(0, 375));
            while (food.Location.X % 15 != 0 || food.Location.Y % 15 != 0)
                food.Location = new Point(rand.Next(0, 420), rand.Next(0, 375));
            panel.Controls.Add(food);            
            //**********************************************************************

            labelLevel.Text = Convert.ToString(level);
            labelScore.Text = Convert.ToString(score);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //set location of the snake's body
            for (int i = count_Tail; i > 0; i--)
                body[i].Location = body[i - 1].Location;

            Move_Snake();
            Eat();
            Check_For_Level();

            //when the snake eat itself
            for (int i = count_Tail; i > 1; i--)
            {
                if (head.Location == body[i - 1].Location)
                {
                    for (int j = count_Tail; j > 0; j--)
                        panel.Controls.Remove(body[j]);

                    timer.Stop();
                    musicGameOver.Play();
                    DialogResult gameOver = MessageBox.Show("GAME OVER", ":)", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    if (gameOver == DialogResult.OK)
                    {
                        if (game == 6 || game == 11 ||game == 16)
                            labelReport.Text = "";
                        labelReport.Text += $"      Game {game}\nScore: {score}\nLevel: {level}\n----------------\n";
                        game++;
                        count_Tail = 0;
                        labelScore.Text = "0";
                        score = 0;
                        labelLevel.Text = "1";
                        level = 1;
                        x = 1;
                        timer.Interval = 120;
                        up = false; down = false; right = false; left = false;
                    }
                    break;
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {            
            timer.Start();
            switch (e.KeyCode)
            {
                case Keys.Up:
                    {
                        if (down == false)
                        {
                            up = true;
                            down = false;
                            right = false;
                            left = false;
                        }
                        break;
                    }
                case Keys.Down:
                    {
                        if (up == false)
                        {
                            up = false;
                            down = true;
                            left = false;
                            right = false;
                        }
                        break;
                    }
                case Keys.Left:
                    {
                        if (right == false)
                        {
                            up = false;
                            down = false;
                            left = true;
                            right = false;
                        }
                        break;
                    }
                case Keys.Right:
                    {
                        if (left == false)
                        {
                            up = false;
                            down = false;
                            left = false;
                            right = true;
                        }
                        break;
                    }

                //this case is for exit of program
                case Keys.Escape:
                    {
                        timer.Stop();
                        DialogResult exit_Program;
                        exit_Program = MessageBox.Show("Do you want to exit??", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                        if (exit_Program == DialogResult.OK)
                            Application.Exit();
                        else
                            timer.Start();
                        break;
                    }

                //this case is for reset all information in labelReport
                case Keys.R:
                    {
                        timer.Stop();
                        DialogResult gameOver = MessageBox.Show("Are you sure ??", "O_o", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                        if (gameOver == DialogResult.OK)
                        {
                            for (int j = count_Tail; j > 0; j--)
                                panel.Controls.Remove(body[j]);

                            game = 1;
                            labelReport.Text = "";
                            count_Tail = 0;
                            labelScore.Text = "0";
                            score = 0;
                            labelLevel.Text = "1";
                            level = 1;
                            x = 1;
                            timer.Interval = 120;
                        }                        
                        break;
                    }

                //this case is for stop the game
                case Keys.Space:
                    {
                        timer.Stop();
                        DialogResult stop = MessageBox.Show("The game was stoped !!\nPlay Game ??",
                                                            "Stop Game",
                                                            MessageBoxButtons.OK,
                                                            MessageBoxIcon.Warning);                        
                        break;
                    }                
            }
        }        

        private void Move_Snake()
        {
            //the snake move as much as head size each timer tick

            if (up)
            {
                int u = head.Location.Y - head.Height;
                head.Location = new Point(head.Location.X, u);
            }

            else if (down)
            {
                int d = head.Location.Y + head.Height;
                head.Location = new Point(head.Location.X, d);
            }

            else if (right)
            {
                int r = head.Location.X + head.Width;
                head.Location = new Point(r, head.Location.Y);
            }

            else if (left)
            {
                int l = head.Location.X - head.Width;
                head.Location = new Point(l, head.Location.Y);
            }

            //the way of snake move in two level
            switch (level)
            {
                //in these case the snake can across the wall
                case 1:
                case 2:
                case 3:
                case 4:
                    {
                        if (head.Location.X > panel.Width)
                            head.Location = new Point(0, head.Location.Y);

                        else if (head.Location.X < 0)
                            head.Location = new Point(panel.Width - head.Width, head.Location.Y);

                        else if (head.Location.Y > panel.Height)
                            head.Location = new Point(head.Location.X, 0);

                        else if (head.Location.Y < 0)
                            head.Location = new Point(head.Location.X, panel.Height - head.Height);
                        break;
                    }

                //in these case the snake can't across the wall                
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    {
                        if (head.Location.X > panel.Width)
                        {
                            //the whole snake body will be remove
                            for (int i = count_Tail; i > 0; i--)
                                panel.Controls.Remove(body[i]);

                            musicGameOver.Play();
                            timer.Stop();
                            DialogResult gameOver = MessageBox.Show("GAME OVER", ":)", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            if (gameOver == DialogResult.OK)
                            {
                                //when labelReport is full it will be empty
                                //**************************
                                if (game == 6)
                                    labelReport.Text = "";
                                //**************************

                                //the information print on labelReport and setting will be reset
                                labelReport.Text += $"      Game {game}\nScore: {score}\nLevel: {level}\n----------------\n";
                                game++;
                                count_Tail = 0;
                                labelScore.Text = "0";
                                score = 0;
                                labelLevel.Text = "1";
                                level = 1;
                                x = 1;
                                timer.Interval = 120;
                                head.Location = new Point(5, head.Location.Y);
                                up = false; down = false; right = false; left = false;
                            }
                        }

                        else if (head.Location.X < 0)
                        {
                            for (int i = count_Tail; i > 0; i--)
                                panel.Controls.Remove(body[i]);

                            timer.Stop();
                            musicGameOver.Play();
                            DialogResult gameOver = MessageBox.Show("GAME OVER", ":)", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            if (gameOver == DialogResult.OK)
                            {
                                if (game == 6)
                                    labelReport.Text = "";
                                labelReport.Text += $"      Game {game}\nScore: {score}\nLevel: {level}\n----------------\n";
                                game++;
                                count_Tail = 0;
                                labelScore.Text = "0";
                                score = 0;
                                labelLevel.Text = "1";
                                level = 1;
                                x = 1;
                                timer.Interval = 120;
                                head.Location = new Point(panel.Width - 20, head.Location.Y);
                                up = false; down = false; right = false; left = false;
                            }
                        }

                        else if (head.Location.Y > panel.Height)
                        {
                            for (int i = count_Tail; i > 0; i--)
                                panel.Controls.Remove(body[i]);

                            timer.Stop();
                            musicGameOver.Play();
                            DialogResult gameOver = MessageBox.Show("GAME OVER", ":)", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            if (gameOver == DialogResult.OK)
                            {
                                if (game == 6)
                                    labelReport.Text = "";
                                labelReport.Text += $"      Game {game}\nScore: {score}\nLevel: {level}\n----------------\n";
                                game++;
                                count_Tail = 0;
                                labelScore.Text = "0";
                                score = 0;
                                labelLevel.Text = "1";
                                level = 1;
                                x = 1;
                                timer.Interval = 120;
                                head.Location = new Point(head.Location.X, 5);
                                up = false; down = false; right = false; left = false;
                            }
                        }

                        else if (head.Location.Y < 0)
                        {
                            for (int i = count_Tail; i > 0; i--)
                                panel.Controls.Remove(body[i]);

                            timer.Stop();
                            musicGameOver.Play();
                            DialogResult gameOver = MessageBox.Show("GAME OVER", ":)", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            if (gameOver == DialogResult.OK)
                            {
                                if (game == 6)
                                    labelReport.Text = "";
                                labelReport.Text += $"      Game {game}\nScore: {score}\nLevel: {level}\n----------------\n";
                                game++;
                                count_Tail = 0;
                                labelScore.Text = "0";
                                score = 0;
                                labelLevel.Text = "1";
                                level = 1;
                                x = 1;
                                timer.Interval = 120;
                                head.Location = new Point(head.Location.X, panel.Height - 20);
                                up = false; down = false; right = false; left = false;
                            }
                        }
                        break;                        
                    }
            }
        }

        private void Create_Food()
        {
            Random rand = new Random();

            food.BackColor = Color.DarkBlue;
            food.Size = head.Size;

            food.Location = new Point(rand.Next(0, 420), rand.Next(0, 375));
            
            //Check that new food is in a correct position in the panel and it isn't on snake
            while (food.Location.X % 15 != 0 || food.Location.Y % 15 != 0)
            {
                food.Location = new Point(rand.Next(0, 420), rand.Next(0, 375));
                for (int i = count_Tail; i > 0; i--)
                {
                    if (body[i].Location.X == food.Location.X || body[i].Location.Y == food.Location.Y)
                    {
                        food.Location = new Point(rand.Next(0, 300), rand.Next(0, 300));
                        i = count_Tail;
                    }

                }
            }
            panel.Controls.Add(food);            
        }

        private void Eat()
        {            
            if (head.Location.X == food.Location.X)            
                if (head.Location.Y == food.Location.Y)
                {
                    musicEat.Play();
                    labelScore.Text = Convert.ToString(++score);
                    count_Tail++;
                    Add_Tail();
                    Controls.Remove(food);
                    Create_Food();
                }            
        }

        private void Add_Tail()
        {
            Label tail = new Label();
            tail.Location = head.Location;
            body[count_Tail] = tail;
            body[count_Tail].BackColor = Color.FromArgb(255, 100, 100);
            body[count_Tail].Size = head.Size;
            body[count_Tail].Location = tail.Location;
            panel.Controls.Add(body[count_Tail]);
        }

        //Control levels
        private void Check_For_Level()
        {
            //each 5 score is one level
            if (count_Tail == (5 * x))
            {
                labelLevel.Text = Convert.ToString(++level);  
                
                //in level 5 it goes to next level
                if (x == 4)
                {
                    timer.Stop();
                    DialogResult nextLevel = MessageBox.Show($"Welcome to the next level\n" +
                                                             $"In this level you can't across the wall\n" +
                                                             $"!! BE CAREFUL !!",
                                                             "Next Level",
                                                             MessageBoxButtons.OKCancel,
                                                             MessageBoxIcon.Warning);                    
                }

                x++;
            }            

            //setting of each level(speed and color)
            switch (level)
            {
                case 1:
                    for (int i = count_Tail; i > 0; i--)
                    {
                        body[i].BackColor = Color.FromArgb(255, 100, 100);
                        timer.Interval = 120;
                    }
                    break;

                case 2:
                    for (int i = count_Tail; i > 0; i--)
                    {
                        body[i].BackColor = Color.FromArgb(255, 50, 50);
                        timer.Interval = 100;
                    }
                    break;

                case 3:
                    for (int i = count_Tail; i > 0; i--)
                    {
                        body[i].BackColor = Color.FromArgb(255, 10, 10);
                        timer.Interval = 90;
                    }
                    break;

                case 4:
                    for (int i = count_Tail; i > 0; i--)
                    {
                        body[i].BackColor = Color.FromArgb(250, 0, 0);
                        timer.Interval = 80;
                    }
                    break;

                case 5:
                    for (int i = count_Tail; i > 0; i--)
                    {
                        body[i].BackColor = Color.FromArgb(230, 0, 0);
                        timer.Interval = 70;
                    }
                    break;

                case 6:
                    for (int i = count_Tail; i > 0; i--)
                    {
                        body[i].BackColor = Color.FromArgb(210, 0, 0);
                        timer.Interval = 60;
                    }
                    break;

                case 7:
                    for (int i = count_Tail; i > 0; i--)
                    {
                        body[i].BackColor = Color.FromArgb(190, 0, 0);
                        timer.Interval = 50;
                    }
                    break;

                case 8:
                    for (int i = count_Tail; i > 0; i--)
                    {
                        body[i].BackColor = Color.FromArgb(170, 0, 0);
                        timer.Interval = 40;
                    }
                    break;

                case 9:
                    for (int i = count_Tail; i > 0; i--)
                    {
                        body[i].BackColor = Color.FromArgb(150, 0, 0);
                        timer.Interval = 30;
                    }
                    break;

                case 10:
                    for (int i = count_Tail; i > 0; i--)
                    {
                        body[i].BackColor = Color.FromArgb(130, 0, 0);
                        timer.Interval = 20;
                    }
                    break;

                default:
                    for (int i = count_Tail; i > 0; i--)
                    {
                        body[i].BackColor = Color.FromArgb(130, 50, 50);
                        timer.Interval = 20;
                    }
                    break;
            }
        }

        // Move Form by mouse
        //******************************************************************
        bool mouseDown;
        private Point lastLocation;        
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }        
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
                this.Location = new Point((this.Location.X - lastLocation.X) + e.X,
                                          (this.Location.Y - lastLocation.Y) + e.Y);
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
        //******************************************************************
    }
}
