﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EspionSpotify.Extensions
{
    public static class ControlExtensions
    {
        public static TResult GetPropertyThreadSafe<TControl, TResult>(this TControl control, Func<TControl, TResult> getter)
            where TControl : Control
        {
            if (control.InvokeRequired)
            {
                return (TResult)control.Invoke(getter, control);
            }
            else
            {
                return getter(control);
            }
        }

        public static void SetPropertyThreadSafe<TControl>(this TControl control, MethodInvoker setter)
           where TControl : Control
        {
            if (control.InvokeRequired)
            {
                control.Invoke(setter);
            }
            else
            {
                setter();
            }
        }
    }
}
