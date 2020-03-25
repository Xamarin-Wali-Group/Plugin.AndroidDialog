using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V7.App;
using Box.Plugs.Dialog;
using Xamarin.Forms;

namespace BoxApp.Droid.DroidRender.UserDialogs
{
    public class ButtonDialogFragment : BaseDialogFragment
    {
        public ButtonDialogFragment(Context context, Xamarin.Forms.View view, DialogConfig dialogConfig
             , DialogMsg dialogMsg)
            : base(context, view, dialogConfig, dialogMsg)
        {
           
        }
     
      

        protected override Dialog SetNativeDialog()
        {
            Android.Support.V7.App.AlertDialog.Builder builder = new Android.Support.V7.App.AlertDialog.Builder(_mContext);
            SetNativeDialogMsgText(_dialogMsg, builder);
            var dialog = builder.Create();
            return dialog;
        }

        protected virtual void SetNativeDialogMsgText(DialogMsg dialogMsg,
            Android.Support.V7.App.AlertDialog.Builder builder)
        {         
            NativeDialogBtnClickListener clickListener = new NativeDialogBtnClickListener(dialogMsg);
            if (!string.IsNullOrEmpty(dialogMsg.Title))
            {
                builder.SetTitle(dialogMsg.Title);
            }
            if (!string.IsNullOrEmpty(dialogMsg.Title))
            {
                builder.SetMessage(dialogMsg.ContentMsg);
            }
            if (!string.IsNullOrEmpty(dialogMsg.PositiveButton))
            {
                builder.SetPositiveButton(text:dialogMsg.PositiveButton,listener: clickListener);
            }
            if (!string.IsNullOrEmpty(dialogMsg.NegativeButton))
            {
                builder.SetNegativeButton(text: dialogMsg.NegativeButton, listener: clickListener);
            }
            if (!string.IsNullOrEmpty(dialogMsg.NeutralButton))
            {
                builder.SetNeutralButton(text: dialogMsg.NeutralButton, listener: clickListener);
            }
        }

        protected override void SetDialogWindowBGDrawable()
        {
            if (IsNative)
            {
                _dialogConfig.BackgroundColor = Color.White;                
            }
            base.SetDialogWindowBGDrawable();
        }
    }
}