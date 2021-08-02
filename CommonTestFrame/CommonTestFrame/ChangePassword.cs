using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CommonTestFrame
{
    public partial class ChangePasswordWindow : Form
    {

       public User ActiveUser = new User();
        public ChangePasswordWindow()
        {
            InitializeComponent();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            if (textBox_orginalpassword.Text == ActiveUser.Password)
            {
                if (textBox_newPassword.Text.Trim() == "")
                {
                    label_message.Text = "Empty password is not allowed!";
                }
                else
                {
                    ActiveUser.Password = textBox_newPassword.Text;
                    textBox_newPassword.Text = "";
                    textBox_orginalpassword.Text = "";
                    this.Hide();
                }
            }
            else
            {
                label_message.Text = "Original password is woring!";
            }
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            textBox_newPassword.Text = "";
            textBox_orginalpassword.Text = "";
            this.Hide();
        }


    }



    
}
