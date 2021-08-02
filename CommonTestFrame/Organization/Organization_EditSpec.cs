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
	public class Organization_EditSpec
	{
        SpecCollection Specs = new SpecCollection();
        public PropertyGrid m_PropertyGrid;
       
       

        public Organization_EditSpec()
		{	
			// Instantiate test data objects and fill para collection
		}


        [Editor(typeof(specCollectionEditor), typeof(UITypeEditor))]
		[TypeConverter(typeof(specCollectionConverter))]
        public SpecCollection Parameters
		{
            get { return Specs; }
		}


        public void SaveCollection()
        {
            MessageBox.Show("changes happend!");
        }

	}

    public class Organization_EditSpec_HideEditor
    {
        SpecCollection Specs = new SpecCollection();
        public PropertyGrid m_PropertyGrid;



        public Organization_EditSpec_HideEditor()
        {
            // Instantiate test data objects and fill para collection
        }


         [Editor(typeof(MyCollectionEditor_NO), typeof(UITypeEditor))]
        [TypeConverter(typeof(specCollectionConverter))]
        public SpecCollection Parameters
        {
            get { return Specs; }
        }


        public void SaveCollection()
        {
            MessageBox.Show("changes happend!");
        }

    }


    public class specCollectionEditor : CollectionEditor
    {
        Organization_EditSpec m_Organization_Edit;
        CollectionForm collectionForm;
        public specCollectionEditor(Type type): base(type)
        {

        }


        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            m_Organization_Edit = (Organization_EditSpec)context.Instance;
          
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

                if (collectionForm.Controls.Count > 0)
                {
                    TableLayoutPanel mainPanel = collectionForm.Controls[0] as TableLayoutPanel;
                    // Get a reference to the inner PropertyGrid and hook 
                    // an event handler to it.
                    PropertyGrid propertyGrid = mainPanel.Controls[5] as PropertyGrid;
                    if (propertyGrid != null)
                    {
                        propertyGrid.PropertySort = PropertySort.NoSort;
                    }

                    //// Also hook to the Add/Remove
                    //TableLayoutPanel buttonPanel = mainPanel.Controls[1] as TableLayoutPanel;
                    //if ((buttonPanel != null) && (buttonPanel.Controls.Count > 1))
                    //{
                    //    Button addButton = buttonPanel.Controls[0] as Button;
                    //    if (addButton != null)
                    //    {
                    //        addButton.Click += new EventHandler(addButton_Click);
                    //    }
                        //Button removeButton = buttonPanel.Controls[1] as Button;
                        //if (removeButton != null)
                        //{
                        //    //removeButton.Click +=new EventHandler(removeButton_Click);
                        //}
                    //}
                }
            }
           

            return collectionForm;
        }

  

       
 

        //void addButton_Click(object sender, EventArgs e)
        //{
           
        //}

        void OKButton_Click(object sender, EventArgs e)
        {
            m_Organization_Edit.m_PropertyGrid.Refresh();
        }


        protected override object SetItems(object editValue, object[] value)
        {
            base.SetItems(editValue, value);
           
                SpecCollection neweditValue = (SpecCollection)editValue;
                if (neweditValue.Count > 0)
                {
                    neweditValue[neweditValue.Count - 1].driver = neweditValue.driver;
                    neweditValue[neweditValue.Count - 1]._class = neweditValue._class;
                    neweditValue[neweditValue.Count - 1].testItem = neweditValue.testItem;
                    return neweditValue;
                }
                else
                {
                    return editValue;
                }
        }

        //protected override Type[] CreateNewItemTypes()
        //{  
        //    return base.CreateNewItemTypes();
        //}

    }

  

 
}
