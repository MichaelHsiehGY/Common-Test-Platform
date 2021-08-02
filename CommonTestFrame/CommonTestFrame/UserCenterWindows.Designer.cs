namespace CommonTestFrame
{
    partial class UserCenterWindows
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserCenterWindows));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel_UserGroup = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButton_Alluser = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Programmer = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Administrator = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_CommonUser = new System.Windows.Forms.ToolStripButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_AddUser = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_DeleteUser = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_UserManagement = new System.Windows.Forms.TabPage();
            this.tabPage_UserCenter = new System.Windows.Forms.TabPage();
            this.label_changePassword = new System.Windows.Forms.Label();
            this.button_apply = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_ok = new System.Windows.Forms.Button();
            this.textBox_remark = new System.Windows.Forms.TextBox();
            this.textBox_lastlogintime = new System.Windows.Forms.TextBox();
            this.textBox_email = new System.Windows.Forms.TextBox();
            this.textBox_phone = new System.Windows.Forms.TextBox();
            this.textBox_Authiorisation = new System.Windows.Forms.TextBox();
            this.textBox_Account = new System.Windows.Forms.TextBox();
            this.label_LastLoginTime = new System.Windows.Forms.Label();
            this.label_remark = new System.Windows.Forms.Label();
            this.label_email = new System.Windows.Forms.Label();
            this.label_Phone = new System.Windows.Forms.Label();
            this.label_Password = new System.Windows.Forms.Label();
            this.label_Authorisation = new System.Windows.Forms.Label();
            this.label_name = new System.Windows.Forms.Label();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage_UserManagement.SuspendLayout();
            this.tabPage_UserCenter.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer2.Size = new System.Drawing.Size(775, 442);
            this.splitContainer2.SplitterDistance = 211;
            this.splitContainer2.SplitterWidth = 6;
            this.splitContainer2.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.LightPink;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel_UserGroup,
            this.toolStripButton_Alluser,
            this.toolStripButton_Programmer,
            this.toolStripButton_Administrator,
            this.toolStripButton_CommonUser});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(211, 442);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel_UserGroup
            // 
            this.toolStripLabel_UserGroup.AutoSize = false;
            this.toolStripLabel_UserGroup.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.toolStripLabel_UserGroup.Name = "toolStripLabel_UserGroup";
            this.toolStripLabel_UserGroup.Size = new System.Drawing.Size(150, 24);
            this.toolStripLabel_UserGroup.Text = "User Group";
            this.toolStripLabel_UserGroup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripButton_Alluser
            // 
            this.toolStripButton_Alluser.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton_Alluser.AutoSize = false;
            this.toolStripButton_Alluser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_Alluser.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Alluser.Image")));
            this.toolStripButton_Alluser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Alluser.Name = "toolStripButton_Alluser";
            this.toolStripButton_Alluser.Size = new System.Drawing.Size(120, 21);
            this.toolStripButton_Alluser.Text = "   All User";
            this.toolStripButton_Alluser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripButton_Alluser.Click += new System.EventHandler(this.toolStripButton_Alluser_Click);
            // 
            // toolStripButton_Programmer
            // 
            this.toolStripButton_Programmer.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton_Programmer.AutoSize = false;
            this.toolStripButton_Programmer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_Programmer.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Programmer.Image")));
            this.toolStripButton_Programmer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Programmer.Name = "toolStripButton_Programmer";
            this.toolStripButton_Programmer.Size = new System.Drawing.Size(120, 21);
            this.toolStripButton_Programmer.Text = "   Programmer";
            this.toolStripButton_Programmer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripButton_Programmer.Click += new System.EventHandler(this.toolStripButton_Programmer_Click);
            // 
            // toolStripButton_Administrator
            // 
            this.toolStripButton_Administrator.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton_Administrator.AutoSize = false;
            this.toolStripButton_Administrator.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_Administrator.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Administrator.Image")));
            this.toolStripButton_Administrator.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Administrator.Name = "toolStripButton_Administrator";
            this.toolStripButton_Administrator.Size = new System.Drawing.Size(120, 21);
            this.toolStripButton_Administrator.Text = "   Administrator";
            this.toolStripButton_Administrator.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripButton_Administrator.Click += new System.EventHandler(this.toolStripButton_Administrator_Click);
            // 
            // toolStripButton_CommonUser
            // 
            this.toolStripButton_CommonUser.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton_CommonUser.AutoSize = false;
            this.toolStripButton_CommonUser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_CommonUser.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_CommonUser.Image")));
            this.toolStripButton_CommonUser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_CommonUser.Name = "toolStripButton_CommonUser";
            this.toolStripButton_CommonUser.Size = new System.Drawing.Size(120, 21);
            this.toolStripButton_CommonUser.Text = "   Common User";
            this.toolStripButton_CommonUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripButton_CommonUser.Click += new System.EventHandler(this.toolStripButton_CommonUser_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.LightCyan;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridView1.RowHeadersVisible = false;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.LightBlue;
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(558, 442);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellMouseEnter);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_AddUser,
            this.toolStripMenuItem_DeleteUser});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(145, 48);
            // 
            // toolStripMenuItem_AddUser
            // 
            this.toolStripMenuItem_AddUser.Name = "toolStripMenuItem_AddUser";
            this.toolStripMenuItem_AddUser.Size = new System.Drawing.Size(144, 22);
            this.toolStripMenuItem_AddUser.Text = "Add User";
            this.toolStripMenuItem_AddUser.Click += new System.EventHandler(this.toolStripMenuItem_AddUser_Click);
            // 
            // toolStripMenuItem_DeleteUser
            // 
            this.toolStripMenuItem_DeleteUser.Name = "toolStripMenuItem_DeleteUser";
            this.toolStripMenuItem_DeleteUser.Size = new System.Drawing.Size(144, 22);
            this.toolStripMenuItem_DeleteUser.Text = "Delete User";
            this.toolStripMenuItem_DeleteUser.Click += new System.EventHandler(this.toolStripMenuItem_DeleteUser_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_UserManagement);
            this.tabControl1.Controls.Add(this.tabPage_UserCenter);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(789, 474);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage_UserManagement
            // 
            this.tabPage_UserManagement.BackColor = System.Drawing.Color.SeaShell;
            this.tabPage_UserManagement.Controls.Add(this.splitContainer2);
            this.tabPage_UserManagement.Location = new System.Drawing.Point(4, 22);
            this.tabPage_UserManagement.Name = "tabPage_UserManagement";
            this.tabPage_UserManagement.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_UserManagement.Size = new System.Drawing.Size(781, 448);
            this.tabPage_UserManagement.TabIndex = 0;
            this.tabPage_UserManagement.Text = "User Management";
            // 
            // tabPage_UserCenter
            // 
            this.tabPage_UserCenter.BackColor = System.Drawing.Color.SeaShell;
            this.tabPage_UserCenter.Controls.Add(this.label_changePassword);
            this.tabPage_UserCenter.Controls.Add(this.button_apply);
            this.tabPage_UserCenter.Controls.Add(this.button_Cancel);
            this.tabPage_UserCenter.Controls.Add(this.button_ok);
            this.tabPage_UserCenter.Controls.Add(this.textBox_remark);
            this.tabPage_UserCenter.Controls.Add(this.textBox_lastlogintime);
            this.tabPage_UserCenter.Controls.Add(this.textBox_email);
            this.tabPage_UserCenter.Controls.Add(this.textBox_phone);
            this.tabPage_UserCenter.Controls.Add(this.textBox_Authiorisation);
            this.tabPage_UserCenter.Controls.Add(this.textBox_Account);
            this.tabPage_UserCenter.Controls.Add(this.label_LastLoginTime);
            this.tabPage_UserCenter.Controls.Add(this.label_remark);
            this.tabPage_UserCenter.Controls.Add(this.label_email);
            this.tabPage_UserCenter.Controls.Add(this.label_Phone);
            this.tabPage_UserCenter.Controls.Add(this.label_Password);
            this.tabPage_UserCenter.Controls.Add(this.label_Authorisation);
            this.tabPage_UserCenter.Controls.Add(this.label_name);
            this.tabPage_UserCenter.Location = new System.Drawing.Point(4, 22);
            this.tabPage_UserCenter.Name = "tabPage_UserCenter";
            this.tabPage_UserCenter.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_UserCenter.Size = new System.Drawing.Size(781, 448);
            this.tabPage_UserCenter.TabIndex = 1;
            this.tabPage_UserCenter.Text = "User Informaiton";
            // 
            // label_changePassword
            // 
            this.label_changePassword.AutoSize = true;
            this.label_changePassword.Font = new System.Drawing.Font("SimSun", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_changePassword.ForeColor = System.Drawing.Color.OrangeRed;
            this.label_changePassword.Location = new System.Drawing.Point(232, 112);
            this.label_changePassword.Name = "label_changePassword";
            this.label_changePassword.Size = new System.Drawing.Size(95, 12);
            this.label_changePassword.TabIndex = 17;
            this.label_changePassword.Text = "Change Password";
            this.label_changePassword.Click += new System.EventHandler(this.label_changePassword_Click);
            this.label_changePassword.MouseEnter += new System.EventHandler(this.label_changePassword_MouseEnter);
            this.label_changePassword.MouseLeave += new System.EventHandler(this.label_changePassword_MouseLeave);
            // 
            // button_apply
            // 
            this.button_apply.BackColor = System.Drawing.Color.Transparent;
            this.button_apply.Location = new System.Drawing.Point(581, 406);
            this.button_apply.Name = "button_apply";
            this.button_apply.Size = new System.Drawing.Size(75, 23);
            this.button_apply.TabIndex = 16;
            this.button_apply.Text = "Apply";
            this.button_apply.UseVisualStyleBackColor = false;
            this.button_apply.Click += new System.EventHandler(this.button_apply_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.BackColor = System.Drawing.Color.Transparent;
            this.button_Cancel.Location = new System.Drawing.Point(470, 406);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 15;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = false;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // button_ok
            // 
            this.button_ok.BackColor = System.Drawing.Color.Transparent;
            this.button_ok.Location = new System.Drawing.Point(359, 406);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 14;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = false;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // textBox_remark
            // 
            this.textBox_remark.Location = new System.Drawing.Point(230, 283);
            this.textBox_remark.Multiline = true;
            this.textBox_remark.Name = "textBox_remark";
            this.textBox_remark.Size = new System.Drawing.Size(426, 101);
            this.textBox_remark.TabIndex = 13;
            // 
            // textBox_lastlogintime
            // 
            this.textBox_lastlogintime.Enabled = false;
            this.textBox_lastlogintime.Location = new System.Drawing.Point(230, 244);
            this.textBox_lastlogintime.Name = "textBox_lastlogintime";
            this.textBox_lastlogintime.Size = new System.Drawing.Size(263, 21);
            this.textBox_lastlogintime.TabIndex = 12;
            // 
            // textBox_email
            // 
            this.textBox_email.Location = new System.Drawing.Point(230, 199);
            this.textBox_email.Name = "textBox_email";
            this.textBox_email.Size = new System.Drawing.Size(263, 21);
            this.textBox_email.TabIndex = 11;
            // 
            // textBox_phone
            // 
            this.textBox_phone.Location = new System.Drawing.Point(230, 157);
            this.textBox_phone.Name = "textBox_phone";
            this.textBox_phone.Size = new System.Drawing.Size(263, 21);
            this.textBox_phone.TabIndex = 10;
            // 
            // textBox_Authiorisation
            // 
            this.textBox_Authiorisation.Enabled = false;
            this.textBox_Authiorisation.Location = new System.Drawing.Point(230, 64);
            this.textBox_Authiorisation.Name = "textBox_Authiorisation";
            this.textBox_Authiorisation.Size = new System.Drawing.Size(263, 21);
            this.textBox_Authiorisation.TabIndex = 8;
            // 
            // textBox_Account
            // 
            this.textBox_Account.Enabled = false;
            this.textBox_Account.Location = new System.Drawing.Point(230, 19);
            this.textBox_Account.Name = "textBox_Account";
            this.textBox_Account.Size = new System.Drawing.Size(263, 21);
            this.textBox_Account.TabIndex = 7;
            // 
            // label_LastLoginTime
            // 
            this.label_LastLoginTime.AutoSize = true;
            this.label_LastLoginTime.Location = new System.Drawing.Point(125, 247);
            this.label_LastLoginTime.Name = "label_LastLoginTime";
            this.label_LastLoginTime.Size = new System.Drawing.Size(101, 12);
            this.label_LastLoginTime.TabIndex = 6;
            this.label_LastLoginTime.Text = "Last Login Time:";
            // 
            // label_remark
            // 
            this.label_remark.AutoSize = true;
            this.label_remark.Location = new System.Drawing.Point(179, 292);
            this.label_remark.Name = "label_remark";
            this.label_remark.Size = new System.Drawing.Size(47, 12);
            this.label_remark.TabIndex = 5;
            this.label_remark.Text = "Remark:";
            // 
            // label_email
            // 
            this.label_email.AutoSize = true;
            this.label_email.Location = new System.Drawing.Point(185, 202);
            this.label_email.Name = "label_email";
            this.label_email.Size = new System.Drawing.Size(41, 12);
            this.label_email.TabIndex = 4;
            this.label_email.Text = "Email:";
            // 
            // label_Phone
            // 
            this.label_Phone.AutoSize = true;
            this.label_Phone.Location = new System.Drawing.Point(185, 157);
            this.label_Phone.Name = "label_Phone";
            this.label_Phone.Size = new System.Drawing.Size(41, 12);
            this.label_Phone.TabIndex = 3;
            this.label_Phone.Text = "Phone:";
            // 
            // label_Password
            // 
            this.label_Password.AutoSize = true;
            this.label_Password.Location = new System.Drawing.Point(167, 112);
            this.label_Password.Name = "label_Password";
            this.label_Password.Size = new System.Drawing.Size(59, 12);
            this.label_Password.TabIndex = 2;
            this.label_Password.Text = "Password:";
            // 
            // label_Authorisation
            // 
            this.label_Authorisation.AutoSize = true;
            this.label_Authorisation.Location = new System.Drawing.Point(137, 67);
            this.label_Authorisation.Name = "label_Authorisation";
            this.label_Authorisation.Size = new System.Drawing.Size(89, 12);
            this.label_Authorisation.TabIndex = 1;
            this.label_Authorisation.Text = "Authorisation:";
            // 
            // label_name
            // 
            this.label_name.AutoSize = true;
            this.label_name.Location = new System.Drawing.Point(173, 22);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(53, 12);
            this.label_name.TabIndex = 0;
            this.label_name.Text = "Account:";
            // 
            // UserCenterWindows
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Snow;
            this.ClientSize = new System.Drawing.Size(789, 474);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "UserCenterWindows";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User Center";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.UserCenterWindows_FormClosed);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_UserManagement.ResumeLayout(false);
            this.tabPage_UserCenter.ResumeLayout(false);
            this.tabPage_UserCenter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_UserGroup;
        private System.Windows.Forms.ToolStripButton toolStripButton_Programmer;
        private System.Windows.Forms.ToolStripButton toolStripButton_Administrator;
        private System.Windows.Forms.ToolStripButton toolStripButton_CommonUser;
        private System.Windows.Forms.ToolStripButton toolStripButton_Alluser;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_DeleteUser;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_AddUser;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage_UserManagement;
        private System.Windows.Forms.TabPage tabPage_UserCenter;
        private System.Windows.Forms.Label label_remark;
        private System.Windows.Forms.Label label_email;
        private System.Windows.Forms.Label label_Phone;
        private System.Windows.Forms.Label label_Password;
        private System.Windows.Forms.Label label_Authorisation;
        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.Label label_LastLoginTime;
        private System.Windows.Forms.TextBox textBox_remark;
        private System.Windows.Forms.TextBox textBox_lastlogintime;
        private System.Windows.Forms.TextBox textBox_email;
        private System.Windows.Forms.TextBox textBox_phone;
        private System.Windows.Forms.TextBox textBox_Authiorisation;
        private System.Windows.Forms.TextBox textBox_Account;
        private System.Windows.Forms.Button button_apply;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Label label_changePassword;

    }
}