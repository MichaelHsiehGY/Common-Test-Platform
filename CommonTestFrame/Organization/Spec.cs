using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Organization
{
    [TypeConverter(typeof(specConverter))]
    public class Spec
    {
        public Spec()
        {
        }

        private int _index = -1;
        public string driver = "";
        public string testItem = "";
        public string _class = "";
        //string measureIndex = "";
        string lowLimit = "";
        string upLimit = "";
        string unit = "";
        string measureName = "";
        string measureValue = "";
        string measureResult = "";
        string remark = "";
        string specNumber = "";
        string specName = "";
        string testTime = "";


        public string Driver
        {
            get { return driver; }
            //set { testItem = value; }
        }

        [BrowsableAttribute(false)]
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        public string Class
        {
            get { return _class; }
            //set { testItem = value; }
        }

        public string TestItem
        {
            get { return testItem; }
            //set { testItem = value; }
        }


        public string LowLimit
        {
            get { return lowLimit; }
            set { lowLimit = value; }
        }

        public string UpLimit
        {
            get { return upLimit; }
            set { upLimit = value; }
        }

        public string Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        public string MeasureName
        {
            get { return measureName; }
            set { measureName = value; }
        }
        [BrowsableAttribute(false)]
        public string MeasureValue
        {
            get { return measureValue; }
            set { measureValue = value; }
        }

        //pass or fail
         [BrowsableAttribute(false)]
        public string MeasureResult
        {
            get { return measureResult; }
            set { measureResult = value; }
        }

         [BrowsableAttribute(false)]
         public string Remark
         {
             get { return remark; }
             set { remark = value; }
         }

        public string SpecNumber
        {
            get { return specNumber; }
            set { specNumber = value; }
        }

        public string SpecName
        {
            get { return specName; }
            set { specName = value; }
        }
        [BrowsableAttribute(false)]
        public string TestTime
        {
            get { return testTime; }
            set { testTime = value; }
        }

        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();
           
            sb.Append(this.MeasureName);
           
            return sb.ToString();
        }
    }
    
}
