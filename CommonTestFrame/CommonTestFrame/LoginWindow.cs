using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CommonTestFrame
{
    public partial class LoginWindow : Form
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.myForm1.Confirm_Login_Click();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1.myForm1.Cancel_Login_Click();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                this.button1_Click(null, null);
            }
        }

        public void LoadUserRecord(List<string> iList)
        {
            if (iList == null) return;
            comboBox1.DataSource = null;
            comboBox1.DataSource = iList;
        }

       

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), "^[a-zA-Z0-9[\b]")) e.Handled = true;
        }
   
        public string GetInputPassword()
        {
            return textBox1.Text;
        }

        public string GetUserName()
        {
            return comboBox1.Text;
        }

        public void ClearPassword()
        {
            textBox1.Text = "";
        }

        private void LoginWindow_VisibleChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                textBox1.Focus();
            }
        }

       


      

  

      

       

    
    }
}
