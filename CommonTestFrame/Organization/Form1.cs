using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Xml;
using System.Security.Cryptography;
using Organization;
using System.Reflection;
using BaseDriver;



namespace CommonTestFrame
{  
    public partial class Form1 : Form
    {
        #region 变量声明

        //int FailedIndex = -1;

        FailInformation iFailInfor = new FailInformation();

        Thread producer;
        protected string RunStatus = "";
       
        long startTimeStick;
        string startTime;


        StatusInformation iStatusInfo = new StatusInformation();

      
        public delegate void myDelegate();
        private delegate void iDelegate(TestObjects iTestObjects);
        private delegate void iDelegate_Sub(TestObject iTestObject);

        //progress bar
        protected delegate void deleProgressSet(int i);

        //SN
        bool IsControlSN = false;
        string SNstr = "";
        string ResultName = "";

        //load test plan
        private bool testPlanLoaded = false;
        string SelectedTestPlan = "";
  

        #region 窗体变量

        //Main Window
        public static Form1 myForm1 = null;

        //Register Window
        RegisterWindow mRW = new RegisterWindow();
        //login Window
        LoginWindow mLW = new LoginWindow();
        //User Center Window
        UserCenterWindows mUCW = new UserCenterWindows();
        //Config Window
        ConfigWindow iConfigWindow = new ConfigWindow();

        //Instrumens Window
        InstrumentsSetup iInstrumentsSetup = new InstrumentsSetup();

        #endregion


        //testObjects
        TestObjects TestObjects = new TestObjects();

        //config file
        public Config iConfig = new Config();

        //Users
        public Users iUsers = new Users();
        public User ActiveUser = new User();
        public List<string> iUserNameList = new List<string>();

        //dll path
        public string DllPath = "";

        //Instruments
        public Instruments iInstruments = new Instruments();

        //Universal Variant
        public UniversalVariant iUniversalVariant = new UniversalVariant();

        //Customer Variant
        public CustomerVarients iCVs = new CustomerVarients();

       

        //Encrypt
         #region key
        private string key1 = @"
28:1	  	In the end of the sabbath, as it began to dawn toward the first day of the week, came Mary Magdalene and the other Mary to see the sepulchre.
28:2	  	And, behold, there was a great earthquake: for the angel of the Lord descended from heaven, and came and rolled back the stone from the door, and sat upon it.
28:3	  	His countenance was like lightning, and his raiment white as snow:
28:4	  	And for fear of him the keepers did shake, and became as dead men.
28:5	  	And the angel answered and said unto the women, Fear not ye: for I know that ye seek Jesus, which was crucified.
28:6	  	He is not here: for he is risen, as he said. Come, see the place where the Lord lay.
28:7	  	And go quickly, and tell his disciples that he is risen from the dead; and, behold, he goeth before you into Galilee; there shall ye see him: lo, I have told you.
28:8	  	And they departed quickly from the sepulchre with fear and great joy; and did run to bring his disciples word.
28:9	  	And as they went to tell his disciples, behold, Jesus met them, saying, All hail. And they came and held him by the feet, and worshipped him.
28:10	  	Then said Jesus unto them, Be not afraid: go tell my brethren that they go into Galilee, and there shall they see me.
28:11	  	Now when they were going, behold, some of the watch came into the city, and shewed unto the chief priests all the things that were done.
28:12	  	And when they were assembled with the elders, and had taken counsel, they gave large money unto the soldiers,
28:13	  	Saying, Say ye, His disciples came by night, and stole him away while we slept.
28:14	  	And if this come to the governor's ears, we will persuade him, and secure you.
28:15	  	So they took the money, and did as they were taught: and this saying is commonly reported among the Jews until this day.
28:16	  	Then the eleven disciples went away into Galilee, into a mountain where Jesus had appointed them.
28:17	  	And when they saw him, they worshipped him: but some doubted.
28:18	  	And Jesus came and spake unto them, saying, All power is given unto me in heaven and in earth.
28:19	  	Go ye therefore, and teach all nations, baptizing them in the name of the Father, and of the Son, and of the Holy Ghost:
28:20	  	Teaching them to observe all things whatsoever I have commanded you: and, lo, I am with you alway, even unto the end of the world. Amen.";

        #endregion

        //DataGridView's column headers
        public string[] dvgColumnHeaders = new string[] { "Index", "Items", "Result", "Unit", "Low Limit", "High Limit", "Judge", "Test Time", "Description" };

        //datagirdviewrow 的backcolor
        const int ReadyColor = 0;
        const int ResultColor = 1;
        List<Color> FunctionRowColorList = new List<Color> { Color.LightBlue, Color.LightGreen };
        List<Color> TestItemRowColorList = new List<Color> { Color.LightSkyBlue, Color.LightSeaGreen };

        //Instrument Config window
        private bool IsInstrConfigFileExits = false;


        #endregion

        #region 系统初始化

        public Form1()
        {

            InitializeComponent();
            try
            {
                myForm1 = this;
                CreatColumnHeaders(dvgColumnHeaders);
                //get dll path
                DllPath = Application.StartupPath + "\\lib\\";
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                iConfigWindow.XmlReadConfigFile(iConfig);

                #region test control button

                if (iConfig.IsStopWhenFailed == true)
                {
                    testFlowModeToolStripMenuItem.Checked = true;
                }
                else
                {
                    testFlowModeToolStripMenuItem.Checked = false;
                }
                if (iConfig.IsSaveToTxt == true)
                {
                    toolStripMenuItem5.Checked = true;
                    iConfig.IsSaveToTxt = true;
                }
                else
                {
                    toolStripMenuItem5.Checked = false;
                }
                if (iConfig.IsSaveToCSV == true)
                {
                    toolStripMenuItem6.Checked = true;
                }
                else
                {
                    toolStripMenuItem6.Checked = false;
                }
                #region//SN text box

                if (iConfig.IsAllowManualInput_SN == true)
                {
                    iConfig.IsAllowManualInput_SN = true;
                    toolStripMenuItem_Forbid.Text = "Forbid Manual Input";
                }
                else
                {
                    iConfig.IsAllowManualInput_SN = false;
                    toolStripMenuItem_Forbid.Text = "Allow Manual Input";
                }
                switch (iConfig.CharacterCaseType_SN)
                {
                    case "Normal":
                        toolStripTextBox1.CharacterCasing = CharacterCasing.Normal;
                        toolStripMenuItem_UpperCase.Checked = false;
                        toolStripMenuItem_LowerCase.Checked = false;
                        toolStripMenuItem_NormalCase.Checked = true;
                        break;
                    case "Upper":
                        toolStripTextBox1.CharacterCasing = CharacterCasing.Upper;
                        toolStripMenuItem_UpperCase.Checked = true;
                        toolStripMenuItem_LowerCase.Checked = false;
                        toolStripMenuItem_NormalCase.Checked = false;
                        break;
                    case "Lower":
                        toolStripTextBox1.CharacterCasing = CharacterCasing.Lower;
                        toolStripMenuItem_UpperCase.Checked = false;
                        toolStripMenuItem_LowerCase.Checked = true;
                        toolStripMenuItem_NormalCase.Checked = false;
                        break;
                }

                #endregion

                #endregion

                #region Window view setting
                if (iConfig.IsShowFunctionRow)
                {
                    displayFuntionRowsToolStripMenuItem.Checked = true;
                }
                else
                {
                    displayFuntionRowsToolStripMenuItem.Checked = false;
                }
                if (iConfig.IsShowTestItemRow)
                {
                    displayTestItemRowsToolStripMenuItem.Checked = true;
                }
                else
                {
                    displayTestItemRowsToolStripMenuItem.Checked = false;
                }
                if (iConfig.IsShowInfoBorad)
                {
                    displayInformationBoardToolStripMenuItem.Checked = true;
                }
                else
                {
                    displayInformationBoardToolStripMenuItem.Checked = false;
                }
                if (iConfig.IsShowStatusBar)
                {
                    displayStatusBarToolStripMenuItem.Checked = true;
                }
                else
                {
                    displayStatusBarToolStripMenuItem.Checked = false;
                }

                if (iConfig.IsLoopTest)
                {
                    loopTestToolStripMenuItem.Checked = true;
                }
                else
                {
                    loopTestToolStripMenuItem.Checked = false;
                }

                #endregion

                //read user data
                iUserNameList.Clear();
                iUsers = XmlReadUsersRecord();
                if (iUsers != null)
                {
                    foreach (User iUser in iUsers)
                    {
                        iUserNameList.Add(iUser.UserName);
                    }
                    mLW.LoadUserRecord(iUserNameList);
                }
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }

          
        }

        #endregion

        #region 测试

        #region 加载测试计划

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                testPlanLoaded = false;
                iConfigWindow = new ConfigWindow();
                OpenFileDialog iOpenFolder = new OpenFileDialog();
                iOpenFolder.Filter = "TPS files (*.tps)|*.tps";
                iOpenFolder.InitialDirectory = @iConfig.RootPath;
                iOpenFolder.Title = "Select test plan folder";
                if (iOpenFolder.ShowDialog() == DialogResult.OK)
                {
                    iConfigWindow.RootPath = Path.GetDirectoryName(iOpenFolder.FileName);
                    iConfigWindow.SelectedPlan = Path.GetFileNameWithoutExtension(iOpenFolder.FileName);
                    iConfig.RootPath = iConfigWindow.RootPath;
                    iConfigWindow.UpdateFilePath(iConfigWindow.RootPath);

                    TestToolStripButton.Enabled = true;
                    LoadToolStripButton.Enabled = true;
                    RefreshTtoolStripButton.Enabled = true;
                    InforDes.Text = "";



                    SelectedTestPlan = iConfigWindow.SelectedPlan;

                    toolStripLabel1.Text = "                    Current Test Plan:  " + iConfigWindow.SelectedPlan; ;

                    FontAdaptText(label_TestPlan, SelectedTestPlan);
          
                    
                    //read Instruments config
                    XmlReadInstrumentsConfigFile();
                    ReadTestPlan(SelectedTestPlan);
                    toolStripTextBox1.Focus();
                    toolStripTextBox1.Text = "";
                    testPlanLoaded = true;
                    RefreshTtoolStripButton.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

      

        private void ReadTestPlan(String SelectedTable)
        {
            #region//初始化TestObjects

            iConfigWindow.CommonTable = iConfigWindow.XmlReadFiletoDataTable(iConfigWindow.TestPlanPath + iConfigWindow.SelectedPlan + ".tps", iConfigWindow.CommonTable, iConfigWindow.dvgColumnHeaders_Testplan);

            int InitItemCount = iConfigWindow.CommonTable.Rows.Count;

            TestObjects.Clear();

            //int RowCount = 0;
            for (int i = 0; i < InitItemCount; i++)
            {
                TestObject newTestObject = new TestObject();

                if (iConfigWindow.CommonTable.Rows[i]["Select"].ToString().ToUpper()=="TRUE") newTestObject.IsSelect = true;else newTestObject.IsSelect = false;
           
                if (iConfigWindow.CommonTable.Rows[i]["Loop"].ToString() == "")newTestObject.Loop = 0;else newTestObject.Loop = Convert.ToInt16(iConfigWindow.CommonTable.Rows[i]["Loop"].ToString());
           
                newTestObject.Driver = iConfigWindow.CommonTable.Rows[i]["Driver"].ToString();
                newTestObject.Class = iConfigWindow.CommonTable.Rows[i]["Class"].ToString();
                newTestObject.FunctionName = iConfigWindow.CommonTable.Rows[i]["TestItem"].ToString();
                newTestObject.DisplayText = iConfigWindow.CommonTable.Rows[i]["DisplayText"].ToString();
                newTestObject.ExecuteType = iConfigWindow.CommonTable.Rows[i]["ExecuteType"].ToString();
                newTestObject.ReturnType = iConfigWindow.CommonTable.Rows[i]["ReturnType"].ToString();
                if (iConfigWindow.CommonTable.Rows[i]["IsJudge"].ToString().ToUpper() == "TRUE") newTestObject.IsJudge = true; else newTestObject.IsJudge = false;

                if (newTestObject.IsSelect == true && newTestObject.Driver != "" && newTestObject.Class != "" && newTestObject.FunctionName != "" && newTestObject.Loop > 0)
                {
                    //get parameters
                    newTestObject.ParaCollection = iConfigWindow.CommonTable.Rows[i]["Para"].ToString();
                    if (newTestObject.ParaCollection.ToUpper().Contains("S_INSTR_"))
                    {
                          newTestObject.ParaCollection = GetInstrment(newTestObject);
                    }
                    if (iConfigWindow.CommonTable.Rows[i]["IsShowResult"].ToString().ToUpper() == "TRUE") newTestObject.IsShowResult = true; else newTestObject.IsShowResult = false;

                    //get MeasureItems
                    string[] MeasureNames = iConfigWindow.CommonTable.Rows[i]["Measure"].ToString().Split(',');
                    string[] SpecFileNames = iConfigWindow.CommonTable.Rows[i]["SpecFile"].ToString().Split(',');

                    newTestObject.SpecCollection = GetMeasureItems(newTestObject, MeasureNames, SpecFileNames);

                    if (newTestObject.SpecCollection == null)
                    {
                        newTestObject.IsShowResult = false;
                    }
                    TestObjects.Add(newTestObject);
                }

            }

            #endregion

            this.Invoke(new iDelegate(TestObjects_Display_dvg), TestObjects);

            return;
        }

        //get MeasureItems
        private SpecCollection GetMeasureItems(TestObject newTestObject, string[] MeasureNames, string[] SpecFileNames)
        {
            SpecCollection newSpecCollection = new SpecCollection();
            string iFileName = "";

            if (MeasureNames.Length > 0)
            {
                for (int i = 0; i < MeasureNames.Length; i++)
                {
                    if (MeasureNames[i] == "") continue;
                    Spec newSpec = new Spec();
                    //newSpec.Index = newTestObject.Index+i+1;
                    newSpec.driver = newTestObject.Driver;
                    newSpec._class = newTestObject.Class;
                    newSpec.testItem = newTestObject.FunctionName;
                    newSpec.MeasureName = MeasureNames[i];
                    string keyString = newSpec.Driver + "-" + newSpec.Class + "-" + newSpec.TestItem + "-";
                    foreach (string iSpecFileName in SpecFileNames)
                    {
                        if (iConfigWindow.GetExactParaOrSpecName(keyString,iSpecFileName) == newSpec.MeasureName)
                        {
                            iFileName = iConfigWindow.SpecPath + iSpecFileName + ".xml";
                            break;
                        }
                        else
                        {
                            iFileName = "";
                        }
                    }
                    if (!File.Exists(iFileName) || iFileName=="")
                    {
                        newSpec.LowLimit = "-";
                        newSpec.UpLimit = "-";
                        newSpec.Unit = "-";
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
                                        newSpec.LowLimit = iConfigWindow.XmlReadString(iXmlReader);
                                        newSpec.UpLimit = iConfigWindow.XmlReadString(iXmlReader);
                                        newSpec.Unit = iConfigWindow.XmlReadString(iXmlReader);
                                        newSpec.SpecNumber = iConfigWindow.XmlReadString(iXmlReader);
                                        newSpec.SpecName = iConfigWindow.XmlReadString(iXmlReader);
                                    }
                                }
                            }
                        }


                        #endregion
                    }
                    newSpecCollection.Add(newSpec);

                }

            }
            else
            {
                return null;
            }
            if (newSpecCollection.Count <= 0)
            {
                return null;
            }

            return newSpecCollection;
        }

        private void RefreshTtoolStripButton_Click(object sender, EventArgs e)
        {
            if(testPlanLoaded==false) return;
            try
            {
                //read Instruments config
                XmlReadInstrumentsConfigFile();
                ReadTestPlan(SelectedTestPlan);
                toolStripTextBox1.Focus();
                toolStripTextBox1.Text = "";
                testPlanLoaded = true;
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        #endregion

        #region 执行测试
   
        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (testPlanLoaded != true)
                {
                    MessageBox.Show("no test plan loaded!");
                    return;
                }

                SNstr = getSN();

                if (iConfig.IsLoopTest == false)
                {
                    if (SNstr == "")
                    {
                        toolStripTextBox1.Focus();
                        toolStripTextBox1.Text = "";
                        return;
                    }
                }
                iUniversalVariant.SN = SNstr;
                iStatusInfo.imageIndex = 2;
                iStatusInfo.Title = "Testing...";
                iStatusInfo.Detail = "";
                this.Invoke(new myDelegate(StatusDisplay));

                toolStripTextBox1.Enabled = false;//禁用SN窗口
                startTime = DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToLongTimeString();
                TestToolStripButton.Enabled = false;
                LoadToolStripButton.Enabled = false;
                RefreshTtoolStripButton.Enabled = false;
                PauseToolStripButton.Enabled = true;
                StopToolStripButton.Enabled = true;

                iFailInfor.Fail_Index = -1;//Initialize failed index 
                iFailInfor.Fail_FunctionName = "";
                iFailInfor.Fail_Message = "";
                startTimeStick = DateTime.Now.Ticks;
                producer = new Thread(new ThreadStart(StartTest));
                producer.SetApartmentState(ApartmentState.STA);
                producer.Start();
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }

        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                switch (PauseToolStripButton.Text.ToUpper())
                {
                    case "PAUSE":
                        RunStatus = "Pause";
                        PauseToolStripButton.Text = "Resume";
                        PauseToolStripButton.ToolTipText = "Resume";
                        break;
                    case "RESUME":
                        RunStatus = "Resume";
                        PauseToolStripButton.Text = "Pause";
                        PauseToolStripButton.ToolTipText = "Pause";
                        break;
                    default:
                        RunStatus = "Pause";
                        PauseToolStripButton.Text = "Resume";
                        PauseToolStripButton.ToolTipText = "Pause";
                        break;
                }
                toolStrip1.Refresh();
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
             RunStatus = "Stop";
        }

        void StartTest()
        {
            int Errstatus = -1;

            this.Invoke(new myDelegate(MeasDisplayClear_DVG));

            if (iConfig.IsLoopTest == false)
            {
                Errstatus = ExecuteTestPlan();

                if (iConfig.IsSaveToTxt == true)
                {
                    saveTXTResult(SNstr, iFailInfor.Fail_Index);
                }
                if (iConfig.IsSaveToCSV == true)
                {
                    saveCSVResult(SNstr, iFailInfor.Fail_Index);
                }
                saveSummaryInfomation(SNstr, iFailInfor.Fail_Index);
                if (Errstatus >= 0)
                {
                    if (iFailInfor.Fail_Index < 0)
                    {
                        this.Invoke(new myDelegate(ResultJudgeDisplay_Pass));
                    }
                    else
                    {
                        this.Invoke(new myDelegate(ResultJudgeDisplay_Fail));
                    }
                }
                else
                {
                    this.Invoke(new myDelegate(ResultJudgeDisplay_Fail));
                }
            }
            else
            { 
                do
                {
                    Errstatus = -1;

                    this.Invoke(new myDelegate(MeasDisplayClear_DVG));

                    Errstatus = ExecuteTestPlan();

                    if (iConfig.IsSaveToTxt == true)
                    {
                        saveTXTResult(SNstr, iFailInfor.Fail_Index);
                    }
                    if (iConfig.IsSaveToCSV == true)
                    {
                        saveCSVResult(SNstr, iFailInfor.Fail_Index);
                    }
                    saveSummaryInfomation(SNstr, iFailInfor.Fail_Index);
                    if (Errstatus >= 0)
                    {
                        if (iFailInfor.Fail_Index < 0)
                        {
                            this.Invoke(new myDelegate(ResultJudgeDisplay_Pass));
                        }
                        else
                        {
                            this.Invoke(new myDelegate(ResultJudgeDisplay_Fail));
                        }
                    }
                    else
                    {
                        this.Invoke(new myDelegate(ResultJudgeDisplay_Fail));
                    }
                    #region check Run status
                    Application.DoEvents();
                    switch (RunStatus.ToUpper())
                    {
                        case "PAUSE":
                            iStatusInfo.imageIndex = 1;
                            iStatusInfo.Title = "User Pause";
                            iStatusInfo.Detail = "User Pause";
                            this.Invoke(new myDelegate(StatusDisplay));
                            do
                            {
                                Application.DoEvents();
                                //Thread.Sleep(50);
                            } while (RunStatus.ToUpper() == "PAUSE");

                            switch (RunStatus.ToUpper())
                            {
                                case "RESUME":
                                    break;
                                case "STOP":
                                    RunStatus = "";
                                    iStatusInfo.imageIndex = 1;
                                    iStatusInfo.Title = "User Stop";
                                    iStatusInfo.Detail = "User Stop";
                                    this.Invoke(new myDelegate(StatusDisplay));
                                    iConfig.IsLoopTest = false;
                                    this.Invoke(new myDelegate(ResumeWindow));
                                    iConfig.IsLoopTest = true;
                                    return;
                            }
                            break;
                        case "STOP":
                            RunStatus = "";
                            iStatusInfo.imageIndex = 1;
                            iStatusInfo.Title = "User Stop";
                            iStatusInfo.Detail = "User Stop";
                            this.Invoke(new myDelegate(StatusDisplay));
                            iConfig.IsLoopTest = false;
                            this.Invoke(new myDelegate(ResumeWindow));
                            iConfig.IsLoopTest = true;
                            return;
                    }

                    RunStatus = "";

                    #endregion
                }while(1==1);
            }
        }

        public int ExecuteTestPlan()
        {
            int funStatus;
            foreach (TestObject iTestObject in TestObjects)
            {
                if (iTestObject.ExecuteType.ToUpper() == "ONCE" && iTestObject.IsHasBeenExecuted==true) continue;
                if (iFailInfor.Fail_Index >= 0 && iConfig.IsStopWhenFailed == true && iTestObject.ExecuteType.ToUpper() != "EVERYTIME") continue;
                int loopCount=iTestObject.Loop;
                do
                {
                    funStatus = functionExecute(iTestObject);

                    //iTestObject.FunExecuteStatus = funStatus;
                    
                    this.Invoke(new iDelegate_Sub(MeasureDisplay_DVG), iTestObject);
                    if (funStatus >= 0 && iTestObject.FunExecuteStatus>=0)
                    {
                        iTestObject.IsHasBeenExecuted = true;
                    }
                    if (iConfig.IsStopWhenFailed == true)
                    {
                        if (funStatus < 0 || iTestObject.FailIndex >= 0 || iTestObject.FunExecuteStatus<0)
                        {
                            if (iTestObject.ExecuteType.ToUpper() == "NORMAL")
                            {
                                break;
                            }
                        }
                    }
                    loopCount--;
                    #region check Run status

                    switch (RunStatus.ToUpper())
                    {
                        case "PAUSE":
                            iStatusInfo.imageIndex = 1;
                            iStatusInfo.Title = "User Pause";
                            iStatusInfo.Detail = "User Pause";
                            this.Invoke(new myDelegate(StatusDisplay));
                            do
                            {
                                Application.DoEvents();
                                Thread.Sleep(50);
                            } while (RunStatus.ToUpper() == "PAUSE");

                            switch (RunStatus.ToUpper())
                            {
                                case "RESUME":
                                    break;
                                case "STOP":
                                    //RunStatus = "";
                                    iStatusInfo.imageIndex = 1;
                                    iStatusInfo.Title = "User Stop";
                                    iStatusInfo.Detail = "User Stop";
                                    this.Invoke(new myDelegate(StatusDisplay));
                                    return -1;
                            }
                            break;
                        case "STOP":
                           // RunStatus = "";
                            iStatusInfo.imageIndex = 1;
                            iStatusInfo.Title = "User Stop";
                             iStatusInfo.Detail = "User Stop";
                             this.Invoke(new myDelegate(StatusDisplay));
                            return -1;
                     }

                    RunStatus = "";

                    #endregion

                } while (loopCount > 0);

            }

            return 0;
        }

        int functionExecute(TestObject iTestObject)
        {
            string dllFileName = DllPath + iTestObject.Driver + ".dll"; 

            Assembly assembly = Assembly.LoadFrom(dllFileName);
            foreach (Type type in assembly.GetTypes())
            {
                if (type.Name == iTestObject.Class)
                {
                    Dictionary<string, string> tDic = new Dictionary<string, string>();
                    if (iTestObject.Driver != "BaseDriver" || iTestObject.Class != "General" || iTestObject.FunctionName != "Declare")
                    {
                        string[] tStringArray = iTestObject.ParaCollection.Split(',');
                        string[] tStringArray2 = null;
                       // Dictionary<string, string> tDic = new Dictionary<string, string>();
                        if (tStringArray.Length > 0)
                        {
                            foreach (string iString in tStringArray)
                            {
                                tStringArray2 = iString.Split('=');
                                if (tStringArray2.Length == 2)
                                {
                                    if (tStringArray2[1].Contains("[U]") && tStringArray2[1].Contains("[/U]"))
                                    {
                                        string[] tString = Regex.Match(tStringArray2[1], @"\[U](.+?)\[/U]").Groups[1].Value.Split(';');
                                        foreach (var iUV in iUniversalVariant.GetType().GetProperties())
                                        {
                                            object value = iUV.GetValue(iUniversalVariant, new object[] { });
                                            if (tString[0] == iUV.Name)
                                            {
                                                tDic[tStringArray2[0]] = value.ToString();
                                            }
                                        }
                                    }
                                    else
                                    {

                                        if (tStringArray2[0].ToUpper().Contains("S_INSTR_"))
                                        {
                                            tDic[tStringArray2[0].Replace("S_Instr_", "")] = tStringArray2[1];
                                        }
                                        else
                                        {
                                            tDic[tStringArray2[0]] = tStringArray2[1];
                                        }
                                    }
                                }
                            }
                        }
                    }
                    MethodInfo method = type.GetMethod(iTestObject.FunctionName, (BindingFlags)int.MaxValue);
                    if (method == null)
                    { 
                         //error dispaly
                        iStatusInfo.imageIndex = 1;
                        iStatusInfo.Title = "Error";
                        iStatusInfo.Detail = "Can not find \"" + iTestObject.FunctionName + "\" execute function";
                        iFailInfor.Fail_Index = iTestObject.Index;
                        iFailInfor.Fail_FunctionName = iTestObject.FunctionName;
                        iFailInfor.Fail_Message = iStatusInfo.Detail;
                        this.Invoke(new myDelegate(StatusDisplay));
                       
                        return -1;
                    }

                    //function dispaly
                    iStatusInfo.imageIndex = 2;
                    iStatusInfo.Title = "Testing...";
                    iStatusInfo.Detail = iTestObject.FunctionName + "(" + iTestObject.ParaCollection + ")";
                    this.Invoke(new myDelegate(StatusDisplay));
                    DateTime tickStart = DateTime.Now;

                    try
                    {
                        var arguments = new List<object>();
                        
                        if (iTestObject.Driver == "BaseDriver" && iTestObject.Class == "General" && iTestObject.FunctionName == "Declare")
                        {
                            arguments.Add(iTestObject.ParaCollection.Replace("=",":"));
                        }
                        else
                        {
                            string tString = "";
                            foreach (var iParameterInfo in method.GetParameters())
                            {
                                foreach (var iDic in tDic)
                                {
                                    tString = iParameterInfo.DefaultValue.ToString();
                                    if (iDic.Key == iParameterInfo.Name)
                                    {
                                        tString = iDic.Value;
                                        break;
                                    }

                                }
                                arguments.Add(Convert.ChangeType(tString, iParameterInfo.ParameterType));
                            }
                        }

                        //create instance
                        if (method.IsStatic == false && iTestObject.Instance == null)
                        {
                            //instance = Activator.CreateInstance(method.DeclaringType);
                            foreach (TestObject eTestObject in TestObjects)
                            {
                                if (eTestObject.Driver == iTestObject.Driver && eTestObject.Class == iTestObject.Class && eTestObject.Instance != null)
                                {
                                    iTestObject.Instance = eTestObject.Instance;
                                    break;
                                }

                            }
                            if (iTestObject.Instance == null)
                            {
                                iTestObject.Instance = Activator.CreateInstance(method.DeclaringType);
                            }

                        }


                        object ReturnObject = method.Invoke(iTestObject.Instance, arguments.ToArray());
                        DateTime tickEnd = DateTime.Now;
                        TimeSpan iTimeSpan = tickEnd - tickStart;
                        iTestObject.TestTime = iTimeSpan.Milliseconds.ToString();

                        

                        IEnumerable enumerable = ReturnObject as IEnumerable;
                       
                        if (enumerable != null)
                        {
                            if (ReturnObject.GetType().ToString().Contains("CustomerVarients"))
                            {
                                object value=null;
                                foreach (object element in enumerable)
                                {
                                    CustomerVarient iCV = new CustomerVarient();
                                    foreach (var iElement in element.GetType().GetProperties())
                                    {
                                       value = iElement.GetValue(element, new object[] { });
                                        
                                        switch (iElement.Name)
                                        { 
                                            case "Name":
                                                iCV.Name = value.ToString();
                                                break;
                                            case "Value":
                                                iCV.Value = value;
                                                break;
                                        }
                                    }
                                    iCVs.Add(iCV);
                                }
                            }
                            if (iTestObject.IsJudge == false || iTestObject.SpecCollection == null) return 0;

                            List<object> ReturnObjects = new List<object>();
                            foreach (object element in enumerable)
                            {
                                ReturnObjects.Add(element);
                            }
                            return JudgeResult_array(iTestObject, ReturnObjects.ToArray());
                        }
                        else
                        {
                            if (iTestObject.IsJudge == false || iTestObject.SpecCollection == null) return 0;
                            return JudgeResult_single(iTestObject, ReturnObject);
                        }
                    }
                    catch (Exception ex)
                    {
                        iStatusInfo.imageIndex = 1;
                        iStatusInfo.Title = "Error";
                        iFailInfor.Fail_Index = iTestObject.Index;
                        iTestObject.FunExecuteStatus = -1;
                        if (ex.InnerException != null)
                        {
                            iStatusInfo.Detail = ex.InnerException.ToString();
                            iTestObject.Description = ex.InnerException.ToString();
                        }
                        else
                        {
                            iStatusInfo.Detail = ex.Message.ToString();
                            iTestObject.Description = ex.Message.ToString();
                        }
                        iFailInfor.Fail_FunctionName = iTestObject.FunctionName;
                        iFailInfor.Fail_Message = iTestObject.Description;
                        this.Invoke(new myDelegate(StatusDisplay));
                      
                        return -1;
                    }
                }
            }
            //error display
            iStatusInfo.imageIndex = 1;
            iStatusInfo.Title = "Error";
            iStatusInfo.Detail = "Can not find \"" + iTestObject.Class + "\"";
            iFailInfor.Fail_Index = iTestObject.Index;
            iFailInfor.Fail_FunctionName = iTestObject.FunctionName;
            iFailInfor.Fail_Message = iStatusInfo.Detail;
            this.Invoke(new myDelegate(StatusDisplay));
           
            return -1; 
           
        }

        private int JudgeResult_array(TestObject iTestObject, object[] ReturnObjects)
        {
            int i = 0;
            bool IsLowPass = false;
            bool IsUpPass = false;
    
            foreach (Spec iSpec in iTestObject.SpecCollection)
            {
                iSpec.MeasureValue = ReturnObjects[i].ToString();
                iSpec.TestTime = iTestObject.TestTime;
                IComparable messureValue = ChangeType(iSpec.MeasureValue, ReturnObjects[i].GetType());
                IComparable LowLimit = ChangeType(iSpec.LowLimit, ReturnObjects[i].GetType());
                IComparable HighLimit = ChangeType(iSpec.UpLimit, ReturnObjects[i].GetType());
                if (LowLimit != null)
                {
                    if (messureValue.CompareTo(LowLimit) >= 0) IsLowPass = true; else IsLowPass = false;
                }
                else
                {
                    IsLowPass = true;
                }
                if (HighLimit != null)
                {
                    if (messureValue.CompareTo(HighLimit) <= 0) IsUpPass = true; else IsUpPass = false;
                }
                else
                {
                    IsUpPass = true;
                }

                if (IsLowPass && IsUpPass)
                {
                    iSpec.MeasureResult = "PASS";
                }
                else
                {
                    iSpec.MeasureResult = "FAIL";
                    if (iTestObject.IsShowResult == false)
                    {
                        iTestObject.FailIndex = iFailInfor.Fail_Index = iTestObject.Index;
                    }
                    else
                    {
                        iTestObject.FailIndex = iFailInfor.Fail_Index = iSpec.Index;
                    }
                    iTestObject.FunExecuteStatus = -1;
                    //error dispaly
                    iStatusInfo.imageIndex = 1;
                    iStatusInfo.Title = "FAIL";
                    iStatusInfo.Detail = iSpec.MeasureName + " :The test result is out of Limit!";
                    iFailInfor.Fail_FunctionName = iTestObject.FunctionName;
                    iFailInfor.Fail_Message = iStatusInfo.Detail;
                    this.Invoke(new myDelegate(StatusDisplay));
                }
                i++;
            }

            return 0;
        }

        private int JudgeResult_single(TestObject iTestObject, object ReturnObject)
        {
            bool IsLowPass = false;
            bool IsUpPass = false;
        
            foreach (Spec iSpec in iTestObject.SpecCollection)
            {
                iSpec.MeasureValue = ReturnObject.ToString();
                iSpec.TestTime = iTestObject.TestTime;
                IComparable messureValue = ChangeType(iSpec.MeasureValue, ReturnObject.GetType());
                IComparable LowLimit = ChangeType(iSpec.LowLimit, ReturnObject.GetType());
                IComparable HighLimit = ChangeType(iSpec.UpLimit, ReturnObject.GetType());
                if (LowLimit != null)
                {
                    if (messureValue.CompareTo(LowLimit) >= 0) IsLowPass = true; else IsLowPass = false;
                }
                else
                {
                    IsLowPass = true;
                }
                if (HighLimit != null)
                {
                    if (messureValue.CompareTo(HighLimit) <= 0) IsUpPass = true; else IsUpPass = false;
                }
                else
                {
                    IsUpPass = true;
                }

                if (IsLowPass && IsUpPass)
                {
                    iSpec.MeasureResult = "PASS";
                }
                else
                {
                    iSpec.MeasureResult = "FAIL";

                    if (iTestObject.IsShowResult == false)
                    {
                        iTestObject.FailIndex = iFailInfor.Fail_Index = iTestObject.Index;
                    }
                    else
                    {
                        iTestObject.FailIndex = iFailInfor.Fail_Index = iSpec.Index;
                    }
                    iTestObject.FunExecuteStatus = -1;
                    //error dispaly
                    iStatusInfo.imageIndex = 1;
                    iStatusInfo.Title = "FAIL";
                    iStatusInfo.Detail = iSpec.MeasureName + " :The test result is out of Limit!";
                    iFailInfor.Fail_FunctionName = iTestObject.FunctionName;
                    iFailInfor.Fail_Message = iStatusInfo.Detail;
                    this.Invoke(new myDelegate(StatusDisplay));
                }
            }

            return 0;
        }

        private IComparable ChangeType(string iValue, Type desType)
        {
            IComparable tValue = null;
            try
            {
                tValue = (IComparable)Convert.ChangeType(iValue, desType);
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
            return tValue;
        }

        #endregion

        #region 保存结果

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            try
            {
                if (toolStripMenuItem5.Checked == true)
                {
                    iConfig.IsSaveToTxt = true;
                }
                else
                {
                    iConfig.IsSaveToTxt = false;
                }
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            try
            {
                if (toolStripMenuItem6.Checked == true)
                {
                    iConfig.IsSaveToCSV = true;
                }
                else
                {
                    iConfig.IsSaveToCSV = false;
                }
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        #region save txt result

        void saveTXTResult(String PSNnumber, int FailedIndex)
        {
            string[] dateStr = DateTime.Now.ToShortDateString().Split('/');
            string[] timeStr = DateTime.Now.ToLongTimeString().Split(':');
            string fileName = "";

            int n = 0;
            bool notRepeat = false;
            fileName = iConfigWindow.TestResultPath_txt + PSNnumber + ".txt";
            do
            {
                if (File.Exists(fileName))
                {
                    n++;
                    fileName = iConfigWindow.TestResultPath_txt + PSNnumber + "(" + n.ToString() + ")" + ".txt";
                }
                else
                {
                    notRepeat = true;
                }
            } while (notRepeat == false);


            ResultName = PSNnumber + "(" + n.ToString() + ")";
            FileStream aFile = new FileStream(fileName, FileMode.OpenOrCreate);

            StreamWriter sw = new StreamWriter(aFile);
            if (iFailInfor.Fail_Index >= 0)
            {
                sw.WriteLine("Test Result :                    FAIL");
                sw.WriteLine("The last failed index is :           " + iFailInfor.Fail_Index.ToString());
                sw.WriteLine("The last failed function :           " + iFailInfor.Fail_FunctionName);
                sw.WriteLine("The last failed message :           " + iFailInfor.Fail_Message);
            }
            else
            {
                sw.WriteLine("Test Result :                    PASS");
            }

            sw.WriteLine("Start time :" + startTime);
            sw.WriteLine("End time:" + DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToLongTimeString());
            sw.WriteLine("Operator: " + ActiveUser.UserName);
            //sw.WriteLine("Project: " + GetCustomerVariant("S_ProjectName").ToString());
            SaveProjectInformation(sw);
            sw.WriteLine("\n");
            //Write title
            string TitleStr ="";
            foreach(string tString in dvgColumnHeaders)
            {
                TitleStr+=tString+",";
            }
            sw.WriteLine(TitleStr.TrimEnd(','));
            int i = 0;
            foreach (TestObject iTestObject in TestObjects)
            {
                if (iTestObject.IsSelect == false || iTestObject.IsShowResult==false)
                {
                    continue;
                }

                foreach (Spec iSpec in iTestObject.SpecCollection)
                {
                    //0First header "Index";
                    //sw.Write(Convert.ToString(i + 1));
                    sw.Write(Convert.ToString(iSpec.Index));
                    sw.Write(",");

                    //1th header "Items"
                    sw.Write(iSpec.MeasureName);
                    sw.Write(",");

                    //2th header "Result"      
                    sw.Write(iSpec.MeasureValue);
                    sw.Write(",");

                    //3th header  "Unit"
                    sw.Write(iSpec.Unit);
                    sw.Write(",");

                    //5th header "Low Limit"
                    sw.Write(iSpec.LowLimit);
                    sw.Write(",");

                    //6th header "High Limit"
                    sw.Write(iSpec.UpLimit);
                    sw.Write(",");

                    //4th header "Judge"
                    sw.Write(iSpec.MeasureResult);
                    sw.Write(",");

                    //7 "Test Time"
                    sw.Write(iSpec.TestTime);
                    sw.Write(",");

                    //8 "Remark"
                    sw.WriteLine(iSpec.Remark);
                    i++;
                }
            }
            sw.Close();
            this.Invoke(new myDelegate(ResumeWindow));

        }

        #endregion

        #region save csv result

        void saveCSVResult(String PSNnumber, int FailedIndex)
        {
            string[] dateStr = DateTime.Now.ToShortDateString().Split('/');
            string[] timeStr = DateTime.Now.ToLongTimeString().Split(':');
            string fileName = "";


            int n = 0;
            bool notRepeat = false;
            fileName = iConfigWindow.TestResultPath_csv + PSNnumber + ".csv";
            do
            {
                if (File.Exists(fileName))
                {
                    n++;
                    fileName = iConfigWindow.TestResultPath_csv + PSNnumber + "(" + n.ToString() + ")" + ".csv";
                }
                else
                {
                    notRepeat = true;
                }
            } while (notRepeat == false);

            ResultName = PSNnumber + "(" + n.ToString() + ")";

            FileStream aFile = new FileStream(fileName, FileMode.OpenOrCreate);

            StreamWriter iCsvFileWriter = new StreamWriter(aFile);

            if (iFailInfor.Fail_Index >= 0)
            {
                iCsvFileWriter.WriteLine("Test Result :                    FAIL");
                iCsvFileWriter.WriteLine("The last failed index is :           " + iFailInfor.Fail_Index.ToString());
                iCsvFileWriter.WriteLine("The last failed function :           " + iFailInfor.Fail_FunctionName);
                iCsvFileWriter.WriteLine("The last failed message :           " + iFailInfor.Fail_Message);

            }
            else
            {
                iCsvFileWriter.WriteLine("Test Result :                    PASS");
            }

            iCsvFileWriter.WriteLine("Start time :" + startTime);
            iCsvFileWriter.WriteLine("End time:" + DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToLongTimeString());
            iCsvFileWriter.WriteLine("Operator: "+ActiveUser.UserName);
            //iCsvFileWriter.WriteLine("Project: " + GetCustomerVariant("S_ProjectName").ToString());
            SaveProjectInformation(iCsvFileWriter);
            iCsvFileWriter.WriteLine("\n");
            //Write title
            string TitleStr = "";
            foreach (string tString in dvgColumnHeaders)
            {
                TitleStr += tString + ",";
            }
            iCsvFileWriter.WriteLine(TitleStr.TrimEnd(','));
            int i = 0;
            foreach (TestObject iTestObject in TestObjects)
            {
                if (iTestObject.IsSelect == false || iTestObject.IsShowResult==false)
                {
                    continue;
                }
                foreach (Spec iSpec in iTestObject.SpecCollection)
                {
                    //0First header "Index";
                    //iCsvFileWriter.Write(Convert.ToString(i + 1));
                    iCsvFileWriter.Write(Convert.ToString(iSpec.Index));
                    iCsvFileWriter.Write(",");

                    //1th header "Items"
                    iCsvFileWriter.Write(iSpec.MeasureName);
                    iCsvFileWriter.Write(",");

                    //2th header "Result"      
                    iCsvFileWriter.Write(iSpec.MeasureValue);
                    iCsvFileWriter.Write(",");

                    //3th header  "Unit"
                    iCsvFileWriter.Write(iSpec.Unit);
                    iCsvFileWriter.Write(",");

                    //5th header "Low Limit"
                    iCsvFileWriter.Write(iSpec.LowLimit);
                    iCsvFileWriter.Write(",");

                    //6th header "High Limit"
                    iCsvFileWriter.Write(iSpec.UpLimit);
                    iCsvFileWriter.Write(",");

                    //4th header "Judge"
                    iCsvFileWriter.Write(iSpec.MeasureResult);
                    iCsvFileWriter.Write(",");

                    //7 "Test Time"
                    iCsvFileWriter.Write(iSpec.TestTime);
                    iCsvFileWriter.Write(",");

                    //8 "Remark"
                    iCsvFileWriter.WriteLine(iSpec.Remark);
                    i++;
                }
            }
            iCsvFileWriter.Close();
            aFile.Close();
            this.Invoke(new myDelegate(ResumeWindow));

        }

        #endregion

        #region Save summary information

        void saveSummaryInfomation(String PSNnumber, int FailedIndex)
        {
            if (iConfig.IsSaveToCSV == false && iConfig.IsSaveToTxt == false) return;
            string fileName =iConfigWindow.TestResultPath + SelectedTestPlan + ".csv";//selectedTableName is current test plan name

            FileStream aFile;
            string iToWrite = "";
            StreamWriter iCsvFileWriter;
            if (!File.Exists(fileName))
            {
                aFile = new FileStream(fileName, FileMode.Create);
                iCsvFileWriter = new StreamWriter(aFile);
                iToWrite = "SN,TestResult,RecordFile,TestTime";
                iCsvFileWriter.WriteLine(iToWrite);
            }
            else
            {
                aFile = new FileStream(fileName, FileMode.Append);
                iCsvFileWriter = new StreamWriter(aFile);
            }


            //save summary information
            String[] dateStr = DateTime.Now.ToShortDateString().Split('/');
            String[] timeStr = DateTime.Now.ToLongTimeString().Split(':');
            String testTime = DateTime.Now.ToShortDateString() + "-" + DateTime.Now.ToLongTimeString();

            String TestResult = "";


            if (iFailInfor.Fail_Index >= 0)
            {
                TestResult = "FAIL";
            }
            else
            {
                TestResult = "PASS";
            }

           
            string finalrecordFilename = "";
            if (iConfig.IsSaveToTxt == true)
            {
                finalrecordFilename = ResultName + ".txt";
            }
            if (iConfig.IsSaveToCSV == true)
            {
                finalrecordFilename += "/" + ResultName + ".csv";
            }
            iToWrite = PSNnumber.ToString() + "," + TestResult.ToString() + ","  + finalrecordFilename + "," + testTime.ToString();

            iCsvFileWriter.WriteLine(iToWrite);
            iCsvFileWriter.Close();
            aFile.Close();

            this.Invoke(new myDelegate(ResumeWindow));

        }

        #endregion

      



        #endregion

        #endregion

        #region 退出系统

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                iConfigWindow.XmlSaveConfigFile(iConfig);
                if (producer != null)
                {
                    bool state = producer.IsAlive;
                    if (true == state)
                    {
                        producer.Abort();
                        producer = null;
                    }

                }
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        #endregion

        #region 窗体处理


        #region UI Buttons

        private void ResumeWindow()//恢复初始状态
        {
            if (iConfig.IsLoopTest == false)
            {
                toolStripTextBox1.Enabled = true;//恢复SN窗口
                TestToolStripButton.Enabled = true;
                LoadToolStripButton.Enabled = true;
                RefreshTtoolStripButton.Enabled = true;
                PauseToolStripButton.Enabled = false;
                StopToolStripButton.Enabled = false;
            }
        }

        private void toolStripMenuItem_UserCenter_Click(object sender, EventArgs e)
        {
            try
            {
                if (mUCW.IsDisposed == true)
                {
                    mUCW = new UserCenterWindows();
                }
                mUCW.SetUIVisablity(ActiveUser.UserType);
                mUCW.LoadActiveUserData(ActiveUser);
                mUCW.Show();
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        //show config window
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (iConfigWindow.IsDisposed == true)
                {
                    iConfigWindow = new ConfigWindow();
                }
                if (iConfigWindow.RootPath == "")
                {
                    iConfigWindow.RootPath = iConfig.RootPath;
                    iConfigWindow.UpdateFilePath(iConfig.RootPath);
                    if (iConfigWindow.SelectedPlan == "")
                    {
                        string[] tStringArray = iConfig.RootPath.Split('\\');
                        iConfigWindow.SelectedPlan = tStringArray[tStringArray.Length - 1];
                    }
                }
                iConfigWindow.Show();
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        private void toolStripMenuItem_Instrument_Click(object sender, EventArgs e)
        {
            try
            {
                if (iInstrumentsSetup.IsDisposed == true)
                {
                    iInstrumentsSetup = new InstrumentsSetup();
                }
                iInstrumentsSetup.LoadInstruments(iInstruments);
                iInstrumentsSetup.Show();
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        //Set Test Flow Mode
        private void testFlowModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (testFlowModeToolStripMenuItem.Checked == true)
                {
                    iConfig.IsStopWhenFailed = true;
                }
                else
                {
                    iConfig.IsStopWhenFailed = false;
                }
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        #endregion

        #region Is Loop Test
        private void loopTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (loopTestToolStripMenuItem.Checked == true)
                {
                    iConfig.IsLoopTest = true;
                }
                else
                {
                    iConfig.IsLoopTest = false;
                }
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        #endregion


        #region Information Board

        private void ResultJudgeDisplay_Fail()
        {
            if (iConfig.IsLoopTest == false)
            {
                TestToolStripButton.Enabled = true;
                LoadToolStripButton.Enabled = true;
                RefreshTtoolStripButton.Enabled = true;
                PauseToolStripButton.Enabled = false;
                StopToolStripButton.Enabled = false;
                toolStripTextBox1.Enabled = true;
            }

            label_Status.Text = "Fail";
            label_Status.ForeColor = Color.Red;
    
            toolStripTextBox1.Focus();
            toolStripTextBox1.Text = "";
            progressBar1.Visible = false;
        }

        private void ResultJudgeDisplay_Pass()
        {

            //iStatusInfo.imageIndex = 2;
            //iStatusInfo.Title = "Pass";
            //iStatusInfo.Detail = "";
            //this.Invoke(new myDelegate(StatusDisplay));
            label_Status.Text = "Pass";
            label_Status.ForeColor = Color.Blue;

            toolStripTextBox1.Focus();
            toolStripTextBox1.Text = "";
            progressBar1.Visible = false;
        }

        #endregion


        #region status bar

        private void StatusDisplay()
        {
            if (iStatusInfo.imageIndex == 1)//error information
            {
                this.label_Status.ForeColor = Color.Red;
                this.InforDes.ForeColor = Color.Red; 
            }
            if (iStatusInfo.imageIndex == 2)//normal information
            {
                this.label_Status.ForeColor = Color.Blue;
                this.InforDes.ForeColor = Color.Blue;
            }
            if (iStatusInfo.Title.Contains("Fail") && iStatusInfo.Detail == "")
            {
                //this.label_Status.Text = iStatusInfo.Title;
                FontAdaptText(this.label_Status, iStatusInfo.Title);
            }
            else
            {
                //this.label_Status.Text = iStatusInfo.Title;
                FontAdaptText(this.label_Status, iStatusInfo.Title);
                this.InforDes.Text = iStatusInfo.Detail;
              
            }
        }

        #endregion


        #region DatagGridView
        //display TestObjects
        private void CreatColumnHeaders(string[] dvgColumnHeaders)
        {
            foreach (string iColunHeader in dvgColumnHeaders)
            {
                dataGridView1.Columns.Add(iColunHeader, iColunHeader);
            }
            dataGridView1.Columns["Index"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            dataGridView1.Columns["Unit"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            dataGridView1.Columns["Judge"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

        }

        private void TestObjects_Display_dvg(TestObjects newTestObjects)
        {
            if (newTestObjects.Count > 0)
            {
                dataGridView1.Rows.Clear();
                int RowCount = 0;
                string cellValue = "";
                foreach (TestObject newTestObject in newTestObjects)
                {
                        #region display functions

                            DataGridViewRow newFunctionRow = new DataGridViewRow();
                            dataGridView1.Rows.Add(newFunctionRow);
                            RowCount++;
                            newTestObject.Index = RowCount;
                            foreach (string iColumnHeader in dvgColumnHeaders)
                            {
                                switch (iColumnHeader)
                                { 
                                    case "Index":
                                        cellValue = RowCount.ToString();
                                        break;
                                    //case "Items":
                                    //    cellValue = newTestObject.FunctionName;
                                    //    break;
                                    case "Items":
                                        cellValue = newTestObject.DisplayText;
                                        break;
                                    default:
                                        cellValue = "";
                                        break;
                                }
                                dataGridView1.Rows[RowCount - 1].Cells[iColumnHeader].Value = cellValue;
                            }
                            dataGridView1.Rows[RowCount - 1].DefaultCellStyle.BackColor = FunctionRowColorList[ReadyColor];
                            dataGridView1.Rows[RowCount - 1].Visible = iConfig.IsShowFunctionRow;
                            

                        #endregion

                        if (newTestObject.IsShowResult != false)
                        {
                            #region display Testitems

                            foreach (Spec newSpec in newTestObject.SpecCollection)
                            {
                                DataGridViewRow newTestItemRow = new DataGridViewRow();
                                dataGridView1.Rows.Add(newTestItemRow);
                                RowCount++;
                                newSpec.Index = RowCount;
                                foreach (string iColumnHeader in dvgColumnHeaders)
                                {
                                    switch (iColumnHeader)
                                    { 
                                        case "Index":
                                            cellValue = RowCount.ToString();
                                            break;
                                        case "Items":
                                            cellValue = newSpec.MeasureName;
                                            break;
                                        case "Result":
                                            cellValue = newSpec.MeasureValue;
                                            break;
                                        case "Unit":
                                            cellValue = newSpec.Unit;
                                            break;
                                        case "Low Limit":
                                            cellValue = newSpec.LowLimit;
                                            break;
                                        case "High Limit":
                                            cellValue = newSpec.UpLimit;
                                            break;
                                        case "Judge":
                                            cellValue = newSpec.MeasureResult;
                                            break;
                                        case "Description":
                                            cellValue = newSpec.Remark;
                                            break;
                                        default:
                                            cellValue = "";
                                            break;
                                    }
                                    dataGridView1.Rows[RowCount - 1].Cells[iColumnHeader].Value = cellValue;
                                }

                                dataGridView1.Rows[RowCount - 1].DefaultCellStyle.BackColor = TestItemRowColorList[ReadyColor];
                                dataGridView1.Rows[RowCount - 1].Visible = iConfig.IsShowTestItemRow;
                            }

                            #endregion
                        }
                }
                if (IsInstrConfigFileExits == true)
                {
                    toolStripMenuItem_Instrument.Visible = true;
                }
                else
                {
                    toolStripMenuItem_Instrument.Visible = false;
                }
                //set max progress
                this.Invoke(new deleProgressSet(SetProgressbarMaxValue), (RowCount));
            }
        }
       
        private void MeasureDisplay_DVG(TestObject iTestObject)
        {
            if (iTestObject.FunExecuteStatus < 0)
            {
                dataGridView1.Rows[iTestObject.Index - 1].Cells["Judge"].Value = "FAIL";
                dataGridView1.Rows[iTestObject.Index - 1].Cells["Test Time"].Value = iTestObject.TestTime;
                dataGridView1.Rows[iTestObject.Index - 1].Cells["Description"].Value = iTestObject.Description;
                dataGridView1.Rows[iTestObject.Index - 1].DefaultCellStyle.BackColor = Color.Red;
                dataGridView1.Rows[iTestObject.Index - 1].Visible = true;
                if (iTestObject.IsShowResult == true)
                {
                    foreach (Spec iSpec in iTestObject.SpecCollection)
                    {
                        dataGridView1.Rows[iSpec.Index - 1].DefaultCellStyle.BackColor = Color.Red;
                        dataGridView1.Rows[iSpec.Index - 1].Visible = true; 
                    }
                }
                return;
            }
            else
            {
                dataGridView1.Rows[iTestObject.Index - 1].Cells["Judge"].Value = "PASS";
                dataGridView1.Rows[iTestObject.Index - 1].Cells["Test Time"].Value = iTestObject.TestTime;
                dataGridView1.Rows[iTestObject.Index - 1].DefaultCellStyle.BackColor = FunctionRowColorList[ResultColor];
                //set progress value
                this.Invoke(new deleProgressSet(SetProgressbarValue), (iTestObject.Index));
            }

            if (iTestObject.IsShowResult == true)
            {
                foreach (Spec iSpec in iTestObject.SpecCollection)
                {
                    dataGridView1.Rows[iSpec.Index - 1].Cells["Result"].Value = iSpec.MeasureValue;
                    dataGridView1.Rows[iSpec.Index - 1].Cells["Judge"].Value = iSpec.MeasureResult;
                    dataGridView1.Rows[iSpec.Index - 1].Cells["Test Time"].Value = iSpec.TestTime;
                    dataGridView1.Rows[iSpec.Index - 1].Cells["Description"].Value = iSpec.Remark;
                    if (iSpec.MeasureResult.ToUpper()=="FAIL")
                    {
                        dataGridView1.Rows[iSpec.Index - 1].DefaultCellStyle.BackColor = Color.Red;
                        dataGridView1.Rows[iSpec.Index - 1].Visible = true;
                    }
                    else
                    {
                        dataGridView1.Rows[iSpec.Index - 1].DefaultCellStyle.BackColor = TestItemRowColorList[ResultColor];
                        //set progress value
                        this.Invoke(new deleProgressSet(SetProgressbarValue), (iSpec.Index));
                    }
                }
            }
        }

        private void MeasDisplayClear_DVG()
        {
            int rowIndex = -1;
            foreach (TestObject iTestObject in TestObjects)
            {
                if (iTestObject.ExecuteType.ToUpper() != "ONCE" || iTestObject.IsHasBeenExecuted == false)
                {
                    rowIndex = iTestObject.Index - 1;
                    iTestObject.FunExecuteStatus = 0;
                    iTestObject.FailIndex = -1;
                    iTestObject.TestTime = "";
                    dataGridView1.Rows[rowIndex].Cells["Result"].Value = "";
                    dataGridView1.Rows[rowIndex].Cells["Judge"].Value = "";
                    dataGridView1.Rows[rowIndex].Cells["Test Time"].Value = "";
                    dataGridView1.Rows[rowIndex].Cells["Description"].Value = "";
                    dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = FunctionRowColorList[ReadyColor];
                }
                //here

                if (iTestObject.IsShowResult)
                {
                    foreach (Spec iSpec in iTestObject.SpecCollection)
                    {
                        rowIndex = iSpec.Index - 1;
                        iSpec.MeasureValue = "";
                        iSpec.MeasureResult = "";
                        iSpec.TestTime = "";
                        dataGridView1.Rows[rowIndex].Cells["Result"].Value = "";
                        dataGridView1.Rows[rowIndex].Cells["Judge"].Value = "";
                        dataGridView1.Rows[rowIndex].Cells["Test Time"].Value = "";
                        dataGridView1.Rows[rowIndex].Cells["Description"].Value = "";
                        dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = TestItemRowColorList[ReadyColor];
                    }
                }
            }
            //set max progress
            this.Invoke(new deleProgressSet(SetProgressbarMaxValue), (rowIndex+1));
            progressBar1.Visible = true;
            Application.DoEvents();
        }

        private void DisplayClear()
        {
            dataGridView1.Rows.Clear();
        }

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
            }
            else
            {
                dataGridView1.ClearSelection();
            }
        }

        #endregion

     


        #region //set progress

        private void SetProgressbarMaxValue(int ProgressValue)
        {
            progressBar1.Minimum = 0;
            progressBar1.Maximum = ProgressValue;
        }

        private void SetProgressbarValue(int ProgressValue)
        {
            progressBar1.Value = ProgressValue;
        }

        #endregion

      
        #region //SN窗口处理

        //SN 扫描
        private String getSN()
        {
            if (toolStripTextBox1.Text == "")
            {
                MessageBox.Show("SN number is empty !");
                return "";
            }
            else
            {
                String SNStr = "";
                Regex SNtext = new Regex(@"^[A-Za-z0-9]+$");
                if (!SNtext.IsMatch(toolStripTextBox1.Text))
                {

                    MessageBox.Show("Only characters and numbers is permitted !");
                    return "";
                }
                else
                {
                    SNStr = toolStripTextBox1.Text;
                    if (SNStr.Length <= 30)
                    {
                        return SNStr;
                    }
                    else
                    {
                        MessageBox.Show("The length of the Inputed characters is too long !");
                        return "";
                    }
                }
            }

        }

        private void toolStripMenuItem_Forbid_Click(object sender, EventArgs e)
        {
            try
            {
                if (toolStripMenuItem_Forbid.Text == "Forbid Manual Input")
                {
                    iConfig.IsAllowManualInput_SN = false;
                    iConfig.IsAllowManualInput_SN = false;
                    toolStripMenuItem_Forbid.Text = "Allow Manual Input";
                    toolStripTextBox1.Text = "";
                }
                else
                {
                    iConfig.IsAllowManualInput_SN = true;
                    iConfig.IsAllowManualInput_SN = true;
                    toolStripMenuItem_Forbid.Text = "Forbid Manual Input";
                }
                toolStripTextBox1.TextBox.Focus();
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (iConfig.IsAllowManualInput_SN == false) e.Handled = true;
        }

        private void toolStripMenuItem_UpperCase_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripTextBox1.CharacterCasing = CharacterCasing.Upper;
                iConfig.CharacterCaseType_SN = "Upper";
                toolStripMenuItem_UpperCase.Checked = true;
                toolStripMenuItem_LowerCase.Checked = false;
                toolStripMenuItem_NormalCase.Checked = false;
                toolStripTextBox1.TextBox.Focus();
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        private void toolStripMenuItem_LowerCase_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripTextBox1.CharacterCasing = CharacterCasing.Lower;
                iConfig.CharacterCaseType_SN = "Lower";
                toolStripMenuItem_UpperCase.Checked = false;
                toolStripMenuItem_LowerCase.Checked = true;
                toolStripMenuItem_NormalCase.Checked = false;
                toolStripTextBox1.TextBox.Focus();
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        private void toolStripMenuItem_NormalCase_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripTextBox1.CharacterCasing = CharacterCasing.Normal;
                iConfig.CharacterCaseType_SN = "Normal";
                toolStripMenuItem_UpperCase.Checked = false;
                toolStripMenuItem_LowerCase.Checked = false;
                toolStripMenuItem_NormalCase.Checked = true;
                toolStripTextBox1.TextBox.Focus();
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        private void toolStripTextBox1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (IsControlSN == true)
                    {
                        toolStripTextBox1.TextBox.ContextMenuStrip = contextMenuStrip_SN;
                    }
                    else
                    {
                        toolStripTextBox1.TextBox.ContextMenuStrip = null;
                    }
                }
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }


        #endregion


        #region 窗体键盘事件

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {
                    e.Handled = true;
                    this.runToolStripMenuItem_Click(null, null);
                }
                if (e.KeyCode == Keys.F4)
                {
                    e.Handled = true;
                    this.PauseToolStripButton_Click(null, null);
                }
                if (e.KeyCode == Keys.F9)
                {
                    e.Handled = true;
                    this.StopToolStripButton_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    this.runToolStripMenuItem_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        private void PauseToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                switch (PauseToolStripButton.Text.ToUpper())
                {
                    case "PAUSE":
                        RunStatus = "Pause";
                        PauseToolStripButton.Text = "Resume";
                        PauseToolStripButton.ToolTipText = "Resume";
                        break;
                    case "RESUME":
                        RunStatus = "Resume";
                        PauseToolStripButton.Text = "Pause";
                        PauseToolStripButton.ToolTipText = "Pause";

                        break;
                    default:
                        RunStatus = "Pause";
                        PauseToolStripButton.Text = "Resume";
                        PauseToolStripButton.ToolTipText = "Resume";
                        break;
                }
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        private void StopToolStripButton_Click(object sender, EventArgs e)
        {
            RunStatus = "Stop";
        }
       
        #endregion


        #region//界面显示

        private void displayFuntionRowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (displayFuntionRowsToolStripMenuItem.Checked == true)
                {
                    displayFuntionRowsToolStripMenuItem.Checked = false;
                    iConfig.IsShowFunctionRow = false;
                }
                else
                {
                    displayFuntionRowsToolStripMenuItem.Checked = true;
                    iConfig.IsShowFunctionRow = true;
                }
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        private void displayTestItemRowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (displayTestItemRowsToolStripMenuItem.Checked == true)
                {
                    displayTestItemRowsToolStripMenuItem.Checked = false;
                    iConfig.IsShowTestItemRow = false;
                }
                else
                {
                    displayTestItemRowsToolStripMenuItem.Checked = true;
                    iConfig.IsShowTestItemRow = true;
                }
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        private void displayInformationBoardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (displayInformationBoardToolStripMenuItem.Checked == true)
                {
                    displayInformationBoardToolStripMenuItem.Checked = false;
                    iConfig.IsShowInfoBorad = false;
                }
                else
                {
                    displayInformationBoardToolStripMenuItem.Checked = true;
                    iConfig.IsShowInfoBorad = true;
                }
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        private void displayStatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (displayStatusBarToolStripMenuItem.Checked == true)
                {
                    displayStatusBarToolStripMenuItem.Checked = false;
                    iConfig.IsShowStatusBar = false;
                }
                else
                {
                    displayStatusBarToolStripMenuItem.Checked = true;
                    iConfig.IsShowStatusBar = true;
                }
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        private void displayStatusBarToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (displayStatusBarToolStripMenuItem.Checked == true)
                {
                    splitContainer3.Panel2Collapsed = false;
                    splitContainer3.Panel2.Show();
                }
                else
                {
                    splitContainer3.Panel2Collapsed = true;
                    splitContainer3.Panel2.Hide();
                }
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        private void displayInformationBoardToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (displayInformationBoardToolStripMenuItem.Checked == true)
                {
                    splitContainer2.Panel2Collapsed = false;
                    splitContainer2.Panel2.Show();
                }
                else
                {
                    splitContainer2.Panel2Collapsed = true;
                    splitContainer2.Panel2.Hide();
                }
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        private void displayTestItemRowsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                bool rowVisible = false;
                if (displayTestItemRowsToolStripMenuItem.Checked == true)
                {
                    rowVisible = true;
                }
                else
                {
                    rowVisible = false;
                }

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (TestItemRowColorList.Contains(dataGridView1.Rows[i].DefaultCellStyle.BackColor))
                    {
                        dataGridView1.Rows[i].Visible = rowVisible;
                    }
                }
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        private void displayFuntionRowsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                bool rowVisible = false;
                if (displayFuntionRowsToolStripMenuItem.Checked == true)
                {
                    rowVisible = true;
                }
                else
                {
                    rowVisible = false;
                }


                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (FunctionRowColorList.Contains(dataGridView1.Rows[i].DefaultCellStyle.BackColor))
                    {
                        dataGridView1.Rows[i].Visible = rowVisible;
                    }
                }
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        

        public void DisplayInstrumentConfigButton(bool IsShow)
        {
            if (IsShow)
            {
                toolStripMenuItem_Instrument.Visible = true;
                IsInstrConfigFileExits = true;
            }
            else
            {
                toolStripMenuItem_Instrument.Visible = false;
                IsInstrConfigFileExits = false;
                iInstruments.Clear();
            }
            Application.DoEvents();
        }

        #endregion

        #endregion

        #region 用户权限管理

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                mLW.Show();
                loginToolStripMenuItem.Enabled = false;
                loginOffToolStripMenuItem.Enabled = true;
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        private void registerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                mRW.Show();
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        public void UserCenterWindowClosed()
        {
            try
            {
                XmlSaveUsersRecord(iUsers);
                mLW.LoadUserRecord(iUserNameList);
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }

        }

        private void loginOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                LoadToolStripButton.Enabled = false;
                RefreshTtoolStripButton.Enabled = false;
                TestToolStripButton.Enabled = false;
                setToolStripMenuItem.Enabled = false;
                toolStripMenuItem_View.Enabled = false;
                toolStripMenuItem_UserCenter.Enabled = false;
                toolStripTextBox1.Enabled = false;
                SN.Enabled = false;
                IsControlSN = false;
                loginToolStripMenuItem.Enabled = true;
                registerToolStripMenuItem.Enabled = true;
                loginOffToolStripMenuItem.Enabled = false;
                toolStripMenuItem_Users.Text = "Users";
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        public void Confirm_Login_Click()
        {
            try
            {
                string inputUserName = mLW.GetUserName();
                string inputPassword = mLW.GetInputPassword();
                User iUser = GetTargetUser(inputUserName);
                if (iUser == null)
                {
                    MessageBox.Show("Not registered name!", "Message", MessageBoxButtons.OK);
                    return;
                }
                if (inputPassword == iUser.Password)
                {
                    iUser.LastLoginTime = DateTime.Now.ToString();
                    UpdateUserData(iUser);
                    switch (iUser.UserType)
                    {
                        case "Programmer":
                            setToolStripMenuItem.Enabled = true;
                            toolStripMenuItem_View.Enabled = true;
                            toolStripMenuItem_UserCenter.Enabled = true;
                            toolStripTextBox1.Enabled = true;
                            IsControlSN = true;
                            SN.Enabled = true;
                            mLW.Hide();
                            ActiveUser = iUser;
                            break;
                        case "Administrator":
                            setToolStripMenuItem.Enabled = true;
                            toolStripMenuItem_View.Enabled = true;
                            toolStripMenuItem_UserCenter.Enabled = true;
                            toolStripTextBox1.Enabled = true;
                            IsControlSN = true;
                            SN.Enabled = true;
                            mLW.Hide();
                            ActiveUser = iUser;
                            break;
                        case "Common User":
                            toolStripTextBox1.Enabled = true;
                            toolStripMenuItem_UserCenter.Enabled = true;
                            IsControlSN = false;
                            SN.Enabled = true;
                            mLW.Hide();
                            ActiveUser = iUser;
                            break;
                    }

                    loginToolStripMenuItem.Enabled = false;
                    registerToolStripMenuItem.Enabled = false;
                    loginOffToolStripMenuItem.Enabled = true;
                    LoadToolStripButton.Enabled = true;
                    RefreshTtoolStripButton.Enabled = true;
                    toolStripMenuItem_Users.Text = ActiveUser.UserName;
                }
                else
                {
                    MessageBox.Show("Wrong Password!", "Message", MessageBoxButtons.OK);
                    toolStripMenuItem_Users.Text = "Users";
                }

                mLW.ClearPassword();
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        public void Cancel_Login_Click()
        {
            try
            {
                loginToolStripMenuItem.Enabled = true;
                loginOffToolStripMenuItem.Enabled = false;
                mLW.Hide();
                return;
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        public void Confirm_Register_Click()
        {
            try
            {
                if (GetTargetUser(mRW.GetUserName()) != null)
                {
                    MessageBox.Show("This name has already existed!", "Message", MessageBoxButtons.OK);
                    return;
                }
                User iUser = new User();
                iUser.UserName = mRW.GetUserName();
                iUser.UserType = "Common User";
                iUser.Password = mRW.GetInputPassword();
                iUser.LastLoginTime = DateTime.Now.ToString();
                SaveNewUser(iUserNameList, iUsers, iUser);

                ActiveUser = iUser;
                LoadToolStripButton.Enabled = true;
                RefreshTtoolStripButton.Enabled = true;
                toolStripTextBox1.Enabled = true;
                loginToolStripMenuItem.Enabled = false;
                registerToolStripMenuItem.Enabled = false;
                loginOffToolStripMenuItem.Enabled = true;
                SN.Enabled = true;
                toolStripMenuItem_Users.Text = ActiveUser.UserName;
                mRW.Hide();
                mRW.ClearPassword();
                loginToolStripMenuItem.Enabled = false;
                loginOffToolStripMenuItem.Enabled = true;
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        public void Cancel_Register_Click()
        {
            try
            {
                loginToolStripMenuItem.Enabled = true;
                registerToolStripMenuItem.Enabled = true;
                loginOffToolStripMenuItem.Enabled = false;
                mRW.Hide();
            }
            catch (Exception ex)
            {
                DealwithException(ex);
            }
        }

        private User GetTargetUser(string iUserName)
        {
            foreach (User iUser in iUsers)
            {
                if (iUser.UserName == iUserName)
                {
                    return iUser;
                }
            }
            return null;
        }

        private void SaveNewUser(List<string> iList, Users iUsers, User iUser)
        {
            if (GetTargetUser(mRW.GetUserName()) == null)
            {
                iList.Add(iUser.UserName);
                iUsers.Add(iUser);
                XmlSaveUsersRecord(iUsers);
                mLW.LoadUserRecord(iList);
            }
        }

        public void UpdateUserData(User nUser)
        {
            if (nUser.UserName == "" || nUser.UserType == "" || nUser.Password == "") return;
            foreach (User oUser in iUsers)
            {
                if (nUser.UserName == oUser.UserName)
                {
                    iUsers.Remove(oUser);
                    iUsers.Add(nUser);
                    XmlSaveUsersRecord(iUsers);
                    break;
                }
            }
        }
      

        #endregion

        #region functions

        protected bool isNumberic(string message)
        {
            System.Text.RegularExpressions.Regex rex =
            new System.Text.RegularExpressions.Regex(@"^(-?\d+[.]?\d+)$");

            if (rex.IsMatch(message))
            {
                return true;
            }
            else
                return false;
        }

        //read user record
        private Users XmlReadUsersRecord()
        {
            string iFileName = Application.StartupPath + "\\Users.xml";
            if (!File.Exists(iFileName))
            {
                FileStream iFileStream = File.Create(iFileName);
                iFileStream.Close();
            }
            iUsers.Clear();
            if (new FileInfo(iFileName).Length <= 0)
            {
                return iUsers;
            }

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
                        if (iXmlReader.Name=="User" && iXmlReader.NodeType == XmlNodeType.Element)
                        {

                            #region read to dictionary

                            tDic.Clear();
                            iList.Clear();
                            do
                            {
                                iString = iConfigWindow.XmlReadString(iXmlReader);
                                if (iXmlReader.Name=="User" && iXmlReader.NodeType == XmlNodeType.EndElement)
                                {
                                    break;
                                }
                                else
                                {
                                    tDic.Add(iXmlReader.Name, iString);
                                }
                            } while (iXmlReader.Name!="User");

                            #endregion

                            #region 根据变量名称对应赋值

                            User iUser = new User();
                            foreach (var iUserElement in iUser.GetType().GetProperties())
                            {
                                iString = "";
                                foreach (var iDic in tDic)
                                {
                                    if (iDic.Key == iUserElement.Name)
                                    {
                                        iString = iDic.Value;
                                        break;
                                    }
                                }
                                switch (iUserElement.Name)
                                {
                                    case "UserName":
                                        iUser.UserName = iString;
                                        break;
                                    default:
                                        iString = DecryptString(iUser.UserName, iString);
                                        iString = iString.Replace("&" + iUser.UserName + "&", "");
                                        iUserElement.SetValue(iUser, iString, new object[] { });
                                        break;
                                }
                            }

                            #endregion
                  
                            if (iUser.UserName != "" && iUser.UserType != "" && iUser.Password != "")
                            {
                                iUsers.Add(iUser);
                            }
                        }

                    }
                }
            }


            #endregion

            return iUsers;
        }
      
        //write user record
        private void XmlSaveUsersRecord(Users iUsers)
        {
            if (iUsers == null) return;
            string iFileName = Application.StartupPath + "\\Users.xml";

            #region Write

            using (FileStream fileSteam = File.Create(iFileName))
            {
                string iString = "";
                XmlWriterSettings iXmlWriterSettings = new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 };

                using (XmlWriter iXmlWriter = XmlWriter.Create(fileSteam, iXmlWriterSettings))
                {
                    iXmlWriter.WriteStartDocument();
                    iXmlWriter.WriteStartElement("Users");
                    foreach (User iUser in iUsers)
                    {
                        if (iUser.UserName != "" && iUser.UserType != "" && iUser.Password != "")
                        {
                            iXmlWriter.WriteStartElement("User");
                            foreach (var iUserElement in iUser.GetType().GetProperties())
                            {
                                if (iUserElement.Name == "UserName")
                                {
                                    iXmlWriter.WriteElementString(iUserElement.Name, iUser.UserName);
                                    continue;
                                }
                                object value = iUserElement.GetValue(iUser, new object[] { });
                                iString = EncryptString(iUser.UserName, "&" + iUser.UserName + "&" + value.ToString());
                                iXmlWriter.WriteElementString(iUserElement.Name, iString);
                            }
                            iXmlWriter.WriteEndElement();
                        }
                    }
                    iXmlWriter.WriteEndDocument();
                }
            }

            #endregion
        }

        #region Encrypt

        protected string EncryptString(string refString,string inputString)
        {
            bool iDone=true;
            int iCount = 0;
            string outputString = "";
            do
            {
                iDone = true;
                try
                {
                    string firstResult = Encrypt(".elegantMM", inputString);
                    string secondResult = Encrypt("xie201314@tang", firstResult);
                    outputString= ReEncrypt(refString, secondResult);

                }
                catch (Exception ex)
                {
                    if (ex.Message.ToString().Contains("Input string is invalid")) iDone = false;
                    iCount++;
                }
            }
            while (iDone == false && iCount<10);
            if (iCount == 10)
            {
                Thread.Sleep(100);
            }
            return outputString;

        }

        protected string DecryptString(string refString,string inputString)
        {
            bool iDone = true;
            int iCount = 0;
            string outputString = "";
            do
            {
                iDone = true;
                try
                {
                    string FirstResult = FirstDecrypt(refString, inputString);
                    string SecondRrsult = Decrypt("xie201314@tang", FirstResult);
                    outputString= Decrypt(".elegantMM", SecondRrsult);
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToString().Contains("Input string is invalid")) iDone = false;
                    iCount++;
                }
            }
            while (iDone == false && iCount < 10);
            if (iCount == 10)
            {
                Thread.Sleep(100);
            }
        
            return outputString;
        }

        //Encrypt
        protected string Encrypt(string securityKey, string toEncrypt)
        {
            //string securityKey = ".elegantMM";
            bool useHashing = true;
            string retVal = string.Empty;

            try
            {
                // Validate inputs
                ValidateInput(toEncrypt);
                ValidateInput(securityKey);

                byte[] keyArray;
                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt.TrimEnd('\0'));

                // If hashing use get hashcode regards to your key
                if (useHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(securityKey));

                    // Always release the resources and flush data
                    // of the Cryptographic service provide. Best Practice
                    hashmd5.Clear();
                }
                else
                {
                    keyArray = UTF8Encoding.UTF8.GetBytes(securityKey);
                }

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

                // Set the secret key for the tripleDES algorithm
                tdes.Key = keyArray;

                // Mode of operation. there are other 4 modes.
                // We choose ECB (Electronic code Book)
                tdes.Mode = CipherMode.ECB;

                // Padding mode (if any extra byte added)
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();

                // Transform the specified region of bytes array to resultArray
                byte[] resultArray =
                  cTransform.TransformFinalBlock(toEncryptArray, 0,
                  toEncryptArray.Length);

                // Release resources held by TripleDes Encryptor
                tdes.Clear();

                // Return the encrypted data into unreadable string format
                retVal = Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception ex)
            {
                throw new System.Runtime.InteropServices.ExternalException(ex.Message.ToString(), 1);
            }

            return retVal;
        }

        //ReEncrypt
        protected string ReEncrypt(string refString, string inputString)
        {
            int keyNumber = GetLuckyNumber(refString.Length);
            string iFileName = Application.StartupPath + "\\ILTXB" + keyNumber.ToString() + ".cle";
            return Encrypt(GetSecurityKey(iFileName), inputString);
        }

        //FirstDecrypt
        protected string FirstDecrypt(string refString, string inputString)
        {
            int keyNumber = GetLuckyNumber(refString.Length);
            string iFileName = Application.StartupPath + "\\ILTXB" + keyNumber.ToString() + ".cle";

            return Decrypt(GetSecurityKey(iFileName),inputString);
        }

        //Decrypt
        public string Decrypt(string securityKey, string cipherString)
        {
            //string securityKey = ".elegantMM";
            bool useHashing = true;
            string retVal = string.Empty;

            try
            {
                // Validate inputs
                ValidateInput(cipherString);
                ValidateInput(securityKey);
                byte[] keyArray;
                byte[] toEncryptArray = Convert.FromBase64String(cipherString.TrimEnd('\0'));

                if (useHashing)
                {
                    // If hashing was used get the hash code with regards to your key
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(securityKey));

                    // Release any resource held by the MD5CryptoServiceProvider
                    hashmd5.Clear();
                }
                else
                {
                    // If hashing was not implemented get the byte code of the key
                    keyArray = UTF8Encoding.UTF8.GetBytes(securityKey);
                }

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

                // Set the secret key for the tripleDES algorithm
                tdes.Key = keyArray;

                // Mode of operation. there are other 4 modes. 
                // We choose ECB(Electronic code Book)
                tdes.Mode = CipherMode.ECB;

                // Padding mode(if any extra byte added)
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(
                                     toEncryptArray, 0, toEncryptArray.Length);

                // Release resources held by TripleDes Encryptor
                tdes.Clear();

                // Return the Clear decrypted TEXT
                retVal = UTF8Encoding.UTF8.GetString(resultArray);


            }
            catch (Exception ex)
            {
                throw new System.Runtime.InteropServices.ExternalException(ex.Message.ToString(), 1);
            }

            return retVal;
        }

        /// <summary>
        /// Validates an input value
        /// </summary>
        /// <param name="inputValue">Specified input value</param>
        /// <returns>True | Falue - Value is valid</returns>
        protected bool ValidateInput(string inputValue)
        {
            bool valid = string.IsNullOrEmpty(inputValue);

            if (valid)
            {
                throw new System.Runtime.InteropServices.ExternalException("Input string is invalid.", 1);
            }

            return valid;
        }

        protected int GetLuckyNumber(int inputInteger)
        {
            inputInteger = inputInteger * 3 + 10;
            if (inputInteger / 4 > 10)
            {
                GetLuckyNumber(inputInteger / 4);
            }
            else
            {
                return inputInteger / 4;
            }

            return -1;
        }

        protected string GetSecurityKey(string iFileName)
        {
            string FirstResult = "";
            string initContext = "";
         
            if (System.IO.File.Exists(iFileName) == true)
            {
                FileStream myFs = new FileStream(iFileName, FileMode.Open);
                StreamReader mySr = new StreamReader(myFs);
                initContext = mySr.ReadToEnd().TrimEnd('\0');
                mySr.Close();
                myFs.Close();
            }
            FirstResult = Decrypt("xie201314@tang", initContext);

            return Decrypt(key1, FirstResult);

        }

        #endregion


        //disable columns sort
        private void dataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }


        //get Instruments
        private string GetInstrment(TestObject newTestObject)
        {
            string ReturnString = "";
            string[] tStringArray = newTestObject.ParaCollection.Split(',');
            string[] tStringSubArray = null;
            bool IsInstrExist = false;
            if (IsInstrConfigFileExits == false)
            {
                foreach (var eString in tStringArray)
                {
                    Instrument newInstrument = new Instrument();
                    tStringSubArray = eString.Split('=');
                    if (tStringSubArray.Length > 1)
                    {
                        string[] tString = tStringSubArray[1].Split(';');
                        newInstrument.Name = tStringSubArray[0].Replace("S_Instr_", "");

                        if (tString.Length >= 2) newInstrument.ConnectType = tString[1];
                        if (tString.Length >= 3) newInstrument.Address = tString[2];
                    }
                    foreach (Instrument tInstrument in iInstruments)
                    {
                        if (tInstrument.Name == newInstrument.Name) IsInstrExist = true;
                    }
                    if (IsInstrExist == false)
                    {
                        iInstruments.Add(newInstrument);
                    }
                    ReturnString += tStringSubArray[0] + "=" + newInstrument.Address + ",";

                }
                XmlSaveInstrumentsConfigFile(iInstruments);
                IsInstrConfigFileExits = true;
            }
            else
            {
                foreach (var eString in tStringArray)
                {
                    tStringSubArray = eString.Split('=');
                    if (tStringSubArray.Length > 1)
                    {
                        string[] tString = tStringSubArray[1].Split(';');
                        foreach (Instrument tInstrument in iInstruments)
                        {
                            if (tInstrument.Name == tStringSubArray[0].Replace("S_Instr_", ""))
                            {
                                ReturnString += tStringSubArray[0] + "=" + tInstrument.Address + ",";
                            }
                        }
                    }
                }
            }
            return ReturnString.TrimEnd(',');
        }


        //Save Instruments Config
        public void XmlSaveInstrumentsConfigFile(Instruments iInstruments)
        {
            string iFileName = iConfigWindow.TestPlanPath +SelectedTestPlan+ "_InstrumentsSetting.xml";
            if (File.Exists(iFileName))
            {
                File.Delete(iFileName);
            }

            if (iInstruments == null) return;

            #region Write

            using (FileStream fileSteam = File.Create(iFileName))
            {
                XmlWriterSettings iXmlWriterSettings = new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 };

                using (XmlWriter iXmlWriter = XmlWriter.Create(fileSteam, iXmlWriterSettings))
                {
                    iXmlWriter.WriteStartDocument();
                    iXmlWriter.WriteStartElement("Instruments");
                    foreach (Instrument iInstrument in iInstruments)
                    {
                        if (iInstrument.Name != "" && iInstrument.ConnectType != "" && iInstrument.Address != "")
                        {
                            iXmlWriter.WriteStartElement("Instrument");
                            foreach (var iUserElement in iInstrument.GetType().GetProperties())
                            {
                                object value = iUserElement.GetValue(iInstrument, new object[] { });
                                iXmlWriter.WriteElementString(iUserElement.Name, value.ToString());
                            }
                            iXmlWriter.WriteEndElement();
                        }
                    }
                    iXmlWriter.WriteEndDocument();
                }
            }

            #endregion
        }

        //Read Instruments Config
        public void XmlReadInstrumentsConfigFile()
        {
            string iFileName = iConfigWindow.TestPlanPath + SelectedTestPlan + "_InstrumentsSetting.xml";

            if (!File.Exists(iFileName))
            {
                IsInstrConfigFileExits = false;
                toolStripMenuItem_Instrument.Visible = false;
                Application.DoEvents();
                return ;
            }
            iInstruments.Clear();

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
                        if (iXmlReader.Name == "Instrument" && iXmlReader.NodeType == XmlNodeType.Element)
                        {

                            #region read to dictionary

                            tDic.Clear();
                            iList.Clear();
                            do
                            {
                                iString = iConfigWindow.XmlReadString(iXmlReader);
                                if (iXmlReader.Name == "Instrument" && iXmlReader.NodeType == XmlNodeType.EndElement)
                                {
                                    break;
                                }
                                else
                                {
                                    tDic.Add(iXmlReader.Name, iString);
                                }
                            } while (iXmlReader.Name != "Instrument");

                            #endregion

                            #region 根据变量名称对应赋值

                            Instrument iInstrument = new Instrument();
                            foreach (var iInstrumentElement in iInstrument.GetType().GetProperties())
                            {
                                iString = "";
                                foreach (var iDic in tDic)
                                {
                                    if (iDic.Key == iInstrumentElement.Name)
                                    {
                                        iString = iDic.Value;
                                        break;
                                    }
                                }
                                iInstrumentElement.SetValue(iInstrument, iString, new object[] { });
                                
                            }

                            #endregion

                            if (iInstrument.Name != "" && iInstrument.ConnectType != "" && iInstrument.Address != "")
                            {
                                iInstruments.Add(iInstrument);
                            }
                        }

                    }
                }
            }

            #endregion

            IsInstrConfigFileExits = true;
        }

        //deal with exception
        public void DealwithException(Exception ex)
        {
            string iFileName = Application.StartupPath + "\\Log\\ApplicaitonExceptionLog - " + DateTime.Now.ToString().Replace("/", "-").Replace(":", "-") +".xml";
            if (!Directory.Exists(Application.StartupPath + "\\Log"))
            { 
              Directory.CreateDirectory(Application.StartupPath + "\\Log");
            }

            using (FileStream fileSteam = File.Create(iFileName))
            {
                XmlWriterSettings iXmlWriterSettings = new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 };

                using (XmlWriter iXmlWriter = XmlWriter.Create(fileSteam, iXmlWriterSettings))
                {
                    iXmlWriter.WriteStartDocument();

                    iXmlWriter.WriteStartElement("Exception");

                    iXmlWriter.WriteElementString("Log", ex.ToString());

                    iXmlWriter.WriteEndElement();

                    iXmlWriter.WriteEndDocument();

                }
            }

        }

        //get customer varient
        public object GetCustomerVariant(string cvName)
        {
            foreach (CustomerVarient iCV in iCVs)
            {
                if (iCV.Name == cvName)
                {
                    return iCV.Value;
                }
            }
            return null;
        }

        public void SaveProjectInformation(StreamWriter iSW)
        {
            foreach (CustomerVarient iCV in iCVs)
            {
                if (iCV.Name.Contains("S_PInfo_"))
                {
                    iSW.WriteLine(iCV.Name.Replace("S_PInfo_","") + ":  " + iCV.Value.ToString());
                }
            }
        }

        private void FontAdaptText(Control iControl, string iText)
        {
            using (Graphics g = CreateGraphics())
            {
                SizeF iSize;
                bool IsOK = false;
                do
                {
                    iSize = g.MeasureString(iText, iControl.Font, 500);
                    if (iSize.Width + 10 >= iControl.Width)
                    {
                        iControl.Font = new Font(iControl.Font.FontFamily, iControl.Font.Size - 0.5f);
                    }
                    else
                    {
                        IsOK = true;
                    }
                } while (IsOK == false);

            }
            iControl.Text = iText;
        }

        #endregion

  

       

       

       

    

       

       



       

 

 

     

     

     

   





    }

   
}

