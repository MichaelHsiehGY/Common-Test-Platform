using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;


namespace Organization
{
	/// <summary>
	/// Summary description for Organization.
	/// </summary>
	public class Organization_EditPara
	{
		ParaCollection Paras = new ParaCollection();
        public PropertyGrid m_PropertyGrid;

		public Organization_EditPara()
		{	
			

		}

        [Editor(typeof(MyCollectionEditor), typeof(UITypeEditor))]
		[TypeConverter(typeof(paraCollectionConverter))]
		public ParaCollection Parameters
		{
            get { return Paras; }
		}


        public void SaveCollection()
        {
            MessageBox.Show("changes happend!");
        }
	}

    public class MyCollectionEditor : CollectionEditor
    {
        Organization_EditPara m_Organization_Edit;
        CollectionForm collectionForm;
        public MyCollectionEditor(Type type): base(type)
        { 

        }


        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            m_Organization_Edit = (Organization_EditPara)context.Instance;
            return base.EditValue(context, provider, value);
            //Code following this line has a guarantee that the collection editor has already
            //been dismissed, and changes to the collection have been applied.
            //context.Instance is the object that holds the property just edited.
            //In the case of the CodeProject example, it is of type Organization.
            //value is the collection itself.
            //Do whatever you need to do to save the collection here.  Either by a method
            //provided by the object owner of the collection, or the collection itself.
            //Example:  Let's imagine that there is a SaveCollection() method in the object
            //holding the collection.
            //((Organization)context.Instance).SaveCollection();
        }

        protected override CollectionForm CreateCollectionForm()
        {
            collectionForm = base.CreateCollectionForm();
            Form frm = collectionForm as Form;
            if (frm != null)
            {
                // Get OK button of the Collection Editor...
                Button button = frm.AcceptButton as Button;
                // Handle click event of the button
                button.Click += new EventHandler(OKButton_Click);
            }
           

            return collectionForm;
        }

        void OKButton_Click(object sender, EventArgs e)
        {
            m_Organization_Edit.m_PropertyGrid.Refresh();
        }

      

       
    }
}
