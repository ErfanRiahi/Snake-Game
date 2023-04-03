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

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            if (flag)
            {
                Form1 f = new Form1();
                this.Visible = false;
                f.Show();
            }
            else
                MessageBox.Show("لطفا ابتدا'روش بازی' را مطالعه کنید", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ButtonInformation_Click(object sender, EventArgs e)
        {
            flag = true;
            MessageBox.Show("* شما میتوانید به وسیله 4 کلید نشانه‌گر حرکت کنید *" +
                            "\n* از دیوارها میتوانید رد شوید *" +
                            "\n* برای خروج کلید Esc را فشار دهید *" +
                            "\n* برای نوسازی نتایج کلید R را فشار دهید *" +
                            "\n* برای توقف بازی کلید space را فشار دهید * ",
                            "اطلاعات",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.RtlReading);
        }

        private void ButtonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ButtonMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
