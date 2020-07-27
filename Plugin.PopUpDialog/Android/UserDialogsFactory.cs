using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using DialogTest.Droid.UserDialogs;
using Plugin.PopUpDialog.Shared;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using FragmentManager = Android.Support.V4.App.FragmentManager;

[assembly: Dependency(typeof(Plugin.PopUpDialog.Android.UserDialogsFactory))]
namespace Plugin.PopUpDialog.Android
{
    public class UserDialogsFactory : IUserDialogsFactory
    {       
        private static Activity _activity;

        private DialogsInitize _dialogsInitize =>  DialogsInitize.Instance();

        private  FragmentManager _fragmentManager => _activity.GetFragmentManager();


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
            DialogResultManager manager = new DialogResultManager();
            manager.Build();
            var dialogFragment = new BaseDialogFragment2(_activity,contentView, config, dialogMsg,
                manager.GetDialogResult());
            var dialogDroid = new DialogInstance(dialogFragment, _fragmentManager,contentView,
                manager.GetResultMission());
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
            IDialogResult dialogResult = null;
            TaskCompletionSource<string> mission = null;
            if (contentView is IDialogElement)
            {
                DialogResultManager manager = new DialogResultManager();
                manager.Build();
                dialogResult = manager.GetDialogResult();
                mission = manager.GetResultMission();
            }            
            var dialogFragment = new BaseDialogFragment2(_activity, contentView, config, dialogMsg, dialogResult);
            var dialogDroid = new DialogInstance(dialogFragment, _fragmentManager,contentView ,mission);
            return dialogDroid;
        }

        public void Toast(string msg, bool islong = false, bool isNative = false)
        {
            Xamarin.Forms.View toastView = isNative ? null : _dialogsInitize.GetInitToastView();
            var config = new DialogConfig() { DialogPosition=DialogPosition.ToastDefault};
            var dialogMsg = new ToastMsg() { Msg=msg};
            ToastDialogUtil toastDialog = new ToastDialogUtil(_activity, toastView, config
                , dialogMsg, islong, isNative);
            var toast = toastDialog.Builder();
            if (toast != null)
            {
                toast.Show();
            }
        }

        public void Toast(IDialogMsg dialogMsg, DialogConfig config = null, bool islong = false, bool isNative = false)
        {
            Xamarin.Forms.View toastView = isNative ? null : _dialogsInitize.GetInitToastView();
            if (config==null)
            {
                config = new DialogConfig() { DialogPosition = DialogPosition.ToastDefault };
            }                        
            ToastDialogUtil toastDialog = new ToastDialogUtil(_activity, toastView, config
                , dialogMsg, islong, isNative);
            var toast = toastDialog.Builder();
            if (toast != null)
            {
                toast.Show();
            }
        }

        /// <summary>
        /// PopupView
        /// </summary>
        /// <param name="baseView"></param>
        /// <param name="popupView"></param>
        /// <param name="dialogMsg"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public IDialog PopupView(Xamarin.Forms.View baseView, Xamarin.Forms.View popupView, IDialogMsg dialogMsg, DialogConfig config)
        {
            if (baseView == null || popupView == null)
            {
                throw new ArgumentException($"dialog contentView is null");
            }

            if (config == null)
            {
                config = new DialogConfig()
                {
                    DialogPosition = DialogPosition.Buttom
                };
            }
            IDialogResult dialogResult = null;
            TaskCompletionSource<string> mission = null;
            if (popupView is IDialogElement)
            {
                DialogResultManager manager = new DialogResultManager();
                manager.Build();
                dialogResult = manager.GetDialogResult();
                mission = manager.GetResultMission();
            }
            var rect = GetBaseViewRect(baseView);
            var dialogFragment = new PopupDialogFragment(_activity, popupView, config, dialogMsg, rect, dialogResult);
            var dialogDroid = new DialogInstance(dialogFragment, _fragmentManager, popupView, mission);
            return dialogDroid;

        }


        int GetStatusHeight()
        {
            var resources = _activity.ApplicationContext.Resources;
            int resourceId = resources.GetIdentifier("status_bar_height", "dimen", "android");
            int height = resources.GetDimensionPixelSize(resourceId);
            return height;
        }

        BaseViewRect GetBaseViewRect(Xamarin.Forms.View baseView)
        {
            int statusHeight = GetStatusHeight();
            var baseViewNative = baseView.ConvertFormsToNative();
            var sc = new int[2];
            baseViewNative.GetLocationInWindow(sc);
            return new BaseViewRect(sc[0], sc[1] - statusHeight, baseViewNative.Width, baseViewNative.Height);
        }
    }
}


