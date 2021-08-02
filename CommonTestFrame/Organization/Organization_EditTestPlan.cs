using System;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace Organization
{
	public class Organization_EditTestPlan
	{
        public DictionaryPropertyGridAdapter _Paras = new DictionaryPropertyGridAdapter();
    

		public Organization_EditTestPlan()
		{	
		}

        [Editor(typeof(MyCollectionEditor_NO), typeof(UITypeEditor))]
        [TypeConverter(typeof(paraCollectionConverter))]
        [Category("Conditions")]
        public DictionaryPropertyGridAdapter Parameters
        {
            get { return _Paras; }
        }

	}

    public class MyCollectionEditor_NO: CollectionEditor
    {

        public MyCollectionEditor_NO(Type type): base(type)
        {

        }
     

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            return base.EditValue(context, provider, value);

        }

    }
}
