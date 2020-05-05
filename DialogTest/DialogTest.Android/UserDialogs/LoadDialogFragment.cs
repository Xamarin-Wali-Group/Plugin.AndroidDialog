using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Box.Plugs.Dialog;

namespace BoxApp.Droid.DroidRender.UserDialogs
{
    public class LoadDialogFragment : BaseDialogFragment
    {
        public LoadDialogFragment(IntPtr a, Android.Runtime.JniHandleOwnership b)
            :base(a,b)
        {

        }
        public LoadDialogFragment(Context context, Xamarin.Forms.View view, DialogConfig dialogConfig, DialogMsg dialogMsg)
            : base(context, view, dialogConfig, dialogMsg)
        {
        }

      

        //protected override Dialog SetNativeDialog()
        //{
        //    var progressDialog = new ProgressDialog(Context);
        //    SetNativeLoadDialogMsgText(progressDialog,_dialogMsg);
        //    return progressDialog;
        //}

        //protected override void SetDialogWindowBGDrawable()
        //{
        //    if (IsNative)
        //    {
        //        _dialogConfig.BackgroundColor = Xamarin.Forms.Color.White;
        //    }
        //    base.SetDialogWindowBGDrawable();
        //}

        protected virtual void SetNativeLoadDialogMsgText(ProgressDialog progressDialog,DialogMsg  dialogMsg) 
        {
            NativeDialogBtnClickListener clickListener = new NativeDialogBtnClickListener(dialogMsg);
            if (!string.IsNullOrEmpty(_dialogMsg.Title))
            {
                progressDialog.SetTitle(_dialogMsg.Title);
            }
            if (!string.IsNullOrEmpty(_dialogMsg.ContentMsg))
            {
                progressDialog.SetMessage(_dialogMsg.ContentMsg);
            }
            if (!string.IsNullOrEmpty(_dialogMsg.PositiveButton))
            {
                progressDialog.SetButton(text: _dialogMsg.PositiveButton, listener: clickListener);
            }
            if (!string.IsNullOrEmpty(_dialogMsg.NegativeButton))
            {
                progressDialog.SetButton2(text: _dialogMsg.NegativeButton, listener: clickListener);
            }
            if (!string.IsNullOrEmpty(_dialogMsg.NeutralButton))
            {
                progressDialog.SetButton3(text: _dialogMsg.NeutralButton, listener: clickListener);
            }
        }

       
    }
}