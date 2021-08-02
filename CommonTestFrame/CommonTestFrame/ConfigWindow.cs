using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Xml;
using Organization;


namespace CommonTestFrame
{
    public partial class ConfigWindow : Form
    {

        #region 变量声明等

        public static ConfigWindow myCWForm = null;
        private delegate void MyDelegatePara1(DataTable myFilterDataTable);
        private delegate void iDelegatePara1(string iFileName);


        public string SelectedPlan = "";
        public int rowIndex = 0;
        public int columIndex = 0;
        public DataTable CommonTable = null;
   

        static Thread FilterThread = null;

        ComboBox cmbTemp = new ComboBox();
        Button btnTemp = new Button();
        CheckBox ckbTemp = new CheckBox();
        TextBox tbTemp = new TextBox();
        ComboBox cmbTemp_Slave = new ComboBox();

        int selectedRowIndex = 0;//


        public string[] dvgColumnHeaders_Testplan = new string[] { "Select", "Driver", "Class", "TestItem", "DisplayText","Loop", "ExecuteType",  "Para", "ParaDescriptionFile", "ReturnType", "IsJudge","IsShowResult","Measure", "SpecFile"};
        public List<string> ExecuteTypeList = new List<string> { "Once","Normal","EveryTime"};
        
        //创建菜单
        ContextMenu MTdgv1 = new ContextMenu();
        ContextMenu STdgv4 = new ContextMenu();
        ContextMenu Basedgv6 = new ContextMenu();
  


        //实现拖放功能
        private Rectangle dragBoxFromMouseDown;
        int indexofsourcerow = -1;
        int indexoftargetrow = -1;


        private string currentTestItem = "";

        //获取驱动名称
        private List<string> DllNames = new List<string>();

        //获取驱动接口
        private List<string> DllFunctions= new List<string>();
        private List<string> DllClasses = new List<string>();


        //file path
       public string RootPath = "";
       public string TestPlanPath = "";
       public string BasePath = "";
       public string SpecPath = "";
       public string DescriptionPath = "";
       public string TestResultPath = "";
       public string TestResultPath_txt = "";
       public string TestResultPath_csv = "";

        //判断是否创建新测试计划
       private bool IsCreateNewTestPlan = false;

        //判断datagridview是否修改
       private bool IsPlanChanged = false;


        #endregion

        #region 初始化

        public ConfigWindow()
        {
            try
            {
                InitializeComponent();

                myCWForm = this;

                UpdateFilePath(RootPath);
            }
            catch (Exception ex)
            {
                Form1.myForm1.DealwithException(ex);
            }
           
        }

        #region  Form Load
        
        private void ConfigWindow_Load(object sender, EventArgs e)
        {
            try
            {
                GetDllNames();
                toolStripComboBox1.ComboBox.DataSource = GetFileNames(TestPlanPath, "*.tps");
                toolStripComboBox1.ComboBox.DisplayMember = "TABLE_NAME";

                //创建添加删除行菜单
                CreateMenuItem_MTdgv1();
            }
            catch (Exception ex)
            {
                Form1.myForm1.DealwithException(ex);
            }
        }

   

        #endregion

        #endregion

        #region 打开测试计划

        //open test plan foler
        private void toolStripMenuItem_Open_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsPlanChanged == true || IsCreateNewTestPlan==true)
                {
                    switch (MessageBox.Show("Do you want to save changes to document \"" + toolStripComboBox1.Text + ".tps\"", "CommonTest", MessageBoxButtons.YesNo))
                    {
                        case DialogResult.Yes:
                            SaveTestPlan();
                            return;
                        case DialogResult.No:
                            break;
                    }
                }
                IsCreateNewTestPlan = false;
                OpenFileDialog iOpenFolder = new OpenFileDialog();
                iOpenFolder.Filter = "TPS files (*.tps)|*.tps";
                iOpenFolder.InitialDirectory = @Form1.myForm1.iConfig.RootPath;
                iOpenFolder.Title = "Select test plan folder";
                if (iOpenFolder.ShowDialog() == DialogResult.OK)
                {
                    RootPath = Path.GetDirectoryName(iOpenFolder.FileName);
                    Form1.myForm1.iConfig.RootPath = RootPath;
                    UpdateFilePath(RootPath);
                    toolStripComboBox1.ComboBox.DataSource = GetFileNames(TestPlanPath, "*.tps");
                    toolStripComboBox1.ComboBox.DisplayMember = "TABLE_NAME";
                    SelectedPlan = Path.GetFileNameWithoutExtension(iOpenFolder.FileName);
                    toolStripComboBox1.ComboBox.Text = SelectedPlan;
                }
            }
            catch (Exception ex)
            {
                Form1.myForm1.DealwithException(ex);
            }
        }

        #endregion

        #region 初始化CommonTable

        public void InitCommonTable(string iFileName)
        {
            CommonTable = XmlReadFiletoDataTable(iFileName, CommonTable, dvgColumnHeaders_Testplan);
        }

        #endregion 

        #region  创建测试计划 

        private string UpdateRootPath(string newPlanName)
        {
            int i = 0;
            string newRootPath = "";
            string[] tString = TestPlanPath.TrimEnd('\\').Split('\\');
            tString[tString.Length - 1] = newPlanName;
            foreach (string iString in tString)
            {
                newRootPath +=  iString+"\\";
            }
            newRootPath = newRootPath.TrimEnd('\\');
            string toOutNewRootPath = newRootPath;
            do
            {
                if (i != 0)
                {
                    toOutNewRootPath = newRootPath + i.ToString();
                }
                i++;

            } while (Directory.Exists(toOutNewRootPath));
            return toOutNewRootPath;
        }
        //create a space plan
        private void toolStripMenuItem_New_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsPlanChanged == true || IsCreateNewTestPlan==true)
                {
                    switch (MessageBox.Show("Do you want to save changes to document \"" + toolStripComboBox1.Text + ".tps\"", "CommonTest", MessageBoxButtons.YesNo))
                    {
                        case DialogResult.Yes:
                            SaveTestPlan();
                            return;
                        case DialogResult.No:
                            break;
                    }
                }
                IsCreateNewTestPlan = true;
                toolStripComboBox1.Text = "NewPlan";
                dataGridView1.DataSource = null;
                CommonTable = CreatColumnHeadersforDatatable(CommonTable, dvgColumnHeaders_Testplan);
                dataGridView1.DataSource = CommonTable;
                dataGridView1.Columns["ParaDescriptionFile"].Visible = false;
                dataGridView1.Columns["SpecFile"].Visible = false;
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                dataGridView1.Columns["Select"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                dataGridView1.Columns["Loop"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            }
            catch (Exception ex)
            {
                Form1.myForm1.DealwithException(ex);
            }
        }

        //create a new plan based on current plan
        private void toolStripMenuItem_Copy_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsPlanChanged == true || IsCreateNewTestPlan==true)
                {
                    switch (MessageBox.Show("Do you want to save changes to document \"" + toolStripComboBox1.Text + ".tps\"", "CommonTest", MessageBoxButtons.YesNo))
                    {
                        case DialogResult.Yes:
                            SaveTestPlan();
                            return;
                        case DialogResult.No:
                            break;
                    }
                }
                IsCreateNewTestPlan = true;
                if (CommonTable == null) return;

                toolStripComboBox1.Text = "NewPlan";
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = CommonTable;
                dataGridView1.Columns["ParaDescriptionFile"].Visible = false;
                dataGridView1.Columns["SpecFile"].Visible = false;
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                dataGridView1.Columns["Select"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                dataGridView1.Columns["Loop"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            }
            catch (Exception ex)
            {
                Form1.myForm1.DealwithException(ex);
            }
        }

        #endregion

        #region 删除测试计划

        //delete current table
        private void toolStripMenuItem_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                IsCreateNewTestPlan = false;
                if (toolStripComboBox1.Items.Count != 0)
                {
                    String iFileName = TestPlanPath + SelectedPlan + ".tps";
                    if (File.Exists(iFileName))
                    {
                        File.Delete(iFileName);
                    }
                    toolStripComboBox1.ComboBox.DataSource = null;
                    toolStripComboBox1.ComboBox.DataSource = GetFileNames(TestPlanPath, "*.tps");
                    toolStripComboBox1.ComboBox.DisplayMember = "TABLE_NAME";
                    toolStripComboBox1.Text = SelectedPlan;
                }
            }
            catch (Exception ex)
            {
                Form1.myForm1.DealwithException(ex);
            }
        }

        #endregion

        #region  保存测试计划

        private void toolStripMenuItem_Save_Click(object sender, EventArgs e)
        {
            SaveTestPlan();
        }

        private void SaveTestPlan()
        { 
            try
            {
                string iFileName = "";
                if (SelectedPlan != toolStripComboBox1.Text && IsCreateNewTestPlan==false)//重命名当前测试计划后的保存操作：SelectedPlan-原测试计划，toolStripComboBox1.Text-新的命名
                {
                    iFileName = TestPlanPath + toolStripComboBox1.Text + ".tps";
                    if (File.Exists(iFileName))
                    {
                        if (DialogResult.No == MessageBox.Show("This plan already exists, do you want to replace it?", "", MessageBoxButtons.YesNo))
                        {
                            return;
                        }
                    }

                    List<string> iList = new List<string>();
                    for (int i = 0; i < toolStripComboBox1.Items.Count; i++)
                    {
                        iList.Add(toolStripComboBox1.Items[i].ToString());

                    }
                    //删除原测试计划
                    iFileName = TestPlanPath + SelectedPlan + ".tps";
                    iList.Remove(SelectedPlan);
                    if (File.Exists(iFileName))
                    {
                        File.Delete(iFileName);
                    }

                    //创建新命名的测试计划
                    iFileName = TestPlanPath + toolStripComboBox1.Text + ".tps";
                    iList.Add(toolStripComboBox1.Text);
                    if (File.Exists(iFileName))
                    {
                        iList.Remove(toolStripComboBox1.Text);
                        File.Delete(iFileName);
                    }
                    toolStripComboBox1.ComboBox.DataSource = null;
                    toolStripComboBox1.ComboBox.DataSource = iList;
                    XmlSaveDVGtoFile(iFileName, dataGridView1, dvgColumnHeaders_Testplan);
                }
                else if (SelectedPlan == toolStripComboBox1.Text && IsCreateNewTestPlan == false)//保存当前测试计划
                {
                    iFileName = TestPlanPath + SelectedPlan + ".tps";
                    if (File.Exists(iFileName))
                    {
                        File.Delete(iFileName);
                    }
                    XmlSaveDVGtoFile(iFileName, dataGridView1, dvgColumnHeaders_Testplan);
                }
                else
                {
                    IsCreateNewTestPlan = false;
                    SaveFileDialog iOpenFolder = new SaveFileDialog();
                    iOpenFolder.Filter = "TPS files (*.tps)|*.tps";
                    iOpenFolder.InitialDirectory = @Form1.myForm1.iConfig.RootPath;
                    iOpenFolder.FileName = toolStripComboBox1.Text;
                    iOpenFolder.Title = "Select test plan folder";
                    if (DialogResult.Cancel == iOpenFolder.ShowDialog()) return;
                    RootPath = Path.GetDirectoryName(iOpenFolder.FileName);
                    Form1.myForm1.iConfig.RootPath = RootPath;
                    UpdateFilePath(RootPath);
                    SelectedPlan = Path.GetFileNameWithoutExtension(iOpenFolder.FileName);
                    iFileName = TestPlanPath + SelectedPlan + ".tps";
                    XmlSaveDVGtoFile(iFileName, dataGridView1, dvgColumnHeaders_Testplan);
                    toolStripComboBox1.ComboBox.DataSource = GetFileNames(TestPlanPath, "*.tps");
                    toolStripComboBox1.ComboBox.DisplayMember = "TABLE_NAME";
                    toolStripComboBox1.ComboBox.Text = Path.GetFileNameWithoutExtension(iOpenFolder.FileName);
  
                }

                //delete instrument config file
                if (File.Exists(TestPlanPath + SelectedPlan + "_InstrumentsSetting.xml"))
                {
                    File.Delete(TestPlanPath + SelectedPlan + "_InstrumentsSetting.xml");
                    Form1.myForm1.DisplayInstrumentConfigButton(false);
                }

                //刷新
                CommonTable = XmlReadFiletoDataTable(iFileName, CommonTable, dvgColumnHeaders_Testplan);
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = CommonTable;
                dataGridView1.Columns["ParaDescriptionFile"].Visible = false;
                dataGridView1.Columns["SpecFile"].Visible = false;
                dataGridView1.Columns["Select"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                dataGridView1.Columns["Loop"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                //dataGridView1.Columns["ParasCount"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                //dataGridView1.Columns["MeasuresCount"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

                IsPlanChanged = false;

               
            }
            catch (Exception ex)
            {
                Form1.myForm1.DealwithException(ex);
            }
        }

        private void ConfigWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsPlanChanged == true || IsCreateNewTestPlan == true)
            {
                switch (MessageBox.Show("Do you want to save changes to document \"" + toolStripComboBox1.Text + ".tps\"", "CommonTest", MessageBoxButtons.YesNoCancel))
              {
                  case DialogResult.Yes:
                      SaveTestPlan();
                      break;
                  case DialogResult.No:
                      break;
                  case DialogResult.Cancel:
                      e.Cancel=true;
                      break;
              }
            }
        }

        #endregion

        #region  选择测试计划combox动作

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {   
                SelectedPlan = toolStripComboBox1.Text;
                CommonTable = null;
                if (SelectedPlan != "System.Data.DataRowView" && SelectedPlan != "")
                {
                    FilterThread = new Thread(new ThreadStart(Filtering));
                    FilterThread.IsBackground = true;
                    FilterThread.Start();
                }
                else
                {
                    dataGridView1.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                Form1.myForm1.DealwithException(ex);
            }
        }

        private void toolStripComboBox1_DropDown(object sender, EventArgs e)
        {
                if (IsPlanChanged == true || IsCreateNewTestPlan == true)
                {
                    switch (MessageBox.Show("Do you want to save changes to document \"" + toolStripComboBox1.Text + ".tps\"", "CommonTest", MessageBoxButtons.YesNo))
                    {
                        case DialogResult.Yes:
                            SaveTestPlan();
                            return;
                        case DialogResult.No:
                            break;
                    }
   
                }
        }

        private void Filtering()
        {
            string mdbFileName = TestPlanPath + SelectedPlan + ".tps";
            CommonTable = XmlReadFiletoDataTable(mdbFileName, CommonTable,dvgColumnHeaders_Testplan);
            MyDelegatePara1 ShowConfig = new MyDelegatePara1(ShowConfigAct);
            this.Invoke(ShowConfig, CommonTable);
            //FilterThread.Abort();

        }

        private void ShowConfigAct(DataTable iDataTable)
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = iDataTable;
            dataGridView1.Columns["ParaDescriptionFile"].Visible = false;
            dataGridView1.Columns["SpecFile"].Visible = false;
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dataGridView1.Columns["Select"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            dataGridView1.Columns["Loop"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            dataGridView1.Columns["IsJudge"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            dataGridView1.Columns["Class"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            IsPlanChanged = false;
        }

        private void toolStripComboBox1_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                toolStripComboBox1.ToolTipText = TestPlanPath + SelectedPlan + ".xml";
            }
            catch (Exception ex)
            {
                Form1.myForm1.DealwithException(ex);
            }
        }
       

        #endregion

        #region 实现编辑测试计划功能

        #region 单击单元格编辑内容

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (cmbTemp.Parent != null) dataGridView1.Controls.Remove(cmbTemp);
                if (cmbTemp_Slave.Parent != null) dataGridView1.Controls.Remove(cmbTemp_Slave);
                if (tbTemp.Parent != null) dataGridView1.Controls.Remove(tbTemp);
                if (btnTemp.Parent != null) dataGridView1.Controls.Remove(btnTemp);


                columIndex = e.ColumnIndex;
                rowIndex = e.RowIndex;
                if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
                {
                    #region Driver column

                    if (e.ColumnIndex == this.dataGridView1.Columns["Driver"].Index)
                    {
                        if (e.RowIndex >= 0)
                        {
                            LoadDriverNames(dataGridView1);
                        }
                    }

                    #endregion

                    #region Class column

                    if (dataGridView1.Rows[e.RowIndex].Cells["Driver"].Value.ToString() != "" && e.ColumnIndex == this.dataGridView1.Columns["Class"].Index)
                    {
                        LoadDllClass(dataGridView1.Rows[e.RowIndex].Cells["Driver"].Value.ToString());
                        cmbTemp.DataSource = null;
                        cmbTemp.DataSource = DllClasses;
                        cmbTemp.Visible = true;
                        cmbTemp.DropDownStyle = ComboBoxStyle.DropDownList;

                        cmbTemp.Width = this.dataGridView1.GetCellDisplayRectangle(columIndex, rowIndex, true).Width;
                        cmbTemp.Height = this.dataGridView1.GetCellDisplayRectangle(columIndex, rowIndex, true).Height;

                        cmbTemp.SelectedIndexChanged += new EventHandler(cmbTemp_SelectedIndexChanged_Class);

                        this.dataGridView1.Controls.Add(cmbTemp);

                        cmbTemp.Location = new System.Drawing.Point(((this.dataGridView1.GetCellDisplayRectangle(columIndex, rowIndex, true).Right) - (cmbTemp.Width)), this.dataGridView1.GetCellDisplayRectangle(columIndex, rowIndex, true).Y);//设置btn显示位置
                    }

                    #endregion

                    #region TestItem column

                    if (dataGridView1.Rows[e.RowIndex].Cells["Driver"].Value.ToString() != "" && dataGridView1.Rows[e.RowIndex].Cells["Class"].Value.ToString() != "" && e.ColumnIndex == this.dataGridView1.Columns["TestItem"].Index)
                    {
                        LoadDllFunction(dataGridView1.Rows[e.RowIndex].Cells["Driver"].Value.ToString(), dataGridView1.Rows[e.RowIndex].Cells["Class"].Value.ToString());
                        cmbTemp.DataSource = null;
                        cmbTemp.DataSource = DllFunctions;
                        cmbTemp.Visible = true;
                        cmbTemp.DropDownStyle = ComboBoxStyle.DropDownList;

                        cmbTemp.Width = this.dataGridView1.GetCellDisplayRectangle(columIndex, rowIndex, true).Width;
                        cmbTemp.Height = this.dataGridView1.GetCellDisplayRectangle(columIndex, rowIndex, true).Height;

                        cmbTemp.SelectedIndexChanged += new EventHandler(cmbTemp_SelectedIndexChanged_forTestItem);

                        this.dataGridView1.Controls.Add(cmbTemp);

                        cmbTemp.Location = new System.Drawing.Point(((this.dataGridView1.GetCellDisplayRectangle(columIndex, rowIndex, true).Right) - (cmbTemp.Width)), this.dataGridView1.GetCellDisplayRectangle(columIndex, rowIndex, true).Y);//设置btn显示位置
                    }

                    #endregion

                    if (dataGridView1.Rows[e.RowIndex].Cells["Driver"].Value.ToString() != "" && dataGridView1.Rows[e.RowIndex].Cells["TestItem"].Value.ToString() != "")
                    {
                        #region Select column

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

                        #endregion

                        #region Loop column

                        if (e.ColumnIndex.Equals(this.dataGridView1.Columns["Loop"].Index))//判断单元格是否是"Loop"列
                        {
                            tbTemp.Visible = true;
                            tbTemp.Text = dataGridView1.Rows[rowIndex].Cells[columIndex].Value.ToString();
                            tbTemp.Width = this.dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Width;//获取单元格高并设置为tbTemp的宽
                            tbTemp.Height = this.dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Height;//获取单元格高并设置为tbTemp的高
                            tbTemp.LostFocus += new EventHandler(tbTemp_LostFocus);
                            tbTemp.KeyPress += new KeyPressEventHandler(tbTemp_KeyPress);
                            this.dataGridView1.Controls.Add(tbTemp);
                            tbTemp.Focus();
                            tbTemp.SelectAll();
                            tbTemp.Location = new System.Drawing.Point(((this.dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Right) - (tbTemp.Width)), this.dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Y);//设置tbTemp显示位置
                            return;
                        }

                        #endregion

                        #region Display Text
                        if (e.ColumnIndex.Equals(this.dataGridView1.Columns["DisplayText"].Index))
                        {
                            tbTemp.Visible = true;
                            tbTemp.Text = dataGridView1.Rows[rowIndex].Cells[columIndex].Value.ToString();
                            tbTemp.Width = this.dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Width;//获取单元格高并设置为tbTemp的宽
                            tbTemp.Height = this.dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Height;//获取单元格高并设置为tbTemp的高
                            tbTemp.LostFocus += new EventHandler(tbTemp_LostFocus_DisplayText);
                            this.dataGridView1.Controls.Add(tbTemp);
                            tbTemp.Focus();
                            tbTemp.SelectionStart = tbTemp.Text.Length;
                            //tbTemp.SelectAll();
                            tbTemp.Location = new System.Drawing.Point(((this.dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Right) - (tbTemp.Width)), this.dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Y);//设置tbTemp显示位置
                            return;
                        }
                        #endregion

                        #region ExecuteType column

                        if (e.ColumnIndex.Equals(this.dataGridView1.Columns["ExecuteType"].Index))//判断单元格是否是"ExecuteType"列
                        {
                            //LoadDllFunction(dataGridView1.Rows[e.RowIndex].Cells["Driver"].Value.ToString());
                            cmbTemp.DataSource = null;
                            cmbTemp.DataSource = ExecuteTypeList;
                            cmbTemp.Visible = true;
                            cmbTemp.DropDownStyle = ComboBoxStyle.DropDownList;
                            cmbTemp.Width = this.dataGridView1.GetCellDisplayRectangle(columIndex, rowIndex, true).Width;
                            cmbTemp.Height = this.dataGridView1.GetCellDisplayRectangle(columIndex, rowIndex, true).Height;
                            cmbTemp.SelectedIndexChanged += new EventHandler(cmbTemp_SelectedIndexChanged_forExecuteType);
                            this.dataGridView1.Controls.Add(cmbTemp);

                            cmbTemp.Location = new System.Drawing.Point(((this.dataGridView1.GetCellDisplayRectangle(columIndex, rowIndex, true).Right) - (cmbTemp.Width)), this.dataGridView1.GetCellDisplayRectangle(columIndex, rowIndex, true).Y);//设置btn显示位置
                        }

                        #endregion

                        #region IsJudge column

                        if (e.ColumnIndex.Equals(this.dataGridView1.Columns["IsJudge"].Index))//判断单元格是否是"IsJudge"列
                        {
                            cmbTemp.DataSource = null;
                            cmbTemp.DataSource = new string[] { "True", "False" };
                            cmbTemp.Visible = true;
                            cmbTemp.DropDownStyle = ComboBoxStyle.DropDownList;
                            cmbTemp.Width = this.dataGridView1.GetCellDisplayRectangle(columIndex, rowIndex, true).Width;
                            cmbTemp.Height = this.dataGridView1.GetCellDisplayRectangle(columIndex, rowIndex, true).Height;
                            cmbTemp.SelectedIndexChanged += new EventHandler(cmbTemp_SelectedIndexChanged_IsJudge);
                            this.dataGridView1.Controls.Add(cmbTemp);

                            cmbTemp.Location = new System.Drawing.Point(((this.dataGridView1.GetCellDisplayRectangle(columIndex, rowIndex, true).Right) - (cmbTemp.Width)), this.dataGridView1.GetCellDisplayRectangle(columIndex, rowIndex, true).Y);//设置btn显示位置
                        }

                        #endregion

                        #region IsShowResult column

                        if (e.ColumnIndex.Equals(this.dataGridView1.Columns["IsShowResult"].Index))//判断单元格是否是"IsShowResult"列
                        {
                            cmbTemp.DataSource = null;
                            cmbTemp.DataSource = new string[] { "True", "False" };
                            cmbTemp.Visible = true;
                            cmbTemp.DropDownStyle = ComboBoxStyle.DropDownList;
                            cmbTemp.Width = this.dataGridView1.GetCellDisplayRectangle(columIndex, rowIndex, true).Width;
                            cmbTemp.Height = this.dataGridView1.GetCellDisplayRectangle(columIndex, rowIndex, true).Height;
                            cmbTemp.SelectedIndexChanged += new EventHandler(cmbTemp_SelectedIndexChanged_IsShowResult);
                            this.dataGridView1.Controls.Add(cmbTemp);

                            cmbTemp.Location = new System.Drawing.Point(((this.dataGridView1.GetCellDisplayRectangle(columIndex, rowIndex, true).Right) - (cmbTemp.Width)), this.dataGridView1.GetCellDisplayRectangle(columIndex, rowIndex, true).Y);//设置btn显示位置
                        }

                        #endregion

                        #region Measure column

                        if (e.ColumnIndex == this.dataGridView1.Columns["Measure"].Index)
                        {
                            btnTemp.Visible = true;
                            btnTemp.Text = "Edit";

                            btnTemp.Width = this.dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Width / 3;//获取单元格高并设置为tbTemp的宽
                            btnTemp.Height = this.dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Height;//获取单元格高并设置为tbTemp的高

                            //为btnTemp添加click  
                            btnTemp.Click += new EventHandler(btnTemp_SpecEidtor_Click);

                            this.dataGridView1.Controls.Add(btnTemp);//dataGridView1中添加控件cmb_Temp
                            btnTemp.Location = new System.Drawing.Point(((this.dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Right) - (btnTemp.Width)), this.dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Y);//设置btn显示位置
                            //dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                            //this.dataGridView1.Refresh();

                            return;
                        }

                        #endregion

                        #region Para column

                        if (e.ColumnIndex == this.dataGridView1.Columns["Para"].Index)
                        {
                            btnTemp.Visible = true;
                            btnTemp.Text = "Edit";

                            btnTemp.Width = this.dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Width / 3;//获取单元格高并设置为tbTemp的宽
                            btnTemp.Height = this.dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Height;//获取单元格高并设置为tbTemp的高

                            //为btnTemp添加click
                            switch (Form1.myForm1.ActiveUser.UserType)
                            {
                                case "Programmer":
                                    btnTemp.Click += new EventHandler(btnTemp_EditPara_Click_Programmer);
                                    break;
                                case "Administrator":
                                    btnTemp.Click += new EventHandler(btnTemp_EditPara_Click_Administrator);
                                    break;
                            }

                            this.dataGridView1.Controls.Add(btnTemp);//dataGridView1中添加控件cmb_Temp
                            btnTemp.Location = new System.Drawing.Point(((this.dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Right) - (btnTemp.Width)), this.dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Y);//设置btn显示位置
                            return;

                        }

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                Form1.myForm1.DealwithException(ex);
            }
        }

        #region 动态控件相关事件

        void cmbTemp_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbTemp.SelectedIndexChanged -= cmbTemp_SelectedIndexChanged;
                DataGridView iDataGridView = (DataGridView)cmbTemp.Parent;
                if (iDataGridView == null) return;
                if (columIndex == iDataGridView.Columns["Driver"].Index)
                {
                    this.LoadDllClass(cmbTemp.Text);
                    if (DllClasses.Count == 1)
                    {
                        iDataGridView.Rows[rowIndex].Cells["Class"].Value = DllClasses[0];
                        iDataGridView.Rows[rowIndex].Cells["Driver"].Value = cmbTemp.Text;
                        iDataGridView.Controls.Remove(cmbTemp);
                        return;
                    }
                    cmbTemp_Slave.DataSource = null;
                    cmbTemp_Slave.DataSource = DllClasses;
                    cmbTemp_Slave.Visible = true;
                    cmbTemp_Slave.DropDownStyle = ComboBoxStyle.DropDownList;

                    cmbTemp_Slave.Width = iDataGridView.GetCellDisplayRectangle(columIndex + 1, rowIndex, true).Width;
                    cmbTemp_Slave.Height = iDataGridView.GetCellDisplayRectangle(columIndex + 1, rowIndex, true).Height;
                    cmbTemp_Slave.SelectedIndexChanged += new EventHandler(cmbTemp_Slave_SelectedIndexChanged);

                    iDataGridView.Controls.Add(cmbTemp_Slave);

                    cmbTemp_Slave.Location = new System.Drawing.Point(((iDataGridView.GetCellDisplayRectangle(columIndex + 1, rowIndex, true).Right) - (cmbTemp_Slave.Width)), iDataGridView.GetCellDisplayRectangle(columIndex + 1, rowIndex, true).Y);//设置btn显示位置
                    iDataGridView.Rows[rowIndex].Cells["Driver"].Value = cmbTemp.Text;

                }
            }
            catch (Exception ex)
            {
                Form1.myForm1.DealwithException(ex);
            }
        }

        void cmbTemp_Slave_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbTemp_Slave.SelectedIndexChanged -= cmbTemp_Slave_SelectedIndexChanged;
                if (cmbTemp_Slave.Parent == null) return;

                DataGridView iDataGridView = (DataGridView)cmbTemp_Slave.Parent;
                if (columIndex == iDataGridView.Columns["Driver"].Index)
                {
                    iDataGridView.Rows[rowIndex].Cells["Class"].Value = cmbTemp_Slave.Text;
                    iDataGridView.Controls.Remove(cmbTemp_Slave);
                    iDataGridView.Rows[rowIndex].Cells["Driver"].Value = cmbTemp.Text;
                    iDataGridView.Controls.Remove(cmbTemp);
                }
            }
            catch (Exception ex)
            {
                Form1.myForm1.DealwithException(ex);
            }
        }

        void cmbTemp_SelectedIndexChanged_Class(object sender, EventArgs e)
        {
            try
            {
                cmbTemp.SelectedIndexChanged -= cmbTemp_SelectedIndexChanged_Class;
                if (cmbTemp.Parent == null) return;
                DataGridView iDataGridView = (DataGridView)cmbTemp.Parent;
                if (columIndex == this.dataGridView1.Columns["Class"].Index)
                {
                    iDataGridView.Rows[rowIndex].Cells["Class"].Value = cmbTemp.Text;
                    iDataGridView.Controls.Remove(cmbTemp);
                }
            }
            catch (Exception ex)
            {
                Form1.myForm1.DealwithException(ex);
            }
        }

        void cmbTemp_SelectedIndexChanged_forTestItem(object sender, EventArgs e)
        {
            try
            {
                cmbTemp.SelectedIndexChanged -= cmbTemp_SelectedIndexChanged_forTestItem;
                if (cmbTemp.Parent == null) return;
                DataGridView iDataGridView = (DataGridView)cmbTemp.Parent;
                if (columIndex == this.dataGridView1.Columns["TestItem"].Index)
                {
                    iDataGridView.Rows[rowIndex].Cells["TestItem"].Value = cmbTemp.Text;
                    iDataGridView.Controls.Remove(cmbTemp);
                    if (iDataGridView.Rows[rowIndex].Cells["Para"].Value.ToString() == "")
                    {
                        LoadFunctionParaAndRetrun(iDataGridView.Rows[rowIndex].Cells["Driver"].Value.ToString()
                            , iDataGridView.Rows[rowIndex].Cells["Class"].Value.ToString()
                            , iDataGridView.Rows[rowIndex].Cells["TestItem"].Value.ToString()
                        , iDataGridView);
                    }
                }
            }
            catch (Exception ex)
            {
                Form1.myForm1.DealwithException(ex);
            }
        }
     
        void cmbTemp_SelectedIndexChanged_forExecuteType(object sender, EventArgs e)
        {
            try
            {
                cmbTemp.SelectedIndexChanged -= cmbTemp_SelectedIndexChanged_forExecuteType;
                if (cmbTemp.Parent == null) return;
                DataGridView iDataGridView = (DataGridView)cmbTemp.Parent;
                if (columIndex == this.dataGridView1.Columns["ExecuteType"].Index)
                {
                    iDataGridView.Rows[rowIndex].Cells["ExecuteType"].Value = cmbTemp.Text;
                    iDataGridView.Controls.Remove(cmbTemp);
                }
            }
            catch (Exception ex)
            {
                Form1.myForm1.DealwithException(ex);
            }
        }

        void cmbTemp_SelectedIndexChanged_IsJudge(object sender, EventArgs e)
        {
            try
            {
                cmbTemp.SelectedIndexChanged -= cmbTemp_SelectedIndexChanged_IsJudge;
                if (cmbTemp.Parent == null) return;
                DataGridView iDataGridView = (DataGridView)cmbTemp.Parent;
                if (columIndex == this.dataGridView1.Columns["IsJudge"].Index)
                {
                    iDataGridView.Rows[rowIndex].Cells["IsJudge"].Value = cmbTemp.Text;
                    iDataGridView.Controls.Remove(cmbTemp);
                }
            }
            catch (Exception ex)
            {
                Form1.myForm1.DealwithException(ex);
            }
        }

        void cmbTemp_SelectedIndexChanged_IsShowResult(object sender, EventArgs e)
        {
            try
            {
                cmbTemp.SelectedIndexChanged -= cmbTemp_SelectedIndexChanged_IsShowResult;
                if (cmbTemp.Parent == null) return;
                DataGridView iDataGridView = (DataGridView)cmbTemp.Parent;
                if (columIndex == this.dataGridView1.Columns["IsShowResult"].Index)
                {
                    iDataGridView.Rows[rowIndex].Cells["IsShowResult"].Value = cmbTemp.Text;
                    iDataGridView.Controls.Remove(cmbTemp);
                }
            }
            catch (Exception ex)
            {
                Form1.myForm1.DealwithException(ex);
            }
        }

        void tbTemp_LostFocus(object sender, EventArgs e)
        {
            try
            {
                tbTemp.LostFocus -= tbTemp_LostFocus;
                dataGridView1.Rows[rowIndex].Cells[columIndex].Value = tbTemp.Text.Trim();
                this.dataGridView1.Controls.Remove(tbTemp);
                //this.dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                Form1.myForm1.DealwithException(ex);
            }
        }

        void tbTemp_LostFocus_DisplayText(object sender, EventArgs e)
        {
            try
            {
                tbTemp.LostFocus -= tbTemp_LostFocus;
                dataGridView1.Rows[rowIndex].Cells[columIndex].Value = tbTemp.Text.Trim();
                this.dataGridView1.Controls.Remove(tbTemp);
                //this.dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                Form1.myForm1.DealwithException(ex);
            }
        }

        void tbTemp_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                tbTemp.KeyPress -= tbTemp_KeyPress;
                if (e.KeyChar.ToString() != Keys.Back.ToString())  //  
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), "\\d+")) e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Form1.myForm1.DealwithException(ex);
            }
        }

        void btnTemp_SpecEidtor_Click(object sender, EventArgs e)
        {
            try
            {
                btnTemp.Click -= btnTemp_SpecEidtor_Click;
                if (columIndex == this.dataGridView1.Columns["Measure"].Index)
                {
                    //DataGridViw,Edit parameters
                    if (btnTemp.Parent == null) return;
                    DataGridView iDataGridView = (DataGridView)btnTemp.Parent;
                    if (iDataGridView.ColumnCount != 0)
                    {
                        if (iDataGridView.Rows[rowIndex].Cells["TestItem"].Value.ToString() != "")
                        {
                            currentTestItem = iDataGridView.Rows[rowIndex].Cells["TestItem"].Value.ToString();
                            switch (Form1.myForm1.ActiveUser.UserType)
                            {
                                case "Programmer":
                                    Organization_EditSpec organization_Programmer = new Organization_EditSpec();
                                    ReadSpecforCurrentTestItem(organization_Programmer, SelectedPlan, iDataGridView);
                                    break;
                                case "Administrator":
                                    Organization_EditSpec_HideEditor organization_Administrator = new Organization_EditSpec_HideEditor();
                                    ReadSpecforCurrentTestItem(organization_Administrator, SelectedPlan, iDataGridView);
                                    break;
                            }

                            iDataGridView.Controls.Remove(btnTemp);
                            iDataGridView.Refresh();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Form1.myForm1.DealwithException(ex);
            }
        }

        void btnTemp_EditPara_Click_Administrator(object sender, EventArgs e)
        {
            try
            {
                btnTemp.Click -= btnTemp_EditPara_Click_Administrator;
                if (columIndex == this.dataGridView1.Columns["Para"].Index)
                {
                    //DataGridViw,Edit parameters
                    if (btnTemp.Parent == null) return;
                    DataGridView iDataGridView = (DataGridView)btnTemp.Parent;
                    if (iDataGridView.ColumnCount != 0)
                    {
                        if (columIndex == iDataGridView.Columns["Para"].Index)
                        {
                            Parameter_set nw = new Parameter_set();
                            Organization_EditTestPlan organization = new Organization_EditTestPlan();

                            String paras = iDataGridView.Rows[rowIndex].Cells["Para"].Value.ToString();
                            string[] descriptionFiles = iDataGridView.Rows[rowIndex].Cells["ParaDescriptionFile"].Value.ToString().Split(',');
                            if (paras != "")
                            {
                                string fileName = "";
                                String[] tempStr = paras.Split(',');
                                string keyString = iDataGridView.Rows[rowIndex].Cells["Driver"].Value.ToString() + "-" + iDataGridView.Rows[rowIndex].Cells["Class"].Value.ToString() + "-" + iDataGridView.Rows[rowIndex].Cells["TestItem"].Value.ToString() + "-";
                                for (int i = 0; i < tempStr.Length; i++)
                                {
                                    String[] tempStr1 = tempStr[i].Split('=');
                                    Para newPara = new Para();
                                    newPara.Index = rowIndex.ToString();
                                    newPara.Driver = iDataGridView.Rows[rowIndex].Cells["Driver"].Value.ToString();
                                    newPara.TestItem = iDataGridView.Rows[rowIndex].Cells["TestItem"].Value.ToString();
                                    newPara.Name = tempStr1[0].Trim();
                                    newPara.Value = tempStr1[1];
                                    fileName = "";
                                    ///
                                    foreach (string idescriptionFile in descriptionFiles)
                                    {
                                        if (GetExactParaOrSpecName(keyString, idescriptionFile) == newPara.Name)
                                        {
                                            fileName = DescriptionPath + idescriptionFile + ".html";
                                            break;
                                        }
                                    }
                                    ///
                                    newPara.Description = ReadParameterDescription(fileName);
                                    organization.Parameters.Add(newPara);
                                }
                            }
                            nw.Show();
                            nw.TopMost = true;
                            nw.BringToFront();
                            this.TopMost = false;

                            nw.loadPropertyGridDVG1(organization);

                            iDataGridView.Controls.Remove(btnTemp);

                            iDataGridView.Refresh();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Form1.myForm1.DealwithException(ex);
            }
        }

        void btnTemp_EditPara_Click_Programmer(object sender, EventArgs e)
        {
            try
            {
                btnTemp.Click -= btnTemp_EditPara_Click_Programmer;
                if (columIndex == this.dataGridView1.Columns["Para"].Index)
                {
                    //DataGridViw,Edit parameters
                    if (btnTemp.Parent == null) return;
                    DataGridView iDataGridView = (DataGridView)btnTemp.Parent;
                    if (iDataGridView.ColumnCount != 0)
                    {
                        if (columIndex == iDataGridView.Columns["Para"].Index)
                        {
                            Parameter_set nw = new Parameter_set();
                            Organization_EditPara organization = new Organization_EditPara();
                            string paras = iDataGridView.Rows[rowIndex].Cells["Para"].Value.ToString();
                            string[] descriptionFiles = iDataGridView.Rows[rowIndex].Cells["ParaDescriptionFile"].Value.ToString().Split(',');

                            if (paras != "")
                            {
                                string fileName = "";
                                String[] tempStr = paras.Split(',');
                                string keyString = iDataGridView.Rows[rowIndex].Cells["Driver"].Value.ToString() + "-" + iDataGridView.Rows[rowIndex].Cells["Class"].Value.ToString() + "-" + iDataGridView.Rows[rowIndex].Cells["TestItem"].Value.ToString() + "-";


                                for (int i = 0; i < tempStr.Length; i++)
                                {
                                    String[] tempStr1 = tempStr[i].Split('=');
                                    Para newPara = new Para();
                                    newPara.Index = rowIndex.ToString();
                                    newPara.Driver = iDataGridView.Rows[rowIndex].Cells["Driver"].Value.ToString();
                                    newPara.TestItem = iDataGridView.Rows[rowIndex].Cells["TestItem"].Value.ToString();
                                    newPara.Name = tempStr1[0].Trim();
                                    newPara.Value = tempStr1[1];
                                    ///
                                    fileName = "";
                                    foreach (string idescriptionFile in descriptionFiles)
                                    {
                                        if (GetExactParaOrSpecName(keyString, idescriptionFile) == newPara.Name)
                                        {
                                            fileName = DescriptionPath + idescriptionFile + ".html";
                                            break;
                                        }
                                    }
                                    ///
                                    //fileName = DescriptionPath + iDataGridView.Rows[rowIndex].Cells["ParaDescriptionFile"].Value.ToString() + ".html";
                                    newPara.Description = ReadParameterDescription(fileName);
                                    organization.Parameters.Add(newPara);
                                }
                            }
                            nw.Show();
                            nw.TopMost = true;
                            nw.BringToFront();
                            this.TopMost = false;

                            nw.loadPropertyGridDVG1(organization);

                            iDataGridView.Controls.Remove(btnTemp);

                            iDataGridView.Refresh();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Form1.myForm1.DealwithException(ex);
            }
        }

        #endregion

        #endregion

        #region//添加删除行
      
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.Button != System.Windows.Forms.MouseButtons.Right) return;
                if (dataGridView1.Rows.Count == 1)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        selectedRowIndex = e.RowIndex;
                        Point curiorPosition = dataGridView1.PointToClient(Cursor.Position);
                        MTdgv1.Show(dataGridView1, curiorPosition, LeftRightAlignment.Right);
                    }

                }
                if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        selectedRowIndex = e.RowIndex;
                        Point curiorPosition = dataGridView1.PointToClient(Cursor.Position);
                        MTdgv1.Show(dataGridView1, curiorPosition, LeftRightAlignment.Right);
                    }
                }
            }
            catch (Exception ex)
            {
                Form1.myForm1.DealwithException(ex);
            }
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right && dataGridView1.Rows.Count == 0)
            {
                selectedRowIndex = 0;
                Point curiorPosition = dataGridView1.PointToClient(Cursor.Position);
                MTdgv1.Show(dataGridView1, curiorPosition, LeftRightAlignment.Right);
            }
        }
    

        private void CreateMenuItem_MTdgv1()
        {
            ContextMenuEx menu1 = new ContextMenuEx();
            menu1.Text = "Add New Row";
            MTdgv1.MenuItems.Add(menu1);
            MTdgv1.MenuItems[0].Click += new EventHandler(MTdgvItem0_Click);
            ContextMenuSplitLine line0 = new ContextMenuSplitLine();
            MTdgv1.MenuItems.Add(line0);
            menu1 = new ContextMenuEx();
            menu1.Text = "Delete Current Row";
            MTdgv1.MenuItems.Add(menu1);
            MTdgv1.MenuItems[2].Click += new EventHandler(MTdgvItem2_Click);
            ContextMenuSplitLine line1 = new ContextMenuSplitLine();
            MTdgv1.MenuItems.Add(line1);
        }

        void MTdgvItem0_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedRowIndex <= CommonTable.Rows.Count)
                {
                    dataGridView1.DataSource = null;
                    DataRow newRow = CommonTable.NewRow();
                    CommonTable.Rows.InsertAt(newRow, (selectedRowIndex + 1));
                    selectedRowIndex = 0;
                    dataGridView1.DataSource = CommonTable;
                    dataGridView1.Columns["ParaDescriptionFile"].Visible = false;
                    dataGridView1.Columns["SpecFile"].Visible = false;
                    //dataGridView1.Refresh();
                }
            }
            catch (Exception ex)
            {
                Form1.myForm1.DealwithException(ex);
            }
        }

        void MTdgvItem2_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedRowIndex < CommonTable.Rows.Count)
                {
                    dataGridView1.DataSource = null;
                    CommonTable.Rows.Remove(CommonTable.Rows[selectedRowIndex]);
                    selectedRowIndex = 0;
                    dataGridView1.DataSource = CommonTable;
                    dataGridView1.Columns["ParaDescriptionFile"].Visible = false;
                    dataGridView1.Columns["SpecFile"].Visible = false;
                    //dataGridView1.Refresh();
                }
            }
            catch (Exception ex)
            {
                Form1.myForm1.DealwithException(ex);
            }
        }

        #endregion

        #region//实现拖放功能
  
        private void dataGridView1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
                {
                    // If the mouse moves outside the rectangle, start the drag.
                    if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
                    {
                        // Proceed with the drag and drop, passing in the list item.                    
                        DragDropEffects dropEffect = dataGridView1.DoDragDrop(dataGridView1.Rows[indexofsourcerow], DragDropEffects.Move);
                    }
                }
            }
            catch (Exception ex)
            {
                Form1.myForm1.DealwithException(ex);
            }
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                // Get the index of the item the mouse is below.
                indexofsourcerow = dataGridView1.HitTest(e.X, e.Y).RowIndex;
                if (indexofsourcerow != -1)
                {
                    // Remember the point where the mouse down occurred. 
                    // The DragSize indicates the size that the mouse can move 
                    // before a drag event should be started.                
                    Size dragSize = SystemInformation.DragSize;

                    // Create a rectangle using the DragSize, with the mouse position being
                    // at the center of the rectangle.
                    dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);

                }
                else
                    // Reset the rectangle if the mouse is not over an item in the ListBox.
                    dragBoxFromMouseDown = Rectangle.Empty;
            }
            catch (Exception ex)
            {
                Form1.myForm1.DealwithException(ex);
            }
        }
     
        private void dataGridView1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                // The mouse locations are relative to the screen, so they must be 
                // converted to client coordinates.
                Point clientPoint = dataGridView1.PointToClient(new Point(e.X, e.Y));

                // Get the row index of the item the mouse is below. 
                indexoftargetrow = dataGridView1.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

                // If the drag operation was a move then remove and insert the row.
                if (e.Effect == DragDropEffects.Move)
                {
                    //    DataGridViewRow rowToMove = e.Data.GetData(
                    //        typeof(DataGridViewRow)) as DataGridViewRow;
                    //    dataGridView1.Rows.RemoveAt(indexofsourcerow);
                    //    dataGridView1.Rows.Insert(indexoftargetrow, rowToMove);

                    //}
                    if (indexoftargetrow != indexofsourcerow && indexoftargetrow >= 0)
                    {
                        DataRow rowToMove = CommonTable.NewRow();
                        rowToMove.ItemArray = CommonTable.Rows[indexofsourcerow].ItemArray.Clone() as object[];


                        CommonTable.Rows.RemoveAt(indexofsourcerow);
                        CommonTable.Rows.InsertAt(rowToMove, indexoftargetrow);
                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = CommonTable;
                        dataGridView1.Columns["ParaDescriptionFile"].Visible = false;
                        dataGridView1.Columns["SpecFile"].Visible = false;
                        dataGridView1.Columns["Select"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                        dataGridView1.Columns["Loop"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                        //dataGridView1.Columns["ParasCount"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                        //dataGridView1.Columns["MeasuresCount"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                        //dataGridView1.Refresh();
                        indexofsourcerow = -1;
                        indexoftargetrow = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                Form1.myForm1.DealwithException(ex);
            }
        }


        #endregion

        #region 判断Datagridview是否修改

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            IsPlanChanged = true;
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            IsPlanChanged = true;
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            IsPlanChanged = true;
        }

        #endregion

        #endregion

        #region  functions

        public void UpdateFilePath(string iRootPath)
        {
            if (iRootPath == "") return;
            iRootPath=iRootPath.TrimEnd('\\');
            TestPlanPath = iRootPath + "\\";
            if (!Directory.Exists(TestPlanPath))
            {
                Directory.CreateDirectory(TestPlanPath);
            }
            
            SpecPath = TestPlanPath + "Spec\\";
            if (!Directory.Exists(SpecPath))
            {
                Directory.CreateDirectory(SpecPath);
            }
            DescriptionPath = TestPlanPath + "Description\\";
            if (!Directory.Exists(DescriptionPath))
            {
                Directory.CreateDirectory(DescriptionPath);
            }
            TestResultPath = TestPlanPath + "TestResult\\";
            if (!Directory.Exists(TestResultPath))
            {
                Directory.CreateDirectory(TestResultPath);
            }
            TestResultPath_txt = TestResultPath + "TXT\\";
            if (!Directory.Exists(TestResultPath_txt))
            {
                Directory.CreateDirectory(TestResultPath_txt);
            }
            TestResultPath_csv = TestResultPath + "CSV\\";
            if (!Directory.Exists(TestResultPath_csv))
            {
                Directory.CreateDirectory(TestResultPath_csv);
            }
            if (Form1.myForm1 != null)
            {
                Form1.myForm1.iUniversalVariant.TestResultPath = TestResultPath;
            }

        }

        private List<string> GetFileNames(string folderPath, string fileFormat)
        {
            List<string> iList = new List<string>();
            foreach (string iPath in Directory.GetFiles(folderPath, fileFormat))
            {
                iList.Add(Path.GetFileNameWithoutExtension(iPath).Trim());
            }
            return iList;
        }

        private void LoadDllFunction(string str_dllName,string ClassName)
        {
            if (str_dllName == "")
            {
                return;
            }
            //String folder = Application.StartupPath;
            //string dllFileName = folder + "\\" + str_dllName + ".dll";
            string dllFileName = Form1.myForm1.DllPath + str_dllName + ".dll";
            DllFunctions.Clear();

            Assembly assembly = Assembly.LoadFrom(dllFileName);
           
            foreach (Type type in assembly.GetTypes())
            {
               if (type.Name == ClassName)
               {
                   MethodInfo[] _Methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                    foreach (MethodInfo mMethodInfo in _Methods)
                    {
                       
                            DllFunctions.Add(mMethodInfo.Name);
                    }
          
                }
            }
           

        }

        private void LoadDllClass(string str_dllName)
        {
            if (str_dllName == "")
            {
                return;
            }
            //String folder = Application.StartupPath;
            //string dllFileName = folder + "\\" + str_dllName + ".dll";
            string dllFileName = Form1.myForm1.DllPath + str_dllName + ".dll";
            DllClasses.Clear();

            Assembly assembly = Assembly.LoadFrom(dllFileName);

            foreach (Type type in assembly.GetTypes())
            {
                if (!type.IsClass || type.IsNotPublic)
                {
                    continue;
                }
                else
                {
                    DllClasses.Add(type.Name);
                }
            }


        }

        private void LoadFunctionParaAndRetrun(string str_dllName,string className, string functionName, DataGridView iDataGridView)
        {
            if (str_dllName == "")
            {
                return;
            }
            //string folder = Application.StartupPath;
            //string dllFileName = folder + "\\" + str_dllName + ".dll";
            string dllFileName = Form1.myForm1.DllPath + str_dllName + ".dll";
            DllFunctions.Clear();

            Assembly assembly = Assembly.LoadFrom(dllFileName);

            foreach (Type type in assembly.GetTypes())
            {
                if (type.Name == className)
                {
                    MethodInfo[] _Methods = type.GetMethods();
                    foreach (MethodInfo mMethodInfo in _Methods)
                    {
                        if (mMethodInfo.Name == functionName)
                        {
                            ParameterInfo[] ParameterInfos = mMethodInfo.GetParameters();
                            if (iDataGridView.Rows[rowIndex].Cells["Para"].Value.ToString() == "")
                            {
                                string tString = "";
                                foreach (ParameterInfo iParameterInfo in ParameterInfos)
                                {
                                    tString += iParameterInfo.Name + "=" + iParameterInfo.DefaultValue.ToString() + ",";
                                }
                                iDataGridView.Rows[rowIndex].Cells["Para"].Value = tString.Trim(',');
                                iDataGridView.Rows[rowIndex].Cells["ReturnType"].Value = mMethodInfo.ReturnType.ToString();
                            }
                        }
                    }

                }
            }


        }

        private void GetDllNames()
        {
            DllNames.Clear();
            String folder = Application.StartupPath+"\\lib";
            var files = Directory.GetFiles(folder, "*.dll");
            FileInfo mFileInfo;
            //load each assembly.
            foreach (string file in files)
            {
                mFileInfo = new FileInfo(file);
                if (mFileInfo.Name!="Interop.ADODB.dll" && mFileInfo.Name!="Interop.ADOX.dll" && mFileInfo.Name!="LibIndex.dll"
                    && mFileInfo.Name != "NationalInstruments.Common.dll" && mFileInfo.Name != "TransTestStructure.dll" && mFileInfo.Name != "ZedGraph.dll"
                     && mFileInfo.Name != "NationalInstruments.VisaNS.dll" && mFileInfo.Name != "LumenWorks.Framework.IO.dll")
                {
                    DllNames.Add(mFileInfo.Name.Substring(0, (mFileInfo.Name.Length - 4)));
                }
            }
            
        }

        //load dll names for "Driver" colomn
        private void LoadDriverNames(DataGridView iDataGridView)
        {
            if (DllNames == null)
            {
                return;
            }
            cmbTemp.DataSource = DllNames;
            cmbTemp.Visible = true;
            cmbTemp.DropDownStyle = ComboBoxStyle.DropDownList;

            cmbTemp.Width = iDataGridView.GetCellDisplayRectangle(columIndex, rowIndex, true).Width;//获取单元格宽并设置为cmb_Temp的宽
            cmbTemp.Height = iDataGridView.GetCellDisplayRectangle(columIndex, rowIndex, true).Height;//获取单元格高并设置为cmb_Temp的高

            //为combox添加selectindex消息响应
            cmbTemp.SelectedIndexChanged += new EventHandler(cmbTemp_SelectedIndexChanged);
            iDataGridView.Controls.Add(cmbTemp);//dataGridView1中添加控件cmb_Temp

            cmbTemp.Location = new System.Drawing.Point(((iDataGridView.GetCellDisplayRectangle(columIndex, rowIndex, true).Right) - (cmbTemp.Width)), iDataGridView.GetCellDisplayRectangle(columIndex, rowIndex, true).Y);//设置btn显示位置

            iDataGridView.Refresh();
        }

        private void ReadSpecforCurrentTestItem(Organization_EditSpec organization,string iEditingProjectName, DataGridView iDataGridView)
        {
            if (iEditingProjectName != "" && iEditingProjectName != "System.Data.DataRowView")
            {
                Parameter_set nw = new Parameter_set();
               
                organization.Parameters.driver = iDataGridView.Rows[rowIndex].Cells["Driver"].Value.ToString();
                organization.Parameters._class = iDataGridView.Rows[rowIndex].Cells["Class"].Value.ToString();
                organization.Parameters.testItem = iDataGridView.Rows[rowIndex].Cells["TestItem"].Value.ToString();
                string[] MeasureNames = iDataGridView.Rows[rowIndex].Cells["Measure"].Value.ToString().Split(',');
                string[] SpecFileNames = iDataGridView.Rows[rowIndex].Cells["SpecFile"].Value.ToString().Split(',');
                string keyString = iDataGridView.Rows[rowIndex].Cells["Driver"].Value.ToString() + "-" + iDataGridView.Rows[rowIndex].Cells["Class"].Value.ToString() + "-" + iDataGridView.Rows[rowIndex].Cells["TestItem"].Value.ToString() + "-";

                if (MeasureNames.Length > 0)
                {
                    string iFileName = "";
                    for (int i = 0; i < MeasureNames.Length; i++)
                    {
                        Spec newSpec = new Spec();
                        newSpec.Index = rowIndex;
                        newSpec.driver = iDataGridView.Rows[rowIndex].Cells["Driver"].Value.ToString();
                        newSpec._class = iDataGridView.Rows[rowIndex].Cells["Class"].Value.ToString();
                        newSpec.testItem = iDataGridView.Rows[rowIndex].Cells["TestItem"].Value.ToString();
                        newSpec.MeasureName = MeasureNames[i];
                       
                        foreach (string iSpecFileName in SpecFileNames)
                        {
                            if (GetExactParaOrSpecName(keyString,iSpecFileName) == newSpec.MeasureName)
                            {
                                iFileName = SpecPath + iSpecFileName + ".xml";
                                break;
                            }
                        }
                        if (!File.Exists(iFileName) || new FileInfo(iFileName).Length == 0)
                        {
                            newSpec.LowLimit = "";
                            newSpec.UpLimit = "";
                            newSpec.Unit = "";
                            newSpec.SpecNumber = "";
                            newSpec.SpecName = "";
                        }
                        else
                        {
                            #region read

                            using (FileStream fileSteam = File.OpenRead(iFileName))
                            {
                                XmlReaderSettings iXmlReaderSettings = new XmlReaderSettings(); ;
                                iXmlReaderSettings.ConformanceLevel = ConformanceLevel.Document;

                                using (XmlReader iXmlReader = XmlReader.Create(fileSteam, iXmlReaderSettings))
                                {
                                    iXmlReader.MoveToContent();
                                    while (iXmlReader.Read())
                                    {
                                        if (iXmlReader.Name.Contains(newSpec.MeasureName) && iXmlReader.NodeType == XmlNodeType.Element)
                                        {
                                            newSpec.LowLimit = XmlReadString(iXmlReader);
                                            newSpec.UpLimit = XmlReadString(iXmlReader);
                                            newSpec.Unit = XmlReadString(iXmlReader);
                                            newSpec.SpecNumber = XmlReadString(iXmlReader);
                                            newSpec.SpecName = XmlReadString(iXmlReader);
                                        }
                                    }
                                }
                            }


                            #endregion
                        }
                        organization.Parameters.Add(newSpec);
                    }

                }
                else
                {
                    Spec newSpec = new Spec();
                    newSpec.Index = rowIndex;
                    newSpec.driver = iDataGridView.Rows[rowIndex].Cells["Driver"].Value.ToString();
                    newSpec._class = iDataGridView.Rows[rowIndex].Cells["Class"].Value.ToString();
                    newSpec.testItem = iDataGridView.Rows[rowIndex].Cells["TestItem"].Value.ToString();
                    newSpec.LowLimit = "";
                    newSpec.UpLimit = "";
                    newSpec.Unit = "";
                    newSpec.MeasureName = "";
                    newSpec.SpecNumber = "";
                    newSpec.SpecName = "";
                    organization.Parameters.Add(newSpec);
                }

                nw.Show();
                nw.TopMost = true;
                nw.BringToFront();
                this.TopMost = false;

                nw.loadPropertyGridDVG1(organization);

            }
            else
            {
                MessageBox.Show("Select a project first!");
            }
        }

        private void ReadSpecforCurrentTestItem(Organization_EditSpec_HideEditor organization, string iEditingProjectName, DataGridView iDataGridView)
        {
            if (iEditingProjectName != "" && iEditingProjectName != "System.Data.DataRowView")
            {
                Parameter_set nw = new Parameter_set();

                organization.Parameters.driver = iDataGridView.Rows[rowIndex].Cells["Driver"].Value.ToString();
                organization.Parameters._class = iDataGridView.Rows[rowIndex].Cells["Class"].Value.ToString();
                organization.Parameters.testItem = iDataGridView.Rows[rowIndex].Cells["TestItem"].Value.ToString();
                string[] MeasureNames = iDataGridView.Rows[rowIndex].Cells["Measure"].Value.ToString().Split(',');
                string[] SpecFileNames = iDataGridView.Rows[rowIndex].Cells["SpecFile"].Value.ToString().Split(',');
                string keyString = iDataGridView.Rows[rowIndex].Cells["Driver"].Value.ToString() + "-" + iDataGridView.Rows[rowIndex].Cells["Class"].Value.ToString() + "-" + iDataGridView.Rows[rowIndex].Cells["TestItem"].Value.ToString() + "-";

                if (MeasureNames.Length > 0)
                {
                    string iFileName = "";
                    for (int i = 0; i < MeasureNames.Length; i++)
                    {
                        Spec newSpec = new Spec();
                        newSpec.Index = rowIndex;
                        newSpec.driver = iDataGridView.Rows[rowIndex].Cells["Driver"].Value.ToString();
                        newSpec._class = iDataGridView.Rows[rowIndex].Cells["Class"].Value.ToString();
                        newSpec.testItem = iDataGridView.Rows[rowIndex].Cells["TestItem"].Value.ToString();
                        newSpec.MeasureName = MeasureNames[i];

                        foreach (string iSpecFileName in SpecFileNames)
                        {
                            if (GetExactParaOrSpecName(keyString,iSpecFileName) == newSpec.MeasureName)
                            {
                                iFileName = SpecPath + iSpecFileName + ".xml";
                                break;
                            }
                        }
                        if (!File.Exists(iFileName) || new FileInfo(iFileName).Length == 0)
                        {
                            newSpec.LowLimit = "";
                            newSpec.UpLimit = "";
                            newSpec.Unit = "";
                            newSpec.SpecNumber = "";
                            newSpec.SpecName = "";
                        }
                        else
                        {
                            #region read

                            using (FileStream fileSteam = File.OpenRead(iFileName))
                            {
                                XmlReaderSettings iXmlReaderSettings = new XmlReaderSettings(); ;
                                iXmlReaderSettings.ConformanceLevel = ConformanceLevel.Document;

                                using (XmlReader iXmlReader = XmlReader.Create(fileSteam, iXmlReaderSettings))
                                {
                                    iXmlReader.MoveToContent();
                                    while (iXmlReader.Read())
                                    {
                                        if (iXmlReader.Name.Contains(newSpec.MeasureName) && iXmlReader.NodeType == XmlNodeType.Element)
                                        {
                                            newSpec.LowLimit = XmlReadString(iXmlReader);
                                            newSpec.UpLimit = XmlReadString(iXmlReader);
                                            newSpec.Unit = XmlReadString(iXmlReader);
                                            newSpec.SpecNumber = XmlReadString(iXmlReader);
                                            newSpec.SpecName = XmlReadString(iXmlReader);
                                        }
                                    }
                                }
                            }


                            #endregion
                        }
                        organization.Parameters.Add(newSpec);
                    }

                }
                else
                {
                    Spec newSpec = new Spec();
                    newSpec.Index = rowIndex;
                    newSpec.driver = iDataGridView.Rows[rowIndex].Cells["Driver"].Value.ToString();
                    newSpec._class = iDataGridView.Rows[rowIndex].Cells["Class"].Value.ToString();
                    newSpec.testItem = iDataGridView.Rows[rowIndex].Cells["TestItem"].Value.ToString();
                    newSpec.LowLimit = "";
                    newSpec.UpLimit = "";
                    newSpec.Unit = "";
                    newSpec.MeasureName = "";
                    newSpec.SpecNumber = "";
                    newSpec.SpecName = "";
                    organization.Parameters.Add(newSpec);
                }

                nw.Show();
                nw.TopMost = true;
                nw.BringToFront();
                this.TopMost = false;

                nw.loadPropertyGridDVG1(organization);

            }
            else
            {
                MessageBox.Show("Select a project first!");
            }
        }

        private string ReadParameterDescription(string fileName)
        {
            string fileContext = "";

            if (!File.Exists(fileName))
            {
                return "";
            }

            FileStream m_FS = new FileStream(fileName, FileMode.OpenOrCreate);
            StreamReader m_SR = new StreamReader(m_FS);
            fileContext = m_SR.ReadToEnd();
            m_SR.Close();
            m_FS.Close();
            return fileContext;
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

        //Save Test Plan
        private void XmlSaveDVGtoFile(string iFileName, DataGridView iDataGridView, string[] dvgColumnHeaders)
        {
            #region Write

            using (FileStream fileSteam = File.Create(iFileName))
            {
                XmlWriterSettings iXmlWriterSettings = new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 };

                using (XmlWriter iXmlWriter = XmlWriter.Create(fileSteam, iXmlWriterSettings))
                {
                    string iStringToWrite = "";
                    string[] iStringArray = null;
                    string[] iStringArray_Sub = null;
                    string iInitFileName = "";
                    string iString = "";
                    iXmlWriter.WriteStartDocument();
                    iXmlWriter.WriteStartElement("TestObjects");
                    for (int i = 0; i < iDataGridView.Rows.Count; i++)
                    {
                        iXmlWriter.WriteStartElement("TestObject" + i.ToString());
                        foreach (string iColumnHeaderText in dvgColumnHeaders)
                        {
                            if (iDataGridView.Rows[i].Cells[iColumnHeaderText].Value.ToString() == "")
                            {
                                switch (iColumnHeaderText)
                                {
                                    case "Select":
                                        iXmlWriter.WriteElementString(iColumnHeaderText, "False");
                                        break;
                                    default:
                                        iXmlWriter.WriteElementString(iColumnHeaderText, "Empty");
                                        break;
                                }
                            }
                            else
                            {
                                    switch (iColumnHeaderText)
                                    {
                                        case "ParaDescriptionFile":
                                            iStringToWrite = "";
                                            if (!iDataGridView.Rows[i].Cells["ParaDescriptionFile"].Value.ToString().Contains("-"))//判断是否存在文件名
                                            {
                                                iStringArray = iDataGridView.Rows[i].Cells["Para"].Value.ToString().Split(',');
                                                foreach (string iTempString in iStringArray)
                                                {
                                                    iStringArray_Sub = iTempString.Split('=');
                                                    if (iStringArray_Sub.Length > 0)
                                                    {
                                                        iInitFileName = iDataGridView.Rows[i].Cells["Driver"].Value.ToString() + "-" + iDataGridView.Rows[i].Cells["TestItem"].Value.ToString() + "-" + iStringArray_Sub[0];
                                                        iString = XmlCreateParaOrSpecDescriptionFile(iInitFileName, "Description");
                                                    }
                                                    iStringToWrite += iString + ",";
                                                }
                                            }
                                            else
                                            {
                                                iStringToWrite = iDataGridView.Rows[i].Cells["ParaDescriptionFile"].Value.ToString();
                                            }
                                            iXmlWriter.WriteElementString(iColumnHeaderText, iStringToWrite.TrimEnd(','));
                                            break;
                                        case "SpecFile":
                                            iStringToWrite = "";
                                            if (!iDataGridView.Rows[i].Cells["SpecFile"].Value.ToString().Contains("-"))//判断是否存在文件名
                                            {
                                                iStringArray = iDataGridView.Rows[i].Cells["Measure"].Value.ToString().Split(',');
                                                foreach (string iTempString in iStringArray)
                                                {
                                                    iInitFileName = iDataGridView.Rows[i].Cells["Driver"].Value.ToString() + "-" + iDataGridView.Rows[i].Cells["TestItem"].Value.ToString() + "-" + iTempString;
                                                    iString = XmlCreateParaOrSpecDescriptionFile(iInitFileName, "Spec");

                                                    iStringToWrite += iString + ",";
                                                }
                                            }
                                            else
                                            {
                                                iStringToWrite = iDataGridView.Rows[i].Cells["SpecFile"].Value.ToString();
                                            }
                                            iXmlWriter.WriteElementString(iColumnHeaderText, iStringToWrite.TrimEnd(','));
                                            break;
                                        default:
                                            iXmlWriter.WriteElementString(iColumnHeaderText, iDataGridView.Rows[i].Cells[iColumnHeaderText].Value.ToString());
                                            break;
                                    }
                               
                                    
                              
                            }
                        }
                        iXmlWriter.WriteEndElement();
                    }
                    iXmlWriter.WriteEndDocument();
                }
            }

            #endregion
        }

        public DataTable XmlReadFiletoDataTable(string iFileName, DataTable iDataTable, string[] dvgColumnHeaders)
        {
            int iCount=0;
            do
            {
                Thread.Sleep(300);
                iCount++;
                if (iCount >= 10)
                {
                    return iDataTable;
                }
            } while (!File.Exists(iFileName));
            //initalize DataTable
            iDataTable = CreatColumnHeadersforDatatable(iDataTable, dvgColumnHeaders);
            //check size at first
            if (new FileInfo(iFileName).Length == 0) return iDataTable;

            #region read

            using (FileStream fileSteam = File.OpenRead(iFileName))
            {
                XmlReaderSettings iXmlReaderSettings = new XmlReaderSettings(); ;
                iXmlReaderSettings.ConformanceLevel = ConformanceLevel.Document;

              
                List<string> iList = new List<string>();
                Dictionary<string, string> tDic = new Dictionary<string, string>();
               
                string iString = "";

                using (XmlReader iXmlReader = XmlReader.Create(fileSteam, iXmlReaderSettings))
                {
                    iXmlReader.MoveToContent();
                    while (iXmlReader.Read())
                    {
                        if (iXmlReader.Name.Contains("TestObject") && iXmlReader.NodeType == XmlNodeType.Element)
                        {
                            tDic.Clear();
                            iList.Clear();
                            do
                            {
                                 iString = XmlReadString(iXmlReader);
                                if(iXmlReader.Name.Contains("TestObject") && iXmlReader.NodeType == XmlNodeType.EndElement)
                                {
                                    break;
                                }
                                else
                                {
                                    tDic.Add(iXmlReader.Name, iString);
                                }
                            } while (!iXmlReader.Name.Contains("TestObject"));
                            foreach (string iColumnHeader in dvgColumnHeaders_Testplan)
                            {
                                iString = "";
                                foreach (var iDic in tDic)
                                {
                                    if (iDic.Key == iColumnHeader)
                                    {
                                        iString = iDic.Value;
                                        switch (iString)
                                        {
                                            case "Empty":
                                                iString = "";
                                                break;
                                            case "0":
                                                iString = "";
                                                break;
                                        }
                                    }
                                }
                                iList.Add(iString);
                            }
                            iDataTable.Rows.Add(iList.ToArray());
                        }
                    }
                }
            }


            #endregion

            return iDataTable;
        }

        public string XmlReadString(XmlReader iXmlReader)
        {
            string iString = "";
            while (iXmlReader.Read())
            {
                if (iXmlReader.Name != "")
                {
                    iString = iXmlReader.ReadString();
                    break;
                }
            }
            return iString;
        }

        //Save Config
        public void XmlSaveConfigFile(Config iConfig)
        {
            string iFileName = Application.StartupPath+"\\CommonTestSetting.xml";
            if (File.Exists(iFileName))
            {
                File.Delete(iFileName);
            }

            #region Write

            using (FileStream fileSteam = File.Create(iFileName))
            {
                XmlWriterSettings iXmlWriterSettings = new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 };

                using (XmlWriter iXmlWriter = XmlWriter.Create(fileSteam, iXmlWriterSettings))
                {
                    iXmlWriter.WriteStartDocument();
                    iXmlWriter.WriteStartElement("CommonTest");
                    iXmlWriter.WriteStartElement("Setting");

                    foreach(var iConfigElement in iConfig.GetType().GetProperties())
                    {
                        object value = iConfigElement.GetValue(iConfig, new object[] { });
                        if (value.ToString() != "")
                        {
                            iXmlWriter.WriteElementString(iConfigElement.Name, value.ToString());
                        }
                        else
                        {
                            iXmlWriter.WriteElementString(iConfigElement.Name, "Empty");
                        }
                    }
                    iXmlWriter.WriteEndElement();

                    iXmlWriter.WriteEndDocument();
                }
            }

            #endregion
        }

        //Read Config
        public void XmlReadConfigFile(Config iConfig)
        {
            string iFileName = Application.StartupPath + "\\CommonTestSetting.xml";

            #region read

            using (FileStream fileSteam = File.OpenRead(iFileName))
            {
                XmlReaderSettings iXmlReaderSettings = new XmlReaderSettings(); ;
                iXmlReaderSettings.ConformanceLevel = ConformanceLevel.Document;

                List<string> iList = new List<string>();
                Dictionary<string, string> tDic = new Dictionary<string, string>();
                string iString = "";

                using (XmlReader iXmlReader = XmlReader.Create(fileSteam, iXmlReaderSettings))
                {
                    iXmlReader.MoveToContent();
                    while (iXmlReader.Read())
                    {
                        if (iXmlReader.Name=="Setting" && iXmlReader.NodeType == XmlNodeType.Element)
                        {
                            #region read to dictionary

                            tDic.Clear();
                            iList.Clear();
                            do
                            {
                                iString = XmlReadString(iXmlReader);
                                if (iXmlReader.Name=="Setting" && iXmlReader.NodeType == XmlNodeType.EndElement)
                                {
                                    break;
                                }
                                else
                                {
                                    tDic.Add(iXmlReader.Name, iString);
                                }
                            } while (iXmlReader.Name!="Setting");

                            #endregion

                            #region 根据变量名称对应赋值

                            foreach (var iConfigElement in iConfig.GetType().GetProperties())
                            {
                                iString = "";
                                foreach (var iDic in tDic)
                                {
                                    if (iDic.Key == iConfigElement.Name)
                                    {
                                        iString = iDic.Value;
                                     }
                                 }
                                switch (iString)
                                {
                                    case "True":
                                    case "true":
                                        iConfigElement.SetValue(iConfig, true, new object[] { });
                                        break;
                                    case "False":
                                    case "false":
                                        iConfigElement.SetValue(iConfig, false, new object[] { });
                                        break;
                                    case "":
                                        break;
                                    default:
                                        iConfigElement.SetValue(iConfig, iString, new object[] { });
                                        break;
                                }
                            }

                            #endregion

                        }
                    }
                }
            }


            #endregion
        }

        public string XmlCreateParaOrSpecDescriptionFile(string ItemInfor,string iFolder)
        {
            string Para_Spec_DescriptionFileName = ItemInfor;
            string iFilePath="";
            string iFileName = "";
            string iFileFormat = "";
            FileStream iFileStream = null;
            switch (iFolder)
            { 
                case "Description":
                    iFilePath = DescriptionPath;
                    iFileFormat = ".html";
                    break;
                case "Spec":
                    iFilePath = SpecPath;
                    iFileFormat = ".xml";
                    break;
                default:
                    break;
            }
            iFileName = iFilePath + Para_Spec_DescriptionFileName + iFileFormat;
            int i = 0;
            do
            {
                if (i != 0)
                {
                    Para_Spec_DescriptionFileName = ItemInfor + "("+i.ToString()+")";
                    iFileName = iFilePath + Para_Spec_DescriptionFileName + iFileFormat;
                }
                i++;

            } while (File.Exists(iFileName));

            iFileStream=File.Create(iFileName);
            iFileStream.Close();


            return Para_Spec_DescriptionFileName;
        }

        public string GetExactParaOrSpecName(string keyString,string ElementFilePath)
        {
            string tString = "";
            if(!ElementFilePath.Contains(keyString))
            {
                return "";
            }
            string[] StringArray = ElementFilePath.Replace(keyString, "").Split('(');
            if (StringArray.Length >0)
            {
                tString = StringArray[0];
            }
            else
            {
                tString = "";
            }
            return tString;
        }

        //disable columns sort
        private void dataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        #endregion

      

     


  

     

       



       

       

       

      

     

   

     }

}