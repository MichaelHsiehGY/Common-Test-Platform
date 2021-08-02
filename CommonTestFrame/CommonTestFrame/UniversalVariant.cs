using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTestFrame
{
    public class UniversalVariant
    {
        public UniversalVariant()
        { 
        
        }

        private string _sn = "";
        private string _testResultPath="";

         public string SN
        {
            get { return _sn; }
            set { _sn = value; }
        }

         public string TestResultPath
        {
            get { return _testResultPath; }
            set { _testResultPath = value; }
        }
    }
}
