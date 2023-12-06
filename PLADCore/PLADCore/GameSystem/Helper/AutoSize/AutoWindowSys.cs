using System;
using System.Drawing;
using System.Windows.Forms;

namespace PLADCore.GameSystem.Helper
{
     public class AutoWindowSys
 {
     private readonly float x;
     private readonly float y;

     public AutoWindowSys(float x, float y)
     {
         this.x = x;
         this.y = y;
     }

     /// <summary>
     ///
     /// </summary>
     /// <param name="control"></param>
     public void SetTag(Control control)
     {
         foreach (Control con in control.Controls)
         {
             con.Tag = con.Width + ";" + con.Height + ";" + con.Left + ";" + con.Top + ";" + con.Font.Size;
             if (con.Controls.Count > 0) SetTag(con);
         }
     }

     /// <summary>
     ///
     /// </summary>
     public void ReWinformLayout(Control control)
     {
         var newx = control.Width / x;
         var newy = control.Height / y;
         SetControls(newx, newy, control);
     }

     /// <summary>
     ///
     /// </summary>
     /// <param name="newx"></param>
     /// <param name="newy"></param>
     /// <param name="cons"></param>
     private void SetControls(float newx, float newy, Control cons)
     {
         foreach (Control con in cons.Controls)
         {
             //��ȡ�ؼ���Tag����ֵ�����ָ��洢�ַ�������
             if (con.Tag != null)
             {
                 var mytag = con.Tag.ToString().Split(';');
                 //���ݴ������ŵı���ȷ���ؼ���ֵ
                 con.Width = Convert.ToInt32(Convert.ToSingle(mytag[0]) * newx); //���
                 con.Height = Convert.ToInt32(Convert.ToSingle(mytag[1]) * newy); //�߶�
                 con.Left = Convert.ToInt32(Convert.ToSingle(mytag[2]) * newx); //��߾�
                 con.Top = Convert.ToInt32(Convert.ToSingle(mytag[3]) * newy); //���߾�
                 var currentSize = Convert.ToSingle(mytag[4]) * newy; //�����С
                 if (currentSize > 0) con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                 con.Focus();
                 if (con.Controls.Count > 0)
                 {
                     SetControls(newx, newy, con);
                 }
             }
         }
     }
 }
}