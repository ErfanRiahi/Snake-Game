using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake_V1._3
{
    public partial class Welcome : Form
    {
        bool flag = false;
        public Welcome()
        {
            InitializeComponent();
        }

        private void ButtonMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            if (flag)
            {
                Form1 f = new Form1();
                this.Visible = false;
                f.Show();
            }
            else
                MessageBox.Show("Please read 'How To Play' first", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ButtonInformation_Click(object sender, EventArgs e)
        {
            flag = true;
            MessageBox.Show("* You can move by 4 arrow(Up, Down, Left, Right) *\n* You can across the wall *" +
                            "\n* for exit click escape(Esc) *\n" +
                            "* for restart click R *\n* for pause click space * ",
                            "Information",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }

        private void ButtonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        
        // Move form by mouse        
        //*******************************************************************************        
        bool mouseDown;
        private Point lastLocation;
        private void Welcome_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }
        private void Welcome_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
                this.Location = new Point((this.Location.X - lastLocation.X) + e.X,
                                          (this.Location.Y - lastLocation.Y) + e.Y);
        }
        private void Welcome_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
        //*******************************************************************************
    }
}
