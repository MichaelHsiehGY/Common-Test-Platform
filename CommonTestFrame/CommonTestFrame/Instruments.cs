using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CommonTestFrame
{
    public class Instruments : CollectionBase
    {
        public void Add(Instrument emp)
        {
            this.List.Add(emp);
        }

        public void Remove(Instrument emp)
        {
            this.List.Remove(emp);
        }

        public Instrument this[int index]
        {
            get
            {
                return (Instrument)this.List[index];
            }
        }
    }

    public class Instrument
    {
        private string _name = "";
        private string _connectType = "";
        private string _address = "";
       

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string ConnectType
        {
            get { return _connectType; }
            set { _connectType = value; }
        }

        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

    }
}
