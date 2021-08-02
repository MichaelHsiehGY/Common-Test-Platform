using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace CommonTestFrame
{
    public partial class InstrumentsSetup : Form
    {
        TextBox tbTemp = new TextBox();
        int rowIndex = -1;
        int columnIndex = -1;

        public InstrumentsSetup()
        {
            InitializeComponent();
        }

        public void LoadInstruments(Instruments iInstruments)
        {
            dataGridView1.Rows.Clear();
            int i=0;
            foreach (Instrument iInstrument in iInstruments)
            {
                DataGridViewRow newDataGridViewRow = new DataGridViewRow();
                dataGridView1.Rows.Add(newDataGridViewRow);
                dataGridView1.Rows[i].Cells["InstrumentName"].Value = iInstrument.Name;
                dataGridView1.Rows[i].Cells["ConnectType"].Value = iInstrument.ConnectType;
                dataGridView1.Rows[i].Cells["Address"].Value = iInstrument.Address;
            }
        }

        private void InstrumentsSetup_FormClosed(object sender, FormClosedEventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                foreach (Instrument iInstrument in Form1.myForm1.iInstruments)
                {
                    if (iInstrument.Name == dataGridView1.Rows[i].Cells["InstrumentName"].Value.ToString())
                    {
                        iInstrument.ConnectType = dataGridView1.Rows[i].Cells["ConnectType"].Value.ToString();
                        iInstrument.Address = dataGridView1.Rows[i].Cells["Address"].Value.ToString();
                    }
                }
            }
            Form1.myForm1.XmlSaveInstrumentsConfigFile(Form1.myForm1.iInstruments);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (tbTemp.Parent != null) dataGridView1.Controls.Remove(tbTemp);
            if (e.ColumnIndex.Equals(this.dataGridView1.Columns["Address"].Index))//判断单元格是否是"Address"列
            {
                columnIndex = e.ColumnIndex;
                rowIndex = e.RowIndex;
                tbTemp.Visible = true;
                tbTemp.Text = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                tbTemp.Width = this.dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Width;//获取单元格高并设置为tbTemp的宽
                tbTemp.Height = this.dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Height;//获取单元格高并设置为tbTemp的高
                tbTemp.LostFocus += new EventHandler(tbTemp_LostFocus);
                this.dataGridView1.Controls.Add(tbTemp);
                tbTemp.Focus();
                tbTemp.SelectAll();
                tbTemp.Location = new System.Drawing.Point(((this.dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Right) - (tbTemp.Width)), this.dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Y);//设置tbTemp显示位置
                return;
            }
        }

        void tbTemp_LostFocus(object sender, EventArgs e)
        {
            tbTemp.LostFocus -= tbTemp_LostFocus;
            if (columnIndex == this.dataGridView1.Columns["Address"].Index)
            {
                dataGridView1.Rows[rowIndex].Cells[columnIndex].Value = tbTemp.Text.Trim();
                this.dataGridView1.Controls.Remove(tbTemp);
                this.dataGridView1.Refresh();
            }
        }

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if ((tbTemp.Parent != null))
            {
                if (e.RowIndex < 0 || e.RowIndex > dataGridView1.Rows.Count)
                {
                    tbTemp.LostFocus -= tbTemp_LostFocus;
                    if (columnIndex == this.dataGridView1.Columns["Address"].Index)
                    {
                        dataGridView1.Rows[rowIndex].Cells[columnIndex].Value = tbTemp.Text.Trim();
                        this.dataGridView1.Controls.Remove(tbTemp);
                        this.dataGridView1.Refresh();
                    }
                }
            }
        }

       

       

    }
}
