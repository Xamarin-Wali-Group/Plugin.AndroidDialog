using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Box.Plugs.Dialog
{
    public interface IUserDialogsFactory
    {
        void Toast(string msg, DialogConfig config = null, bool islong = false, bool isNative = false);
    
        IDialog ButtonDialog(string msg, string button1Text, string button2Text, string button3Text=null,
            DialogConfig config = null, bool isNative = false);

        IDialog ButtonDialog(DialogMsg dialogMsg,
          DialogConfig config = null, bool isNative = false);

        IDialog LoadDialog(string msg, DialogConfig config = null, bool isNative = false);

        IDialog LoadDialog(DialogMsg dialogMsg, DialogConfig config = null, bool isNative = false);

        IDialog CustomDialog(View view, DialogConfig config = null);
    }
}
