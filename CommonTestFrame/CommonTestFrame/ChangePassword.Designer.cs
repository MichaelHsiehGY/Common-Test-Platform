namespace CommonTestFrame
{
    partial class ChangePasswordWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label_orginalPassword = new System.Windows.Forms.Label();
            this.label_NewPassword = new System.Windows.Forms.Label();
            this.textBox_orginalpassword = new System.Windows.Forms.TextBox();
            this.textBox_newPassword = new System.Windows.Forms.TextBox();
            this.button_ok = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.label_message = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_orginalPassword
            // 
            this.label_orginalPassword.AutoSize = true;
            this.label_orginalPassword.Location = new System.Drawing.Point(61, 48);
            this.label_orginalPassword.Name = "label_orginalPassword";
            this.label_orginalPassword.Size = new System.Drawing.Size(113, 12);
            this.label_orginalPassword.TabIndex = 0;
            this.label_orginalPassword.Text = "Original Password:";
            // 
            // label_NewPassword
            // 
            this.label_NewPassword.AutoSize = true;
            this.label_NewPassword.Location = new System.Drawing.Point(91, 105);
            this.label_NewPassword.Name = "label_NewPassword";
            this.label_NewPassword.Size = new System.Drawing.Size(83, 12);
            this.label_NewPassword.TabIndex = 1;
            this.label_NewPassword.Text = "New Password:";
            // 
            // textBox_orginalpassword
            // 
            this.textBox_orginalpassword.Location = new System.Drawing.Point(181, 48);
            this.textBox_orginalpassword.Name = "textBox_orginalpassword";
            this.textBox_orginalpassword.PasswordChar = '*';
            this.textBox_orginalpassword.Size = new System.Drawing.Size(157, 21);
            this.textBox_orginalpassword.TabIndex = 2;
            // 
            // textBox_newPassword
            // 
            this.textBox_newPassword.Location = new System.Drawing.Point(181, 105);
            this.textBox_newPassword.Name = "textBox_newPassword";
            this.textBox_newPassword.Size = new System.Drawing.Size(157, 21);
            this.textBox_newPassword.TabIndex = 3;
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(124, 162);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 4;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Location = new System.Drawing.Point(263, 162);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 5;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // label_message
            // 
            this.label_message.AutoSize = true;
            this.label_message.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_message.ForeColor = System.Drawing.Color.Red;
            this.label_message.Location = new System.Drawing.Point(70, 13);
            this.label_message.Name = "label_message";
            this.label_message.Size = new System.Drawing.Size(0, 16);
            this.label_message.TabIndex = 6;
            // 
            // ChangePasswordWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SeaShell;
            this.ClientSize = new System.Drawing.Size(406, 232);
            this.ControlBox = false;
            this.Controls.Add(this.label_message);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.textBox_newPassword);
            this.Controls.Add(this.textBox_orginalpassword);
            this.Controls.Add(this.label_NewPassword);
            this.Controls.Add(this.label_orginalPassword);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ChangePasswordWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ChangePassword";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_orginalPassword;
        private System.Windows.Forms.Label label_NewPassword;
        private System.Windows.Forms.TextBox textBox_orginalpassword;
        private System.Windows.Forms.TextBox textBox_newPassword;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Label label_message;
    }
}