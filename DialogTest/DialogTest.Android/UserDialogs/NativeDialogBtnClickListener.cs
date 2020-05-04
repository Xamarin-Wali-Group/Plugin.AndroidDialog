using Android.Content;
using Android.Runtime;
using Box.Plugs.Dialog;
using System;
using Xamarin.Forms;

namespace BoxApp.Droid.DroidRender.UserDialogs
{
    public class NativeDialogBtnClickListener : Java.Lang.Object, IDialogInterfaceOnClickListener
    {
        DialogMsg _dialogMsg;
        public NativeDialogBtnClickListener(DialogMsg dialogMsg)
        {
            _dialogMsg = dialogMsg;
        }

        public NativeDialogBtnClickListener(IntPtr handle, JniHandleOwnership transfer)
            :base(handle,transfer)
        {

        }


        public void OnClick(IDialogInterface dialog, int which)
        {
            var index = Math.Abs(which);
            string resultStr = null;
            if (index==1)
            {
                resultStr = _dialogMsg.PositiveButton;
            }
            if (index == 2)
            {
                resultStr = _dialogMsg.NegativeButton;
            }
            if (index == 3)
            {
                resultStr = _dialogMsg.NeutralButton;
            }
            //MessagingCenter.Send<NativeDialogBtnClickListener, string>
            //    (this, BaseDialogFragment.MessagingCenter_Tag, resultStr);
        }
    }
}