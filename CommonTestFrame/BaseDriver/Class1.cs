using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;


namespace BaseDriver
{
    public class General
    {

        public CustomerVarients Declare(string inputs)
        {
            string[] iArray_M = inputs.Split(';');
            string[] iArray_S = null;
            CustomerVarients iCVs = new CustomerVarients();
            foreach (string iString in iArray_M)
            {
               iArray_S = iString.Split(':');
               if (iArray_S.Length == 2)
               {
                   CustomerVarient iCV = new CustomerVarient();
                   if (iArray_S[0].Contains("S_"))
                   {
                       iCV.Name = iArray_S[0];
                       iCV.Value = Convert.ToString(iArray_S[1]);
                   }
                   iCVs.Add(iCV);
               }
            }
            return iCVs;
        }
    }

    public class CustomerVarients :CollectionBase
    {
        public void Add(CustomerVarient emp)
        {
            this.List.Add(emp);
        }

        public void Remove(CustomerVarient emp)
        {
            this.List.Remove(emp);
        }

        public CustomerVarient this[int index]
        {
            get
            {
                return (CustomerVarient)this.List[index];
            }
        }
    }

    public class CustomerVarient
    {
        private string _Name = "";
        private object _Value = "";
   
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public object Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
    }
}
