using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTestFrame
{
    class FailInformation
    {

        public FailInformation()
        {
        }

        int _fail_Index = -1;
        string _fail_FunctionName = "";
        string _fail_Message = "";

        public int Fail_Index
        {
            get {return _fail_Index;}
            set { _fail_Index = value; }
        }

        public string Fail_FunctionName
        {
            get { return _fail_FunctionName; }
            set { _fail_FunctionName = value; }
        }

        public string Fail_Message
        {
            get { return _fail_Message; }
            set { _fail_Message = value; }
        }
    }
}
