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
using Plugin.CurrentActivity;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XFView = Xamarin.Forms.View;


[assembly: Dependency(typeof(BoxApp.Droid.DroidRender.UserDialogs.UserDialogsFactory))]
namespace BoxApp.Droid.DroidRender.UserDialogs
{
    public class UserDialogsFactory : IUserDialogsFactory
    {
        public static double Density { get; private set; }

        public static Xamarin.Forms.Size WinowSize { get; private set; }

        public static Android.Graphics.Point WindowPoint { get; private set; }

        static UserDialogsFactory()
        {
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            Density = mainDisplayInfo.Density;
            WinowSize = new Size()
            {
                Width = mainDisplayInfo.Width / Density,
                Height = mainDisplayInfo.Height / Density
            };
            var windowManager = CrossCurrentActivity.Current.Activity.WindowManager;
            Display display = windowManager.DefaultDisplay;
            WindowPoint = new Android.Graphics.Point();
            display.GetSize(WindowPoint);
        }

        Context Context => CrossCurrentActivity.Current.Activity;
        Android.Support.V4.App.FragmentManager FragmentManage => CrossCurrentActivity.Current.Activity.GetFragmentManager();

        /// <summary>
        /// 获取Dialog配置
        /// </summary>
        /// <param name="config"></param>
        /// <param name="defaultAlertView"></param>
        /// <returns></returns>
        DialogConfig GetDialogConfigByView(DialogConfig config, XFView defaultAlertView)
        {
            DialogConfig lastdialogConfig = config;       
            if (defaultAlertView is IDialogElement  dialogElement)
            {
                if (config == null)
                {
                    var viewDialogConfig = dialogElement.GetViewDefaultDialogConfig();
                    lastdialogConfig = viewDialogConfig;
                }
            }
            else
            {
                if (config == null)
                {
                    lastdialogConfig = new DialogConfig();
                }
            }
            return lastdialogConfig;
        }



        #region 实现

        #region Toast

        public void Toast(string msg, DialogConfig config = null, bool islong = false, bool isNative = false)
        {
            var dialogMsg = new DialogMsg()
            {
                ContentMsg = msg
            };
            XFView toastView = isNative ? null : DialogsServer.ToastDialogsFunc?.Invoke();
            DialogConfig lastdialogConfig = GetDialogConfigByView(config, toastView);
            ToastDialogUtil toastDialog = new ToastDialogUtil(Context, toastView, lastdialogConfig
                , dialogMsg, islong, isNative);
            var toast = toastDialog.Builder();
            if (toast != null)
            {
                toast.Show();
            }

        }

        #endregion

        #region ButtonDialog
        public IDialog ButtonDialog(string msg, string postiveBtn, string negativeBtn, string neutralbtn = null,
       DialogConfig config = null, bool isNative = false)
        {
            var dialogMsg = new DialogMsg()
            {
                ContentMsg = msg,
                PositiveButton = postiveBtn,
                NegativeButton = negativeBtn,
                NeutralButton = neutralbtn
            };
            XFView defaultAlertView = isNative ? null : DialogsServer.ButtonDialogsFunc?.Invoke();
            DialogConfig lastdialogConfig = GetDialogConfigByView(config, defaultAlertView);
            var dialogFragment = new ButtonDialogFragment(Context,
                defaultAlertView, lastdialogConfig, dialogMsg);

            var lwyDialogDroid = new DialogInstance(dialogFragment, FragmentManage, defaultAlertView);
            if (defaultAlertView is IDialogElement dialogElement)
            {
                dialogElement.LwyDialog = lwyDialogDroid;
            }
            return lwyDialogDroid;
        }

        public IDialog ButtonDialog(DialogMsg dialogMsg,
          DialogConfig config = null, bool isNative = false)
        {
            XFView defaultAlertView = isNative ? null : DialogsServer.ButtonDialogsFunc?.Invoke();
            DialogConfig lastdialogConfig = GetDialogConfigByView(config, defaultAlertView);
            var dialogFragment = new ButtonDialogFragment(Context,
                defaultAlertView, lastdialogConfig, dialogMsg);

            var lwyDialogDroid = new DialogInstance(dialogFragment, FragmentManage, defaultAlertView);
            if (defaultAlertView is IDialogElement dialogElement)
            {
                dialogElement.LwyDialog = lwyDialogDroid;
            }
            return lwyDialogDroid;
        }

        #endregion


        #region LoadDialog
        public IDialog LoadDialog(string msg, DialogConfig config = null, bool isNative = false)
        {
            var dialogMsg = new DialogMsg()
            {
                ContentMsg = msg
            };
            XFView defaultAlertView = isNative ? null : DialogsServer.LoadDialogsFunc?.Invoke();
            DialogConfig lastdialogConfig = GetDialogConfigByView(config, defaultAlertView);
            var dialogFragment = new LoadDialogFragment(Context,
                defaultAlertView, lastdialogConfig, dialogMsg);

            var lwyDialogDroid = new DialogInstance(dialogFragment, FragmentManage, defaultAlertView);
            if (defaultAlertView is IDialogElement dialogElement)
            {
                dialogElement.LwyDialog = lwyDialogDroid;
            }
            return lwyDialogDroid;
        }

        public IDialog LoadDialog(DialogMsg dialogMsg, DialogConfig config = null, bool isNative = false)
        {
            XFView defaultAlertView = isNative ? null : DialogsServer.LoadDialogsFunc?.Invoke();
            DialogConfig lastdialogConfig = GetDialogConfigByView(config, defaultAlertView);
            var dialogFragment = new LoadDialogFragment(Context,
                defaultAlertView, lastdialogConfig, dialogMsg);

            var lwyDialogDroid = new DialogInstance(dialogFragment, FragmentManage, defaultAlertView);
            if (defaultAlertView is IDialogElement dialogElement)
            {
                dialogElement.LwyDialog = lwyDialogDroid;
            }
            return lwyDialogDroid;
        }

        #endregion

        #region CustomDialog
        public IDialog CustomDialog(XFView view, DialogConfig config = null)
        {
            BaseDialogFragment baseDialogFragment = new ButtonDialogFragment(Context, view, config, null);
            var lwyDialogDroid = new DialogInstance(baseDialogFragment, FragmentManage, view);
            if (view is IDialogElement dialogElement)
            {
                dialogElement.LwyDialog = lwyDialogDroid;
            }
            return lwyDialogDroid;
        } 
        #endregion

        #endregion
    }
}


