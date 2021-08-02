using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CommonTestFrame
{
    public partial class RegisterWindow : Form
    {
        public RegisterWindow()
        {
            InitializeComponent();
            textBox_UserName.Focus();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            Form1.myForm1.Confirm_Register_Click();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            Form1.myForm1.Cancel_Register_Click();
        }

        public string GetInputPassword()
        {
            return textBox_Password.Text;
        }

        public string GetUserName()
        {
            return textBox_UserName.Text;
        }

        private void textBox_UserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), "^[a-zA-Z0-9[\b]")) e.Handled = true;
        }

        private void textBox_Password_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), "^[a-zA-Z0-9[\b]")) e.Handled = true;
        }

        public void ClearPassword()
        {
            this.textBox_Password.Text="";
        }
    }
}
