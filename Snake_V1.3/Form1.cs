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

            labelLevel.Text = Convert.ToString(level);
            labelScore.Text = Convert.ToString(score);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //set location of the snake's body
            for (int i = count_Tail; i > 0; i--)
                body[i].Location = body[i - 1].Location;

            Move();

            Eat();
            Check_For_Level();

            //the snake eat itself
            for (int i = count_Tail; i > 1; i--)
            {
                if (head.Location == body[i - 1].Location)
                {
                    for (int j = count_Tail; j > 0; j--)
                        panel.Controls.Remove(body[j]);

                    timer.Stop();
                    musicGameOver.Play();
                    DialogResult gameOver = MessageBox.Show("شما باختید", ":)", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    if (gameOver == DialogResult.OK)
                    {
                        if (game == 5 || game == 9 ||game == 13)
                            labelReport.Text = "";
                        labelReport.Text += $"        بازی {game}\nامتیاز: {score}\nمرحله: {level}\n----------\n";
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
                case Keys.Escape:
                    {
                        timer.Stop();
                        DialogResult exit_Program = MessageBox.Show("آیا مطمئن هستید؟؟", "خروج",
                                                                    MessageBoxButtons.OKCancel,
                                                                    MessageBoxIcon.Warning,
                                                                    MessageBoxDefaultButton.Button2,
                                                                    MessageBoxOptions.RightAlign);

                        if (exit_Program == DialogResult.OK)
                            Application.Exit();
                        else
                            timer.Start();
                        break;
                    }
                case Keys.R:
                    {
                        timer.Stop();
                        DialogResult gameOver = MessageBox.Show("آیا مطمئن هستید؟؟", "نوسازی نتایج",
                                                                MessageBoxButtons.OKCancel,
                                                                MessageBoxIcon.Warning,
                                                                MessageBoxDefaultButton.Button1,
                                                                MessageBoxOptions.RightAlign);
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
                case Keys.Space:
                    {
                        timer.Stop();
                        DialogResult stop = MessageBox.Show("بازی متوقف شد\nادامه بازی؟؟",
                                                            "توقف بازی",
                                                            MessageBoxButtons.OK,
                                                            MessageBoxIcon.Warning,
                                                            MessageBoxDefaultButton.Button1,
                                                            MessageBoxOptions.RightAlign);                        
                        break;
                    }                
            }
        }        

        private void Move()
        {
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

            switch (level)
            {
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
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    {
                        if (head.Location.X > panel.Width)
                        {
                            for (int i = count_Tail; i > 0; i--)
                                panel.Controls.Remove(body[i]);

                            timer.Stop();
                            musicGameOver.Play();
                            DialogResult gameOver = MessageBox.Show("شما باختید", ":)", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            if (gameOver == DialogResult.OK)
                            {
                                if (game == 5 || game == 9 || game == 13)
                                    labelReport.Text = "";
                                labelReport.Text += $"        بازی {game}\nامتیاز: {score}\nمرحله: {level}\n----------\n";
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
                        }

                        else if (head.Location.X < 0)
                        {
                            for (int i = count_Tail; i > 0; i--)
                                panel.Controls.Remove(body[i]);

                            timer.Stop();
                            musicGameOver.Play();
                            DialogResult gameOver = MessageBox.Show("شما باختید", ":)", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            if (gameOver == DialogResult.OK)
                            {
                                if (game == 5 || game == 9 || game == 13)
                                    labelReport.Text = "";
                                labelReport.Text += $"        بازی {game}\nامتیاز: {score}\nمرحله: {level}\n----------\n";
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
                        }

                        else if (head.Location.Y > panel.Height)
                        {
                            for (int i = count_Tail; i > 0; i--)
                                panel.Controls.Remove(body[i]);

                            timer.Stop();
                            musicGameOver.Play();
                            DialogResult gameOver = MessageBox.Show("شما باختید", ":)", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            if (gameOver == DialogResult.OK)
                            {
                                if (game == 5 || game == 9 || game == 13)
                                    labelReport.Text = "";
                                labelReport.Text += $"        بازی {game}\nامتیاز: {score}\nمرحله: {level}\n----------\n";
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
                        }

                        else if (head.Location.Y < 0)
                        {
                            for (int i = count_Tail; i > 0; i--)
                                panel.Controls.Remove(body[i]);

                            timer.Stop();
                            musicGameOver.Play();
                            DialogResult gameOver = MessageBox.Show("شما باختید", ":)", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            if (gameOver == DialogResult.OK)
                            {
                                if (game == 5 || game == 9 || game == 13)
                                    labelReport.Text = "";
                                labelReport.Text += $"        بازی {game}\nامتیاز: {score}\nمرحله: {level}\n----------\n";
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
            //labelReport.Text += Convert.ToString(food.Location.X + " " + food.Location.Y) + "\n";


            //for (int i = count_Tail; i > 0; i--)
            //{
            //    if (body[i].Location.X + food.Width >= food.Location.X && body[i].Location.X <= food.Location.X + food.Width)
            //    {
            //        if (body[i].Location.Y + food.Height >= food.Location.Y && body[i].Location.Y <= food.Location.Y + food.Height)
            //        {
            //            i = count_Tail;
            //        }
            //    }
            //}
            panel.Controls.Add(food);
            //labelReport.Text += Convert.ToString(food.Location.X + " " + food.Location.Y + "\n");
        }

        private void Eat()
        {
            if (head.Location.X == food.Location.X)/*head.Location.X + food.Width >= food.Location.X && head.Location.X <= food.Location.X + food.Width*/
            {
                if (head.Location.Y == food.Location.Y)/*head.Location.Y + food.Height >= food.Location.Y && head.Location.Y <= food.Location.Y + food.Height*/
                {
                    musicEat.Play();
                    labelScore.Text = Convert.ToString(++score);
                    count_Tail++;
                    Add_Tail();
                    Controls.Remove(food);
                    Create_Food();
                }
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

        private void Check_For_Level()
        {            
            if (count_Tail == (5 * x))
            {
                labelLevel.Text = Convert.ToString(++level);  
                if (x == 4)
                {
                    timer.Stop();
                    DialogResult nextLevel = MessageBox.Show($"به مرحله بعد خوش آمدید\n" +
                                                             $"دراین‌مرحله‌شمانمیتوانیدازدیوارهاعبورکنید" +
                                                             $"\n** مراقب باشید ** :/",
                                                             "مرحله بعد",
                                                             MessageBoxButtons.OKCancel,
                                                             MessageBoxIcon.Warning,
                                                             MessageBoxDefaultButton.Button1,
                                                             MessageBoxOptions.RightAlign);                    
                }
                x++;
            }            

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
    }
}
