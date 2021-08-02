using System;
using System.Text;
using System.ComponentModel;

namespace Organization
{
	/// <summary>
	/// Summary description for CollectionPropertyDescriptor.
	/// </summary>
	public class SpecCollectionPropertyDescriptor : PropertyDescriptor
	{
        private SpecCollection collection = null;
		private int index = -1;

        public SpecCollectionPropertyDescriptor(SpecCollection coll, int idx) : 
			base( "#"+idx.ToString(), null )
		{
			this.collection = coll;
			this.index = idx;
		} 

		public override AttributeCollection Attributes
		{
			get 
			{ 
				return new AttributeCollection(null);
			}
		}

		public override bool CanResetValue(object component)
		{
			return true;
		}

		public override Type ComponentType
		{
			get 
			{ 
				return this.collection.GetType();
			}
		}

		public override string DisplayName
		{
			get 
			{
                if (index < 0 || index >= this.collection.Count)
                {
                    return "";
                }
                Spec emp = this.collection[index];
                return emp.MeasureName;
			}
		}

   

		public override string Description
		{
			get
			{
                return this.collection[index].TestItem + " - " + this.collection[index].MeasureName+
                    ":\nRange: "+this.collection[index].LowLimit+" ~ "+this.collection[index].UpLimit+"\n"
                    + "Unit: " + this.collection[index].Unit+"\n"
                    + "Spec Number: " + this.collection[index].SpecNumber + "\nSpec Name: " + this.collection[index].SpecName;
			}
		}

		public override object GetValue(object component)
		{
			return this.collection[index];
		}

		public override bool IsReadOnly
		{
			get { return false;  }
		}

		public override string Name
		{
			get { return "#"+index.ToString(); }
		}

		public override Type PropertyType
		{
			get {return this.collection[index].GetType(); }
		}

		public override void ResetValue(object component)
		{
		}

		public override bool ShouldSerializeValue(object component)
		{
			return true;
		}

		public override void SetValue(object component, object value)
		{
			// this.collection[index] = value;
		}
	}
}
