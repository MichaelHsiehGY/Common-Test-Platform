using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using System.IO;
using System.Xml;
using Organization;


namespace CommonTestFrame
{
    public partial class Parameter_set : Form
    {
        String[] paraName=new String[50];
        String[] paraValue=new String[50];
        String parasChanged="";
        int Count = 0;
       

        public Parameter_set()
        {  
            InitializeComponent();
        }


        public void loadPropertyGridDVG1(Organization_EditSpec d)
        {
            this.TopMost = true;
            propertyGrid1.Show();
            propertyGrid1.SelectedObject = d;
            d.m_PropertyGrid = propertyGrid1;
            propertyGrid1.PropertySort = PropertySort.NoSort;
            propertyGrid1.Refresh();
            propertyGrid1.ExpandAllGridItems();
        }

        public void loadPropertyGridDVG1(Organization_EditSpec_HideEditor d)
        {
            this.TopMost = true;
            propertyGrid1.Show();
            propertyGrid1.SelectedObject = d;
            d.m_PropertyGrid = propertyGrid1;
            propertyGrid1.PropertySort = PropertySort.NoSort;
            propertyGrid1.Refresh();
            propertyGrid1.ExpandAllGridItems();
        }

        public void loadPropertyGridDVG1(Organization_EditPara d)
        {
            this.TopMost = true;
            propertyGrid1.Show();
            propertyGrid1.SelectedObject = d;
            d.m_PropertyGrid = propertyGrid1;
            propertyGrid1.PropertySort = PropertySort.NoSort;
            propertyGrid1.Refresh();
            propertyGrid1.ExpandAllGridItems();
        }

        public void loadPropertyGridDVG1(Organization_EditTestPlan d)
        {
            this.TopMost = true;
            propertyGrid1.Show();
            propertyGrid1.SelectedObject = d;
            propertyGrid1.Refresh();
            propertyGrid1.ExpandAllGridItems();
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (Form1.myForm1.ActiveUser.UserType == "Administrator")
            {
                String pName = e.ChangedItem.Label;
                String pValue = e.ChangedItem.Value.ToString();
                if (!parasChanged.Contains(pName))
                {
                    paraName[Count] = pName;
                    paraValue[Count] = pValue;
                    Count++;
                    parasChanged = parasChanged + "," + pName;
                }
                else
                {
                    for (int i = 0; i < Count; i++)
                    {
                        if (paraName[Count] == pName)
                        {
                            paraValue[Count] = pValue;
                        }
                    }

                }
            }

            if (Form1.myForm1.ActiveUser.UserType == "Programmer")
            {
                String pName = e.ChangedItem.Parent.Label;
                String pValue = e.ChangedItem.Value.ToString();
                if (!parasChanged.Contains(pName))
                {
                    paraName[Count] = pName;
                    paraValue[Count] = pValue;
                    Count++;
                    parasChanged = parasChanged + "," + pName;
                }
                else
                {
                    for (int i = 0; i < Count; i++)
                    {
                        if (paraName[Count] == pName)
                        {
                            paraValue[Count] = pValue;
                        }
                    }

                }
            }

 

        }

        public void CWupdateDGV()
        {
            int rowIndex = ConfigWindow.myCWForm.rowIndex;
            if (ConfigWindow.myCWForm.columIndex == ConfigWindow.myCWForm.dataGridView1.Rows[rowIndex].Cells["Para"].ColumnIndex)
            {
                if (Form1.myForm1.ActiveUser.UserType == "Programmer")
                {
                    Organization_EditPara current_Organization = (Organization_EditPara)this.propertyGrid1.SelectedObject;
                    SaveParameterEdit(current_Organization, ConfigWindow.myCWForm.dataGridView1);
                    return;
                }  
                if (Form1.myForm1.ActiveUser.UserType == "Administrator")
                {
                    Organization_EditTestPlan current_Organization = (Organization_EditTestPlan)this.propertyGrid1.SelectedObject;
                    SaveParameterEdit(current_Organization, ConfigWindow.myCWForm.dataGridView1);
                    return;
                }
            }
            if (ConfigWindow.myCWForm.columIndex == ConfigWindow.myCWForm.dataGridView1.Rows[rowIndex].Cells["Measure"].ColumnIndex)
            {
                if (Form1.myForm1.ActiveUser.UserType == "Programmer")
                {
                    Organization_EditSpec current_Organization = (Organization_EditSpec)this.propertyGrid1.SelectedObject;
                    SaveSpecEdit(current_Organization, ConfigWindow.myCWForm.dataGridView1);
                    return;
                }
                if (Form1.myForm1.ActiveUser.UserType == "Administrator")
                {
                    Organization_EditSpec_HideEditor current_Organization = (Organization_EditSpec_HideEditor)this.propertyGrid1.SelectedObject;
                    SaveSpecEdit(current_Organization, ConfigWindow.myCWForm.dataGridView1);
                    return;
                }
            }
        }

        public void SaveSpecEdit(Organization_EditSpec current_Organization_EditSpec, DataGridView iDataGridView)
        {
            int rowIndex = ConfigWindow.myCWForm.rowIndex;
            int columIndex = ConfigWindow.myCWForm.columIndex;
    
            string MeasureNames = "";
            string iFileName = "";
            string keyString = iDataGridView.Rows[rowIndex].Cells["Driver"].Value.ToString() + "-" + iDataGridView.Rows[rowIndex].Cells["Class"].Value.ToString() + "-" + iDataGridView.Rows[rowIndex].Cells["TestItem"].Value.ToString() + "-";
            string[] SpecFileNames = iDataGridView.Rows[rowIndex].Cells["SpecFile"].Value.ToString().Split(',');
            string newFileNames = "";

            //delete old description files
            foreach (string iSpecFileName in SpecFileNames)
            {
                iFileName = ConfigWindow.myCWForm.SpecPath + iSpecFileName + ".xml";
                if (File.Exists(iFileName))
                {
                    File.Delete(iFileName);
                }
            }

            foreach (Spec iSpec in current_Organization_EditSpec.Parameters)
            {
                if (iSpec.MeasureName == "") continue;
                iFileName = "";
                foreach (string iSpecFileName in SpecFileNames)
                {
                    if (ConfigWindow.myCWForm.GetExactParaOrSpecName(keyString,iSpecFileName) == iSpec.MeasureName)
                    {
                        newFileNames = newFileNames + "," + iSpecFileName;
                        iFileName = ConfigWindow.myCWForm.SpecPath + iSpecFileName + ".xml";
                        break;
                    }
                }

                if (iFileName == "")
                {
                    string iInitFileName = keyString + iSpec.MeasureName;
                    iInitFileName = ConfigWindow.myCWForm.XmlCreateParaOrSpecDescriptionFile(iInitFileName, "Spec");
                    newFileNames = newFileNames + "," + iInitFileName;
                    iFileName =ConfigWindow.myCWForm.SpecPath+ iInitFileName+".xml";  
                }

                #region Write
                //if (iFileName == "") return;
                using (FileStream fileSteam = File.Create(iFileName))
                {
                    XmlWriterSettings iXmlWriterSettings = new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 };

                    using (XmlWriter iXmlWriter = XmlWriter.Create(fileSteam, iXmlWriterSettings))
                    {
                        iXmlWriter.WriteStartDocument();
                        iXmlWriter.WriteStartElement("Spec");
                        string mmm = iSpec.MeasureName.Trim().Replace(" ", "_");
                        iXmlWriter.WriteStartElement(iSpec.MeasureName.Trim().Replace(" ", "_"));

                        //iXmlWriter.WriteElementString("Index", iSpec.Index.ToString());
                        //iXmlWriter.WriteElementString("Driver", iSpec.Driver);
                        //iXmlWriter.WriteElementString("TestItem", iSpec.TestItem);
                        iXmlWriter.WriteElementString("LowLimit", iSpec.LowLimit.Trim().Replace(" ","_"));
                        iXmlWriter.WriteElementString("UpLimit", iSpec.UpLimit.Trim().Replace(" ", "_"));
                        iXmlWriter.WriteElementString("Unit", iSpec.Unit.Trim().Replace(" ", "_"));
                        //iXmlWriter.WriteElementString("MeasureName", iSpec.MeasureName);
                        iXmlWriter.WriteElementString("SpecNumber", iSpec.SpecNumber.Trim().Replace(" ", "_"));
                        iXmlWriter.WriteElementString("SpecName", iSpec.SpecName.Trim().Replace(" ", "_"));

                        iXmlWriter.WriteEndElement();
                         
                        iXmlWriter.WriteEndDocument();
                    }
                }

                #endregion

                MeasureNames += iSpec.MeasureName + ",";
            }

            iDataGridView.Rows[rowIndex].Cells["Measure"].Value = MeasureNames.Trim(',');
            iDataGridView.Rows[rowIndex].Cells["SpecFile"].Value = newFileNames.Trim(',');
            //iDataGridView.Refresh();
        }

        public void SaveSpecEdit(Organization_EditSpec_HideEditor current_Organization_EditSpec, DataGridView iDataGridView)
        {
            int rowIndex = ConfigWindow.myCWForm.rowIndex;
            int columIndex = ConfigWindow.myCWForm.columIndex;
  

            string MeasureNames = "";
            string iFileName = "";
            string keyString = iDataGridView.Rows[rowIndex].Cells["Driver"].Value.ToString() + "-" + iDataGridView.Rows[rowIndex].Cells["Class"].Value.ToString() + "-" + iDataGridView.Rows[rowIndex].Cells["TestItem"].Value.ToString() + "-";
            string[] SpecFileNames = ConfigWindow.myCWForm.dataGridView1.Rows[rowIndex].Cells["SpecFile"].Value.ToString().Split(',');
            string newFileNames = "";

            //delete old description files
            foreach (string iSpecFileName in SpecFileNames)
            {
                iFileName = ConfigWindow.myCWForm.SpecPath + iSpecFileName + ".xml";
                if (File.Exists(iFileName))
                {
                    File.Delete(iFileName);
                }
            }

            foreach (Spec iSpec in current_Organization_EditSpec.Parameters)
            {
                if (iSpec.MeasureName == "") continue;
                iFileName = "";
                foreach (string iSpecFileName in SpecFileNames)
                {
                    if (ConfigWindow.myCWForm.GetExactParaOrSpecName(keyString,iSpecFileName) == iSpec.MeasureName)
                    {
                        newFileNames = newFileNames + "," + iSpecFileName;
                        iFileName = ConfigWindow.myCWForm.SpecPath + iSpecFileName + ".xml";
                        break;
                    }
                }

                if (iFileName == "")
                {
                    string iInitFileName = keyString + iSpec.MeasureName;
                    iInitFileName = ConfigWindow.myCWForm.XmlCreateParaOrSpecDescriptionFile(iInitFileName, "Spec");
                    newFileNames = newFileNames + "," + iInitFileName;
                    iFileName = ConfigWindow.myCWForm.SpecPath +iInitFileName + ".xml";
                }

                #region Write
                using (FileStream fileSteam = File.Create(iFileName))
                {
                    XmlWriterSettings iXmlWriterSettings = new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 };

                    using (XmlWriter iXmlWriter = XmlWriter.Create(fileSteam, iXmlWriterSettings))
                    {
                        iXmlWriter.WriteStartDocument();
                        iXmlWriter.WriteStartElement("Spec");
                        string mmm = iSpec.MeasureName.Trim().Replace(" ", "_");
                        iXmlWriter.WriteStartElement(iSpec.MeasureName.Trim().Replace(" ", "_"));

                        //iXmlWriter.WriteElementString("Index", iSpec.Index.ToString());
                        //iXmlWriter.WriteElementString("Driver", iSpec.Driver);
                        //iXmlWriter.WriteElementString("TestItem", iSpec.TestItem);
                        iXmlWriter.WriteElementString("LowLimit", iSpec.LowLimit.Trim().Replace(" ", "_"));
                        iXmlWriter.WriteElementString("UpLimit", iSpec.UpLimit.Trim().Replace(" ", "_"));
                        iXmlWriter.WriteElementString("Unit", iSpec.Unit.Trim().Replace(" ", "_"));
                        //iXmlWriter.WriteElementString("MeasureName", iSpec.MeasureName);
                        iXmlWriter.WriteElementString("SpecNumber", iSpec.SpecNumber.Trim().Replace(" ", "_"));
                        iXmlWriter.WriteElementString("SpecName", iSpec.SpecName.Trim().Replace(" ", "_"));

                        iXmlWriter.WriteEndElement();

                        iXmlWriter.WriteEndDocument();
                    }
                }

                #endregion

                MeasureNames += iSpec.MeasureName + ",";
            }

            iDataGridView.Rows[rowIndex].Cells["Measure"].Value = MeasureNames.Trim(',');
            iDataGridView.Rows[rowIndex].Cells["SpecFile"].Value = newFileNames.Trim(',');
            //iDataGridView.Refresh();
        }

        private void SaveParameterEdit(Organization_EditTestPlan current_Organization, DataGridView iDataGridView)
        {
            if (current_Organization.Parameters.Count > 0)
            {
                int rowIndex = ConfigWindow.myCWForm.rowIndex;
                int columIndex = ConfigWindow.myCWForm.columIndex;
                string Paras = "";
                string[] ParaFileNames = iDataGridView.Rows[rowIndex].Cells["ParaDescriptionFile"].Value.ToString().Split(',');

                foreach (Para iPara in current_Organization.Parameters.list)
                {
                     Paras = Paras + iPara.Name + "=" + iPara.Value + ",";      
                 }
                iDataGridView.Rows[rowIndex].Cells["Para"].Value = Paras.TrimEnd(',');
            }
        }

        private void SaveParameterEdit(Organization_EditPara current_Organization, DataGridView iDataGridView)
        {
            int rowIndex = ConfigWindow.myCWForm.rowIndex;
            int columIndex = ConfigWindow.myCWForm.columIndex;
 
            string Paras = "";
            string iFileName = "";
            string keyString = iDataGridView.Rows[rowIndex].Cells["Driver"].Value.ToString() + "-" + iDataGridView.Rows[rowIndex].Cells["Class"].Value.ToString() + "-" + iDataGridView.Rows[rowIndex].Cells["TestItem"].Value.ToString() + "-";
            string[] ParaFileNames = iDataGridView.Rows[rowIndex].Cells["ParaDescriptionFile"].Value.ToString().Split(',');
            string newfileNames = "";
            //delete old description files
            foreach (string iParaFileName in ParaFileNames)
            {
                iFileName = ConfigWindow.myCWForm.DescriptionPath + iParaFileName + ".html";
                if (File.Exists(iFileName))
                {
                    File.Delete(iFileName);
                }
            }
                
            foreach (Para iPara in current_Organization.Parameters)
            {
                if (iPara.Description != "")
                {
                    iFileName = "";
                    foreach (string iParaFileName in ParaFileNames)
                    {
                        if (ConfigWindow.myCWForm.GetExactParaOrSpecName(keyString, iParaFileName) == iPara.Name)
                        {
                            newfileNames = newfileNames + "," + iParaFileName;
                            iFileName = ConfigWindow.myCWForm.DescriptionPath + iParaFileName + ".html";
                            break;
                        }
                    }
                    if (iFileName == "")
                    {
                        string iInitFileName = keyString + iPara.Name;
                        iInitFileName = ConfigWindow.myCWForm.XmlCreateParaOrSpecDescriptionFile(iInitFileName, "Description");
                        newfileNames = newfileNames + "," + iInitFileName;
                        iFileName = ConfigWindow.myCWForm.DescriptionPath + iInitFileName + ".html";
                        
                    }


                    FileStream m_FS = new FileStream(iFileName, FileMode.OpenOrCreate);
                    StreamWriter m_SW = new StreamWriter(m_FS);
                    m_SW.Write(iPara.Description);
                    m_SW.Close();
                    m_FS.Close();
                }

                Paras = Paras + iPara.Name + "=" + iPara.Value + ",";  
                    
            }
            iDataGridView.Rows[rowIndex].Cells["Para"].Value = Paras.TrimEnd(',');
            iDataGridView.Rows[rowIndex].Cells["ParaDescriptionFile"].Value = newfileNames.Trim(',');
        }

        private void Parameter_set_FormClosed(object sender, FormClosedEventArgs e)
        {
            CWupdateDGV();
            propertyGrid1.SelectedObject = null;


            this.TopMost = false;
        }

     



    

       
   

     


         




    }
}