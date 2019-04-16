using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PP791
{
    public static class Extension
    {
        /*************************************************************/
        public static void InvokeIfRequired(
         this Control control, MethodInvoker action)
        {
            //List<Control> controlList = new List<Control>();
            //foreach (Control x in control)
            //{

            if (control.InvokeRequired)//在非當前執行緒內 使用委派
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
            //}

        }
    }
}
