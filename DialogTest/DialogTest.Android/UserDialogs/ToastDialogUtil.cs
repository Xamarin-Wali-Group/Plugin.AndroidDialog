using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Box.Plugs.Dialog;
using UserDialogs;
using Xamarin.Essentials;

namespace BoxApp.Droid.DroidRender.UserDialogs
{
    public class ToastDialogUtil
    {
        private Point _windowSize;
        protected Point WindowSize
        {
            get
            {
                if (_windowSize != null && _windowSize.X != 0 && _windowSize.Y != 0)
                {
                    return _windowSize;
                }
                _windowSize = new Point();
                Display display = _mContext.WindowManager.DefaultDisplay;
                display.GetSize(_windowSize);
                return _windowSize;
            }
        }

        private double _density;
        protected double Density
        {
            get
            {
                if (_density == 0)
                {
                    var dm = new Android.Util.DisplayMetrics();
                    _mContext.WindowManager.DefaultDisplay.GetMetrics(dm);
                    _density = dm.Density;
                }
                return _density;
            }
        }

     

        protected Xamarin.Forms.View _toastView;
        protected DialogConfig _dialogConfig;
        protected IDialogMsg _dialogMsg;
        protected Activity _mContext;
        protected Xamarin.Forms.Size _toastViewSize;
        protected bool _isLong;
        protected bool _isNative;

        public static Toast lastToast = null;
        public static Toast _nativeToast = null;

        public ToastDialogUtil(Activity context, Xamarin.Forms.View view, DialogConfig dialogConfig
            , IDialogMsg dialogMsg, bool isLong, bool isNative)
        {

            _toastView = view;
            _mContext = context;
            _dialogConfig = dialogConfig;
            _dialogMsg = dialogMsg;
            _isLong = isLong;
            _isNative = isNative;
            if (_toastView==null)
            {
                _isNative = true;
            }
        }

        public Toast Builder()
        {
            if (lastToast != null)
            {
                lastToast.Cancel();
                lastToast.Dispose();
                lastToast = null;
            }

            Toast nowToast;
            if (_isNative)
            {
                if (_nativeToast == null)
                {
                    _nativeToast = Toast.MakeText(_mContext, "", ToastLength.Short);
                }
                nowToast = _nativeToast;                 
            }
            else
            {
                nowToast = new Toast(_mContext);
            }
            SetViewAndText(nowToast);
            SetToastConfig(nowToast);
            SetToastDuring(nowToast);
            if (!_isNative)
            {
                lastToast = nowToast;
            }
            return nowToast;
        }

     
        protected virtual void SetViewAndText(Toast toast)
        {
            if (_toastView != null)
            {
                //设定文本          
                var dialogEle = _toastView as IDialogElement;
                dialogEle?.OnCreated(_dialogMsg);
                var droidView = _toastView.ConvertFormsToNative(_mContext);
                var winWidth = WindowSize.X / Density;
                var winHeight = WindowSize.Y / Density;
                _toastViewSize = _toastView.Measure(winWidth, winHeight).Request;
                _toastView.Layout(new Xamarin.Forms.Rectangle(0, 0, _toastViewSize.Width, _toastViewSize.Height));
                var lastView = WrapperViewAndControlSize(droidView);
                toast.View = lastView;
            }
            else
            {
                toast.SetText(_dialogMsg.Msg);
            }
            
        }

        /// <summary>
        /// 将自定义View添加入容器LinearLayout中
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        protected virtual View WrapperViewAndControlSize(View view)
        {            
            var linearLayout = new LinearLayout(_mContext);
            var density = DeviceDisplay.MainDisplayInfo.Density;
            LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams((int)Math.Ceiling(_toastViewSize.Width * density)
                , (int)Math.Ceiling(_toastViewSize.Height * density));
            linearLayout.AddView(view, lp);
            return linearLayout;

        }

        protected virtual void SetToastConfig(Toast toast)
        {
            if (_dialogConfig.DialogPosition == DialogPosition.ToastDefault)
            {
                return;
            }
            int xOffset = (int)Math.Ceiling(_dialogConfig.XOffset * Density);
            int yOffset = (int)Math.Ceiling(_dialogConfig.YOffset * Density);
            if (_dialogConfig.DialogPosition == DialogPosition.Custom)
            {
                toast.SetGravity(GravityFlags.Left | GravityFlags.Top, xOffset
                    , yOffset);
            }
            else
            {
                toast.SetGravity((GravityFlags)_dialogConfig.DialogPosition
                     , xOffset
                     , yOffset);
            }
        }

        protected virtual void SetToastDuring(Toast toast)
        {
            toast.Duration = _isLong ? ToastLength.Long : ToastLength.Short;
        }


    }

    public class ToastMsg : IDialogMsg
    {
        public string Msg { get; set ; }
    }

}