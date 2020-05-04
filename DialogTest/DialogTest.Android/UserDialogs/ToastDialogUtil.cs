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
using Plugin.CurrentActivity;
using UserDialogs;
using Xamarin.Essentials;
using Xamarin.Forms.Platform.Android;

namespace BoxApp.Droid.DroidRender.UserDialogs
{
    public class ToastDialogUtil
    {
        protected Xamarin.Forms.Size DSizeByDensity
        {
            get
            {
                return UserDialogsFactory.WinowSize;
            }
        }

        protected Xamarin.Forms.View _xfView;
        protected DialogConfig _dialogConfig;
        protected DialogMsg _dialogMsg;
        protected Context _mContext;
        protected Xamarin.Forms.Size _formSize;
        protected bool _isLong;
        protected bool _isNative;

        public static Toast lastToast = null;
        public static Toast _nativeToast = null;

        public ToastDialogUtil(Context context, Xamarin.Forms.View view, DialogConfig dialogConfig
            , DialogMsg dialogMsg, bool isLong, bool isNative)
        {

            _xfView = view;
            _mContext = context;
            _dialogConfig = dialogConfig;
            _dialogMsg = dialogMsg;
            _isLong = isLong;
            _isNative = isNative;
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
            if (_xfView != null)
            {
                //设定文本          
                var dialogEle = _xfView as IDialogElement;
                if (dialogEle != null)
                {
                    //dialogEle.SetDialogMsg(_dialogMsg);
                }
                var droidView = _xfView.ConvertFormsToNative(_mContext);
                _formSize = _xfView.Measure(DSizeByDensity.Width, DSizeByDensity.Height).Request;
                _xfView.Layout(new Xamarin.Forms.Rectangle(0, 0, _formSize.Width, _formSize.Height));
                var lastView = WrapperViewAndControlSize(droidView);
                toast.View = lastView;
            }
            else
            {
                toast.SetText(_dialogMsg.ContentMsg);
            }
        }

        protected virtual View WrapperViewAndControlSize(View view)
        {
            var linearLayout = new LinearLayout(_mContext);
            var density = DeviceDisplay.MainDisplayInfo.Density;
            LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams((int)Math.Ceiling(_formSize.Width * density)
                , (int)Math.Ceiling(_formSize.Height * density));
            linearLayout.AddView(view, lp);
            return linearLayout;

        }

        protected virtual void SetToastConfig(Toast toast)
        {
            if (_dialogConfig.DialogPosition == DialogPosition.ToastDefault)
            {
                return;
            }
            int xOffset = (int)Math.Ceiling(_dialogConfig.XOffset * UserDialogsFactory.Density);
            int yOffset = (int)Math.Ceiling(_dialogConfig.YOffset * UserDialogsFactory.Density);
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


}