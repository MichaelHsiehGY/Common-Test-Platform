using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CommonTestFrame
{
    public partial class UserCenterWindows : Form
    {

        #region 变量

        DataTable UserDataTable = new DataTable();
        string[] dvgColumnHeaders = new string[] {"Select","User Name","Authorisation","Password","Phone","Email","Remark","Last Login Time" };
        int  columIndex = -1;
        int rowIndex = -1;

        ComboBox cmbTemp = new ComboBox();
        TextBox tbTemp = new TextBox();

        User ActiveUser = new User();

        ChangePasswordWindow iChangePasswordWindow = new ChangePasswordWindow();

      

        #endregion

        #region 用户管理

        #region 初始化

        public UserCenterWindows()
        {
            InitializeComponent();
            UserDataTable = CreatColumnHeadersforDatatable(UserDataTable, dvgColumnHeaders);
          
        }

        private DataTable CreatColumnHeadersforDatatable(DataTable iDataTable, string[] dvgColumnHeaders)
        {
            if (iDataTable == null)
            {
                iDataTable = new DataTable();
                foreach (string iColunHeader in dvgColumnHeaders)
                {
                    switch (iColunHeader)
                    {
                        case "Select":
                            iDataTable.Columns.Add(iColunHeader, typeof(bool));
                            break;
                        default:
                            iDataTable.Columns.Add(iColunHeader, typeof(string));
                            break;
                    }
                }
            }
            else
            {
                if (iDataTable.Columns.Count == 0)
                {
                    foreach (string iColunHeader in dvgColumnHeaders)
                    {
                        switch (iColunHeader)
                        {
                            case "Select":
                                iDataTable.Columns.Add(iColunHeader, typeof(bool));
                                break;
                            default:
                                iDataTable.Columns.Add(iColunHeader, typeof(string));
                                break;
                        }
                    }
                }
                if (iDataTable.Rows.Count != 0)
                {
                    iDataTable.Rows.Clear();
                }
            }

            return iDataTable;
        }

        //根据用户权限设置按钮可见性
        public void SetUIVisablity(string UserType)
        {
            switch (UserType)
            {
                case "Programmer":
                    break;
                case "Administrator":
                    toolStripButton_Programmer.Visible = false;
                    toolStripButton_Administrator.Visible = false;
                    break;
                case "Common User":
                    tabControl1.TabPages.Remove(tabPage_UserManagement);
                    break;
            }
            tabControl1.Refresh();
        }

        #endregion

        #region 加载用户数据

        private void DisplayUsers(Users iUsers,string SelectString)
        {
            UpdateUserDatafromDVG();

            List<string> iList=new List<string>();
            UserDataTable.Rows.Clear();
            foreach (User iUser in iUsers)
            {
                switch (SelectString)
                {
                    case "All":
                        iList.Clear();
                         foreach(var iUserElement in iUser.GetType().GetProperties())
                        {
                            object value = iUserElement.GetValue(iUser, new object[] { });
                            iList.Add(value.ToString());
                        }
                        iList.Insert(0, "False");
                        UserDataTable.Rows.Add(iList.ToArray());
                        break;
                    default:
                        if (SelectString.Contains("Programmer"))
                        {
                            if (iUser.UserType == "Programmer")
                            {
                                iList.Clear();
                                foreach (var iUserElement in iUser.GetType().GetProperties())
                                {
                                    object value = iUserElement.GetValue(iUser, new object[] { });
                                    iList.Add(value.ToString());

                                }
                                iList.Insert(0, "False");
                                UserDataTable.Rows.Add(iList.ToArray());
                            }
                        }
                        if (SelectString.Contains("Administrator"))
                        {
                            if (iUser.UserType == "Administrator")
                            {
                                iList.Clear();
                                foreach (var iUserElement in iUser.GetType().GetProperties())
                                {
                                    object value = iUserElement.GetValue(iUser, new object[] { });
                                    iList.Add(value.ToString());
                                }
                                iList.Insert(0, "False");
                                UserDataTable.Rows.Add(iList.ToArray());
                            }
                        }
                        if (SelectString.Contains("Common User"))
                        {
                            if (iUser.UserType == "Common User")
                            {
                                iList.Clear();
                                foreach (var iUserElement in iUser.GetType().GetProperties())
                                {
                                    object value = iUserElement.GetValue(iUser, new object[] { });
                                    iList.Add(value.ToString());
                                }
                                iList.Insert(0, "False");
                                UserDataTable.Rows.Add(iList.ToArray());
                            }
                        }
                        break;
                }
            }
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = UserDataTable;
            dataGridView1.Refresh();
           
        }

        private void toolStripButton_Alluser_Click(object sender, EventArgs e)
        {
            switch (Form1.myForm1.ActiveUser.UserType)
            {
                case "Programmer":
                    DisplayUsers(Form1.myForm1.iUsers, "All");
                    break;
                case "Administrator":
                    DisplayUsers(Form1.myForm1.iUsers, "Common User");
                    break;
            }
        }

        private void toolStripButton_Programmer_Click(object sender, EventArgs e)
        {
            DisplayUsers(Form1.myForm1.iUsers, "Programmer");
        }

        private void toolStripButton_Administrator_Click(object sender, EventArgs e)
        {
            DisplayUsers(Form1.myForm1.iUsers, "Administrator");
        }

        private void toolStripButton_CommonUser_Click(object sender, EventArgs e)
        {
            DisplayUsers(Form1.myForm1.iUsers, "Common User");
        }

        #endregion

        #region 添加删除用户

        private void RemoveUser(Users inUsers, List<string> inList, DataTable inDataTable)
        {
            string iUserName = "";
            for (int i = 0; i < inDataTable.Rows.Count; i++)
            {
                if ((bool)inDataTable.Rows[i]["Select"])
                {
                    iUserName = inDataTable.Rows[i]["User Name"].ToString();
                    inDataTable.Rows.RemoveAt(i);
                    foreach (User iUser in inUsers)
                    {
                        if (iUserName == iUser.UserName)
                        {
                            inUsers.Remove(iUser);
                            break;
                        }
                    }
                    foreach (string eUserName in inList)
                    {
                        if (eUserName == iUserName)
                        {
                            inList.Remove(eUserName);
                            break;
                        }
                    }
                    break;
                }
            }
            for (int i = 0; i < UserDataTable.Rows.Count; i++)
            {
                if ((bool)UserDataTable.Rows[i]["Select"])
                {
                    RemoveUser(inUsers, inList, inDataTable);
                }
            }
        }

        private void toolStripMenuItem_AddUser_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            DataRow newDataRow = UserDataTable.NewRow();
            UserDataTable.Rows.Add(newDataRow);
            dataGridView1.DataSource = UserDataTable;
            dataGridView1.Refresh();
        }

        private void toolStripMenuItem_DeleteUser_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            RemoveUser(Form1.myForm1.iUsers, Form1.myForm1.iUserNameList, UserDataTable);
            dataGridView1.DataSource = UserDataTable;
            dataGridView1.Refresh();

        }


        #endregion

        #region 编辑用户数据

        //manage authorisation
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            columIndex = e.ColumnIndex;
            rowIndex = e.RowIndex;
            if (cmbTemp.Parent != null)
            {
                dataGridView1.Controls.Remove(cmbTemp);
            }
           
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                if (e.ColumnIndex.Equals(this.dataGridView1.Columns["Select"].Index))//判断单元格是否是"Select"列
                {
                    if (dataGridView1.Rows[e.RowIndex].Cells["Select"].Value.Equals(true))
                    {
                        dataGridView1.Rows[e.RowIndex].Cells["Select"].Value = false;
                    }
                    else
                    {
                        dataGridView1.Rows[e.RowIndex].Cells["Select"].Value = true;
                    }
                }
                else
                {
                    if (e.ColumnIndex == this.dataGridView1.Columns["Authorisation"].Index)
                    {
                        switch (Form1.myForm1.ActiveUser.UserType)
                        {
                            case "Programmer":
                                cmbTemp.DataSource = new string[] { "Programmer", "Administrator", "Common User" };
                                break;
                            case "Administrator":
                                cmbTemp.DataSource = new string[] { "Common User" };
                                break;
                        }
                        cmbTemp.Visible = true;
                        cmbTemp.DropDownStyle = ComboBoxStyle.DropDown;
                        
                        cmbTemp.Width = this.dataGridView1.GetCellDisplayRectangle(columIndex, rowIndex, true).Width;
                        cmbTemp.Height = this.dataGridView1.GetCellDisplayRectangle(columIndex, rowIndex, true).Height;
                        cmbTemp.SelectedIndexChanged += new EventHandler(cmbTemp_SelectedIndexChanged);
                        this.dataGridView1.Controls.Add(cmbTemp);
                        cmbTemp.Location = new System.Drawing.Point(((this.dataGridView1.GetCellDisplayRectangle(columIndex, rowIndex, true).Right) - (cmbTemp.Width)), this.dataGridView1.GetCellDisplayRectangle(columIndex, rowIndex, true).Y);//设置btn显示位置
                        //this.dataGridView1.Refresh();
                        //cmbTemp.Text = dataGridView1.Rows[rowIndex].Cells["Authorisation"].Value.ToString();
                    }
                    else
                    {
                        tbTemp.Visible = true;
                        tbTemp.Width = this.dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Width;//获取单元格高并设置为tbTemp的宽
                        tbTemp.Height = this.dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Height;//获取单元格高并设置为tbTemp的高
                        tbTemp.LostFocus += new EventHandler(tbTemp_LostFocus);
                        this.dataGridView1.Controls.Add(tbTemp);
                        tbTemp.Focus();
                        tbTemp.SelectAll();
                        tbTemp.Location = new System.Drawing.Point(((this.dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Right) - (tbTemp.Width)), this.dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Y);//设置tbTemp显示位置
                        //this.dataGridView1.Refresh();
                        tbTemp.Text = dataGridView1.Rows[rowIndex].Cells[e.ColumnIndex].Value.ToString();
                        return;
                    }
                }
            }
            
        }

        void tbTemp_LostFocus(object sender, EventArgs e)
        {
            if (tbTemp.Parent == null) return;
            dataGridView1.Rows[rowIndex].Cells[columIndex].Value = tbTemp.Text.Trim();
            this.dataGridView1.Controls.Remove(tbTemp);
            //this.dataGridView1.Refresh();
        }

        void cmbTemp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (columIndex == this.dataGridView1.Columns["Authorisation"].Index)
            {
                dataGridView1.Rows[rowIndex].Cells["Authorisation"].Value = cmbTemp.Text;
                dataGridView1.Controls.Remove(cmbTemp);
            }
        }

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
            }
        }

        private void UpdateUserDatafromDVG()
        {
            string iUserName = "";
            if (dataGridView1.Columns.Count <= 0) return;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                User iUser = new User();
                iUser.UserName = dataGridView1.Rows[i].Cells["User Name"].Value.ToString();
                iUser.UserType = dataGridView1.Rows[i].Cells["Authorisation"].Value.ToString();
                iUser.Password = dataGridView1.Rows[i].Cells["Password"].Value.ToString();
                iUser.Phone = dataGridView1.Rows[i].Cells["Phone"].Value.ToString();
                iUser.Email = dataGridView1.Rows[i].Cells["Email"].Value.ToString();
                iUser.LastLoginTime = dataGridView1.Rows[i].Cells["Last Login Time"].Value.ToString();
                if (iUser.UserName != "" && iUser.UserType != "" && iUser.Password != "")
                {
                    if (!FindTargetUserName(iUser.UserName))
                    {
                        Form1.myForm1.iUsers.Add(iUser);
                        Form1.myForm1.iUserNameList.Add(iUser.UserName);
                    }
                    else
                    {
                        foreach (User oUser in Form1.myForm1.iUsers)
                        {
                            if (iUser.UserName == oUser.UserName)
                            {
                                iUserName = iUser.UserName;
                                Form1.myForm1.iUsers.Remove(oUser);
                                Form1.myForm1.iUsers.Add(iUser);
                                break;
                            }
                        }
                    }
                   
                    dataGridView1.Rows.RemoveAt(i);
                    break;
                }
            }
            if (dataGridView1.Rows.Count> 0)
            {
                UpdateUserDatafromDVG();
            }
        }

        private bool FindTargetUserName(string inUserName)
        {
            foreach (string eUserName in Form1.myForm1.iUserNameList)
            {
                if (eUserName == inUserName)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region 关闭窗体，更新用户数据到记录文件

        private void UserCenterWindows_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage_UserManagement)
            {
                UpdateUserDatafromDVG();
                Form1.myForm1.UserCenterWindowClosed();
            }
        }

        #endregion

       

        #endregion

       

        #region 个人中心

        private void button_ok_Click(object sender, EventArgs e)
        {
            SaveDatatoActiveUser();
            Form1.myForm1.UpdateUserData(ActiveUser);
            this.Hide();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            LoadActiveUserData(ActiveUser);
            this.Hide();//here
        }

        private void button_apply_Click(object sender, EventArgs e)
        {
            SaveDatatoActiveUser();
            Form1.myForm1.UpdateUserData(ActiveUser);
        }

        public void LoadActiveUserData(User iUser)
        {
            ActiveUser = iUser;
            this.textBox_Account.Text = iUser.UserName;
            this.textBox_Authiorisation.Text = iUser.UserType;
            this.textBox_phone.Text = iUser.Phone;
            this.textBox_email.Text = iUser.Email;
            this.textBox_lastlogintime.Text = iUser.LastLoginTime;
            this.textBox_remark.Text = iUser.Remark;
        }

        private void SaveDatatoActiveUser()
        {
            ActiveUser.UserName = this.textBox_Account.Text;
            ActiveUser.UserType = this.textBox_Authiorisation.Text;
            ActiveUser.Phone = this.textBox_phone.Text;
            ActiveUser.Email = this.textBox_email.Text;
            ActiveUser.LastLoginTime = this.textBox_lastlogintime.Text;
            ActiveUser.Remark = this.textBox_remark.Text;
        }

        private void label_changePassword_Click(object sender, EventArgs e)
        {
            iChangePasswordWindow.ActiveUser = ActiveUser;
            iChangePasswordWindow.Show();
        }

        private void label_changePassword_MouseEnter(object sender, EventArgs e)
        {
            label_changePassword.BackColor = Color.LightBlue;
        }

        private void label_changePassword_MouseLeave(object sender, EventArgs e)
        {
            label_changePassword.BackColor = Color.Transparent;
        }
        

        #endregion

       

        

      

       

    }
}
