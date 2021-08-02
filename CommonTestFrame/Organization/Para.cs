using System;
using System.Text;
using System.ComponentModel;

namespace Organization
{
	/// <summary>
	/// para is our sample business or domin object. It derives from the general base class Person.
	/// </summary>
    ///   [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    //public class DisplayModeAttribute : Attribute
    //{
    //    private readonly string mode;
    //    public DisplayModeAttribute(string mode)
    //    {
    //        this.mode = mode ?? "";
    //    }

    //    public override bool Match(object obj)
    //    {
    //        var other = obj as DisplayModeAttribute;
    //        if (other == null) return false;

    //        if (other.mode == mode) return true;

    //        // allow for a comma-separated match, in either direction
    //        if (mode.IndexOf(',') >= 0)
    //        {
    //            string[] tokens = mode.Split(',');
    //            if (Array.IndexOf(tokens, other.mode) >= 0) return true;
    //        }
    //        else if (other.mode.IndexOf(',') >= 0)
    //        {
    //            string[] tokens = other.mode.Split(',');
    //            if (Array.IndexOf(tokens, mode) >= 0) return true;
    //        }
    //        return false;
    //    }
    //}


	[TypeConverter(typeof(paraConverter))]
	public class Para
	{
		private string _value = "";
		private string _name = "";
        private string _description = "";

        private string _index = "";
        private string _driver = "";
        private string _class = "";
        private string _testItem = "";
       
  

		public Para()
		{
		}

        [BrowsableAttribute(false)]
        public string Index
        {
            get { return _index; }
            set { _index = value; }
        }

        [BrowsableAttribute(false)]
        public string Driver
        {
            get { return _driver; }
            set { _driver = value; }
        }

        [BrowsableAttribute(false)]
        public string Class
        {
            get { return _class; }
            set { _class = value; }
        }

        [BrowsableAttribute(false)]
        public string TestItem
        {
            get { return _testItem; }
            set { _testItem = value; }
        }
  
        public string Name
        {
            get { return _name; }
            set{ _name = value; }
         
        }

		public string Value
		{
            get { return _value; }
            set {_value = value;}
		}

        public string Description
        {
            get { return _description; }
            set {_description = value;}
        }









		// Meaningful text representation
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
            //sb.Append(",");
            //sb.Append(this.Value);
            //sb.Append(",");
            sb.Append(this.Name);
			return sb.ToString();
		}
	}


  


}
