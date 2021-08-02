using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;


namespace CommonTestFrame
{
    //[TypeConverter(typeof(specConverter))]
    public class Config
    {
        public Config()
        {
        }

        string _RootPath = "";
        bool _IsStopWhenFailed = true;
        bool _IsSaveToCSV = true;
        bool _IsSaveToTxt = true;
        //SN
        bool _IsAllowManualInput_SN = false;
        string _characterCaseType_SN = "Normal";
        //window
        bool _IsShowFunctionRow = false;
        bool _IsShowTestItemRow = false;
        bool _IsShowInfoBorad = false;
        bool _IsShowStatusBar = false;
        //Loop Test
        bool _IsLoopTest = false;
    

        public string RootPath
        {
            get { return _RootPath; }
            set { _RootPath = value; }
        }


        public bool IsStopWhenFailed
        {
            get { return _IsStopWhenFailed; }
            set { _IsStopWhenFailed = value; }
        }

        public bool IsSaveToCSV
        {
            get { return _IsSaveToCSV; }
            set { _IsSaveToCSV = value; }
        }


        public bool IsSaveToTxt
        {
            get { return _IsSaveToTxt; }
            set { _IsSaveToTxt = value; }
        }

        public bool IsAllowManualInput_SN
        {
            get { return _IsAllowManualInput_SN; }
            set { _IsAllowManualInput_SN = value; }
        }

        public string CharacterCaseType_SN
        {
            get { return _characterCaseType_SN; }
            set { _characterCaseType_SN = value; }
        }

        public bool IsShowFunctionRow
        {
            get { return _IsShowFunctionRow; }
            set { _IsShowFunctionRow = value; }
        }

        public bool IsShowTestItemRow
        {
            get { return _IsShowTestItemRow; }
            set { _IsShowTestItemRow = value; }
        }

     
        public bool IsShowInfoBorad
        {
            get { return _IsShowInfoBorad; }
            set { _IsShowInfoBorad = value; }
        }

        public bool IsShowStatusBar
        {
            get { return _IsShowStatusBar; }
            set { _IsShowStatusBar = value; }
        }

        public bool IsLoopTest
        {
            get { return _IsLoopTest; }
            set { _IsLoopTest = value; }
        }

    }
    
}
