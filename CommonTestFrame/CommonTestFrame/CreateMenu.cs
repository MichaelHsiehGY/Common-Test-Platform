using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CommonTestFrame
{
    /// <summary>
    /// 重新绘制右键菜单
    /// </summary>    
    public class ContextMenuEx : System.Windows.Forms.MenuItem
    {
        /// <summary>
        /// 菜单的字体样式
        /// </summary>
        private Font menu_Font;

        /// <summary>
        /// 菜单中左边的字体样式 
        /// </summary>
        private Font product_Font;

        /// <summary>
        /// 弹出菜单边框样式
        /// </summary>
        private Pen menu_pen;

        /// <summary>
        /// 弹出菜单老的边框样式
        /// </summary>
        private Pen menu_oldPen;

        /// <summary>
        /// 渐变起始色
        /// </summary>
        private Color color_top;

        /// <summary>
        /// 渐变中间色top
        /// </summary>
        private Color color_middle_top;

        /// <summary>
        /// 渐变中间色bottom
        /// </summary>
        private Color color_middle_bottom;

        /// <summary>
        /// 渐变底部色
        /// </summary>
        private Color color_bottom;

        /// <summary>
        /// 渐变笔刷
        /// </summary>
        private LinearGradientBrush menu_linerBursh;

        private SolidBrush menu_LeftBackColor;

        public ContextMenuEx()
        {
            this.OwnerDraw = true;
            menu_Font = SystemInformation.MenuFont;
            product_Font = new Font("宋体", 13, FontStyle.Bold, GraphicsUnit.World);
            menu_pen = new Pen(Color.LightGray, 1.5f);
            menu_oldPen = new Pen(Color.White, 1.5f);  
            menu_LeftBackColor = new SolidBrush(Color.FromArgb(222, 227, 255));


            color_top = Color.FromArgb(234, 238, 249);
            color_middle_top = Color.FromArgb(222, 227, 255);
            color_middle_bottom = Color.FromArgb(195, 207, 246);
            color_bottom = Color.FromArgb(156, 177, 240);
        }
        //这是很重要的，这给你的菜单定义了大小，
        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            e.ItemHeight = SystemInformation.MenuHeight;
            e.ItemWidth = 150;

        }
        protected override void OnDrawItem(System.Windows.Forms.DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            //鼠标移动上去后的状态
            if (e.State == DrawItemState.NoAccelerator && this.Enabled)
            {
                //单击后鼠标未移动到该菜单上去
                g.FillRectangle(Brushes.White, e.Bounds);
                g.DrawRectangle(menu_oldPen, e.Bounds);
            }

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected && this.Enabled)
            {
                //鼠标移动上去了，但是没有单击
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

            //左边灰色部分
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
            this.Enabled = false;//这里设置为false的话用户单击线条时可使菜单不会消失
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
