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
using DialogTest.Droid.UserDialogs;
using Plugin.CurrentActivity;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
//using XFView = Xamarin.Forms.View;


[assembly: Dependency(typeof(BoxApp.Droid.DroidRender.UserDialogs.UserDialogsFactory))]
namespace BoxApp.Droid.DroidRender.UserDialogs
{
    public class UserDialogsFactory : IUserDialogsFactory
    {       
        private static Activity _activity;

        private DialogsInitize _dialogsInitize =>  DialogsInitize.Instance();

        private  Android.Support.V4.App.FragmentManager _fragmentManager => _activity.GetFragmentManager();


        public static void InitActivity(Activity activity)
        {
            _activity = activity;
        }

        public IDialog CreateDialog(DialogType dialogType, IDialogMsg dialogMsg, DialogConfig config = null)
        {

            var contentView = _dialogsInitize.GetInitDialogContentView(dialogType);          
            if (contentView == null)
            {
                throw new Exception($"{dialogType} not map");
            }
            if (config==null)
            {
                config = _dialogsInitize.GetInitDialogConfig(dialogType);
            }
            DialogResultManager resultManager = new DialogResultManager();
            var dialogFragment = new BaseDialogFragment2(_activity,contentView, config, dialogMsg);
            var dialogDroid = new DialogInstance(dialogFragment, _fragmentManager);
            return dialogDroid;
        }

        public IDialog CreateDialog(Xamarin.Forms.View contentView, IDialogMsg dialogMsg, DialogConfig config)
        {
            if(contentView == null)
            {
                throw new ArgumentException($"dialog contentView is null");
            }
            if (config==null)
            {
                config = new DialogConfig();
            }
            var dialogFragment = new BaseDialogFragment2(_activity, contentView, config, dialogMsg);
            var dialogDroid = new DialogInstance(dialogFragment, _fragmentManager);
            return dialogDroid;
        }

        public void Toast(string msg, bool islong = false, bool isNative = false)
        {
            Xamarin.Forms.View toastView = isNative ? null : _dialogsInitize.GetInitToastView();
           
            //ToastDialogUtil toastDialog = new ToastDialogUtil(Context, toastView, lastdialogConfig
            //    , dialogMsg, islong, isNative);
            //var toast = toastDialog.Builder();
            //if (toast != null)
            //{
            //    toast.Show();
            //}
        }
    }
}


