using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Plugin.PopUpDialog.Shared
{
    public interface IUserDialogsFactory
    {
        void Toast(string msg, bool isNative = false,bool islong=false);

        void Toast(IDialogMsg dialogMsg, DialogConfig config = null, bool islong = false, bool isNative = false);

        IDialog CreateDialog(DialogType dialogType, IDialogMsg dialogMsg, DialogConfig config = null);

        IDialog CreateDialog(View contentView, IDialogMsg dialogMsg, DialogConfig config);

        IDialog PopupView(View baseView, View popupView, IDialogMsg dialogMsg, DialogConfig config);
    }

    public enum DialogType 
    {
        Confirm,
        Alert,
        Load

    }

    public interface IDialogMsg
    {
        string Msg { get; set; }
    }
}
