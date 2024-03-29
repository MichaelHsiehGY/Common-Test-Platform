using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;


namespace Organization
{
	/// <summary>
	/// Type safe collection class for para objects. Extends the base class 
	/// CollectionBase to inherit base collection functionality.
	/// Implementation of ICustomTypeDescvriptor to provide customized type description.
	/// </summary>
    public class SpecCollection : CollectionBase, ICustomTypeDescriptor
	{
		#region collection impl


        public string testItem = "";
        public string driver = "";
        public string _class = "";

		/// <summary>
		/// Adds an para object to the collection
		/// </summary>
		/// <param name="emp"></param>
		public void Add( Spec emp )
		{
            emp.testItem = testItem;
            emp._class = _class;
            emp.driver = driver;
			this.List.Add( emp );
		}
		
		/// <summary>
		/// Removes an para object from the collection
		/// </summary>
		/// <param name="emp"></param>
        public void Remove(Spec emp)
		{
			this.List.Remove( emp );
		}




		
		/// <summary>
		/// Returns an para object at index position.
		/// </summary>
        public Spec this[int index] 
		{
			get
			{
                if (index < 0 || index>=this.List.Count)
                {
                    return null;
                }
                return (Spec)this.List[index];
			}
		}

		#endregion

		// Implementation of interface ICustomTypeDescriptor 
		#region ICustomTypeDescriptor impl 

		public String GetClassName()
		{
			return TypeDescriptor.GetClassName(this,true);
		}

		public AttributeCollection GetAttributes()
		{
			return TypeDescriptor.GetAttributes(this,true);
		}

		public String GetComponentName()
		{
			return TypeDescriptor.GetComponentName(this, true);
		}

		public TypeConverter GetConverter()
		{
			return TypeDescriptor.GetConverter(this, true);
		}

		public EventDescriptor GetDefaultEvent() 
		{
			return TypeDescriptor.GetDefaultEvent(this, true);
		}

		public PropertyDescriptor GetDefaultProperty() 
		{
			return TypeDescriptor.GetDefaultProperty(this, true);
		}

		public object GetEditor(Type editorBaseType) 
		{
			return TypeDescriptor.GetEditor(this, editorBaseType, true);
		}

		public EventDescriptorCollection GetEvents(Attribute[] attributes) 
		{
			return TypeDescriptor.GetEvents(this, attributes, true);
		}

		public EventDescriptorCollection GetEvents()
		{
			return TypeDescriptor.GetEvents(this, true);
		}

		public object GetPropertyOwner(PropertyDescriptor pd) 
		{
			return this;
		}


		/// <summary>
		/// Called to get the properties of this type. Returns properties with certain
		/// attributes. this restriction is not implemented here.
		/// </summary>
		/// <param name="attributes"></param>
		/// <returns></returns>
		public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			return GetProperties();
		}

		/// <summary>
		/// Called to get the properties of this type.
		/// </summary>
		/// <returns></returns>
		public PropertyDescriptorCollection GetProperties()
		{
			// Create a collection object to hold property descriptors
			PropertyDescriptorCollection pds = new PropertyDescriptorCollection(null);

			// Iterate the list of paras
			for( int i=0; i<this.List.Count; i++ )
			{
				// Create a property descriptor for the para item and add to the property descriptor collection
                SpecCollectionPropertyDescriptor pd = new SpecCollectionPropertyDescriptor(this, i);
                pds.Add(pd);
			}
			// return the property descriptor collection
			return pds;
		}

		#endregion
	}

   
}
