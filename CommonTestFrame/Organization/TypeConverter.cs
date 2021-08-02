using System;
using System.ComponentModel;

namespace Organization
{
	// This is a special type converter which will be associated with the para class.
	// It converts an para object to string representation for use in a property grid.
	internal class paraConverter : ExpandableObjectConverter
	{
		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType )
		{
			if( destType == typeof(string) && value is Para )
			{
				// Cast the value to an para type
				Para emp = (Para)value;

				// Return department and department role separated by comma.
				return emp.Value;
			}
			return base.ConvertTo(context,culture,value,destType);
		}
	}

	// This is a special type converter which will be associated with the paraCollection class.
	// It converts an paraCollection object to a string representation for use in a property grid.
	internal class paraCollectionConverter : ExpandableObjectConverter
	{
		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType )
		{
			if( destType == typeof(string))
			{
				if(value is ParaCollection)
                {
				    return "Click here to edit Parameter's Value";
                }
                if (value is DictionaryPropertyGridAdapter)
                {
                    return "";
                }
			}
			return base.ConvertTo(context,culture,value,destType);
		}
	}

    internal class specConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType)
        {
            if (destType == typeof(string) && value is Spec)
            {
                // Cast the value to an para type
                Spec emp = (Spec)value;

                // Return department and department role separated by comma.
                return emp.MeasureName;
            }
            return base.ConvertTo(context, culture, value, destType);
        }
    }

    internal class specCollectionConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType)
        {
            if (destType == typeof(string))
            {
                if (value is SpecCollection)
                {
                    return "Click here to edit Parameter's Value";
                }
                if (value is DictionaryPropertyGridAdapter)
                {
                    return "";
                }
            }
            return base.ConvertTo(context, culture, value, destType);
        }
    }

}
