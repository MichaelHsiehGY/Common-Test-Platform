using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CommonTestFrame
{
    /// <summary>
    /// ���»����Ҽ��˵�
    /// </summary>    
    public class ContextMenuEx : System.Windows.Forms.MenuItem
    {
        /// <summary>
        /// �˵���������ʽ
        /// </summary>
        private Font menu_Font;

        /// <summary>
        /// �˵�����ߵ�������ʽ 
        /// </summary>
        private Font product_Font;

        /// <summary>
        /// �����˵��߿���ʽ
        /// </summary>
        private Pen menu_pen;

        /// <summary>
        /// �����˵��ϵı߿���ʽ
        /// </summary>
        private Pen menu_oldPen;

        /// <summary>
        /// ������ʼɫ
        /// </summary>
        private Color color_top;

        /// <summary>
        /// �����м�ɫtop
        /// </summary>
        private Color color_middle_top;

        /// <summary>
        /// �����м�ɫbottom
        /// </summary>
        private Color color_middle_bottom;

        /// <summary>
        /// ����ײ�ɫ
        /// </summary>
        private Color color_bottom;

        /// <summary>
        /// �����ˢ
        /// </summary>
        private LinearGradientBrush menu_linerBursh;

        private SolidBrush menu_LeftBackColor;

        public ContextMenuEx()
        {
            this.OwnerDraw = true;
            menu_Font = SystemInformation.MenuFont;
            product_Font = new Font("����", 13, FontStyle.Bold, GraphicsUnit.World);
            menu_pen = new Pen(Color.LightGray, 1.5f);
            menu_oldPen = new Pen(Color.White, 1.5f);  
            menu_LeftBackColor = new SolidBrush(Color.FromArgb(222, 227, 255));


            color_top = Color.FromArgb(234, 238, 249);
            color_middle_top = Color.FromArgb(222, 227, 255);
            color_middle_bottom = Color.FromArgb(195, 207, 246);
            color_bottom = Color.FromArgb(156, 177, 240);
        }
        //���Ǻ���Ҫ�ģ������Ĳ˵������˴�С��
        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            e.ItemHeight = SystemInformation.MenuHeight;
            e.ItemWidth = 150;

        }
        protected override void OnDrawItem(System.Windows.Forms.DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            //����ƶ���ȥ���״̬
            if (e.State == DrawItemState.NoAccelerator && this.Enabled)
            {
                //���������δ�ƶ����ò˵���ȥ
                g.FillRectangle(Brushes.White, e.Bounds);
                g.DrawRectangle(menu_oldPen, e.Bounds);
            }

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected && this.Enabled)
            {
                //����ƶ���ȥ�ˣ�����û�е���
                PointF topleft = new PointF(e.Bounds.X, e.Bounds.Y);
                PointF middleLeft = new PointF(e.Bounds.X, (e.Bounds.Height / 2 + e.Bounds.Y));
                PointF bottomLeft = new PointF(e.Bounds.X, e.Bounds.Height + e.Bounds.Y);

                menu_linerBursh = new LinearGradientBrush(topleft, middleLeft, color_middle_top, color_middle_top);
                g.FillRectangle(menu_linerBursh, e.Bounds.X + 27, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height / 2);
                menu_linerBursh.Dispose();

                menu_linerBursh = new LinearGradientBrush(middleLeft, bottomLeft, color_middle_bottom, color_bottom);
                g.FillRectangle(menu_linerBursh, e.Bounds.X + 27, (e.Bounds.Y + e.Bounds.Height / 2), e.Bounds.Width, e.Bounds.Height / 2);
                menu_linerBursh.Dispose();

                g.DrawRectangle(menu_pen, e.Bounds.X + 27, e.Bounds.Y, e.Bounds.Width - 26, e.Bounds.Height);
            }

            //��߻�ɫ����
            g.FillRectangle(menu_LeftBackColor, 0, 0, 26, e.Bounds.Height * this.Parent.MenuItems.Count);
            
     
            //g.DrawString("", product_Font, Brushes.Black, 4, 3);
            //g.DrawString("", product_Font, Brushes.Black, 4, e.Bounds.Height + 3);
            //g.DrawString("", product_Font, Brushes.Black, 4, e.Bounds.Height * 2 + 4);
            //g.DrawString("", product_Font, Brushes.Black, 4, e.Bounds.Height * 3 + 4);

            if (this.Enabled)
            {
                g.DrawString(this.Text, menu_Font, Brushes.Black, e.Bounds.X + 33, e.Bounds.Y + 1);
            }
            else
            {
                g.DrawString(this.Text, menu_Font, Brushes.Silver, e.Bounds.X + 33, e.Bounds.Y + 1);
            }
            g.Dispose();
        }

    }

    public class ContextMenuSplitLine : System.Windows.Forms.MenuItem
    {
        public ContextMenuSplitLine()
        {
            this.OwnerDraw = true;
            this.Enabled = false;//��������Ϊfalse�Ļ��û���������ʱ��ʹ�˵�������ʧ
        }
        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            e.ItemHeight = 3;
            e.ItemWidth = 150;
        }
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            int x_min = 0;
            int x_max = e.Bounds.Width;
            int y = SystemInformation.MenuHeight * this.Index + 1;
            e.Graphics.DrawLine(Pens.LightGray, x_min, y, x_max, y);
        }
    }


}
