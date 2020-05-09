using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Media.TV;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Util;
using Android.Widget;
using Box.Plugs.Dialog;
using DialogTest.Dialog;
using UserDialogs;
using Xamarin.Forms.Platform.Android;
using DialogFragment = Android.Support.V4.App.DialogFragment;
namespace DialogTest.Droid.UserDialogs
{
    public class BaseDialogFragment2 : DialogFragment
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
                Display display = _context.WindowManager.DefaultDisplay;
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
                    _context.WindowManager.DefaultDisplay.GetMetrics(dm);
                    _density = dm.Density;
                }
                return _density;
            }
        }


        protected Activity _context;
        protected Xamarin.Forms.View _contentView;
        protected DialogConfig _dialogConfig;
        protected IDialogElement _dialogElement;
        protected IDialogMsg _iDialogMsg;
        protected Size _dialogViewSize;
        protected IDialogResult _dialogResult;


        public BaseDialogFragment2(IntPtr a, JniHandleOwnership b)
        {

        }

        public BaseDialogFragment2(Activity activity, Xamarin.Forms.View contentView,
         DialogConfig dialogConfig, IDialogMsg dialogMsg, IDialogResult dialogResult = null)
        {
            _context = activity;
            _dialogConfig = dialogConfig;
            _iDialogMsg = dialogMsg;
            _contentView = contentView;
            _dialogResult = dialogResult;
            if (contentView is IDialogElement dialogElement)
            {
                _dialogElement = contentView as IDialogElement;
            }
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            UnRegisterEvent();
        }

        #region 为Dialog绑定DialogElement的事件
        protected virtual void RegisterDialogElementEvent(Android.App.Dialog dialog)
        {
            if (_dialogElement != null)
            {
                dialog.ShowEvent += Dialog_ShowEvent;
                dialog.DismissEvent += DialogDismiss_ElementClose;
            }
            dialog.DismissEvent += DialogDismiss_DisposeFragment;
        }

        protected virtual void Dialog_ShowEvent(object sender, EventArgs e)
        {
            _dialogElement.OnShowed();
        }

        protected virtual void DialogDismiss_ElementClose(object sender, EventArgs e)
        {
            _dialogElement.OnClosed();
        }

        #endregion


        public override Android.App.Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var dialog = base.OnCreateDialog(savedInstanceState);
            SetDialogWindowBGDrawable(dialog.Window);

            SetDialogCloseWays(_dialogConfig, dialog);
            return dialog;
        }

        /// <summary>
        /// 构造XF中对应的ContentView
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var dialogView = _contentView.ConvertFormsToNative(_context.ApplicationContext);
            _dialogElement?.OnCreated(_iDialogMsg);
            if (_dialogElement != null)
            {
                _dialogElement.DialogResult = _dialogResult;
            }
            return dialogView;
        }

        /// <summary>
        /// 此方法在视图View已经创建后返回的，此时view 还没有添加到父级中。
        /// 构造View后，计算view的大小，并且调用View的layout方法，完成布局
        /// </summary>
        /// <param name="view"></param>
        /// <param name="savedInstanceState"></param>
        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            var size = _contentView.Measure(WindowSize.X / Density, WindowSize.Y / Density).Request;

            _contentView.Layout(new Xamarin.Forms.Rectangle(0, 0, size.Width, size.Height));
            _dialogViewSize = new Size((int)Math.Ceiling(size.Width * Density)
                , (int)Math.Ceiling(size.Height * Density));
        }


        #region 设置Dialog的window属性

        void SetDialogWindowPosition(WindowManagerLayoutParams attrs)
        {
            if (_dialogConfig.DialogPosition == DialogPosition.Custom)
            {
                attrs.Gravity = GravityFlags.Top | GravityFlags.Left;
            }
            else
            {
                attrs.Gravity = (GravityFlags)((int)_dialogConfig.DialogPosition);
            }
            attrs.X = (int)Math.Ceiling(_dialogConfig.XOffset * Density);
            attrs.Y = (int)Math.Ceiling(_dialogConfig.YOffset * Density);
        }

        void SetDialogWindowSize(WindowManagerLayoutParams attrs)
        {

            attrs.Width = this._dialogViewSize.Width;
            attrs.Height = this._dialogViewSize.Height;
        }

        void SetDialogWindowFlags(Window window, WindowManagerLayoutParams attrs)
        {

            if (_dialogConfig.NotTouchModal)
            {
                window.ClearFlags(WindowManagerFlags.DimBehind);
                window.AddFlags(WindowManagerFlags.NotTouchModal);
            }
            else
            {
                attrs.DimAmount = _dialogConfig.DimAmount;
            }
        }

        protected virtual void SetDialogWindowBGDrawable(Window window)
        {
            var andColor = _dialogConfig.BackgroundColor.ToAndroid();
            var dw = new ColorDrawable(andColor);
            window.SetBackgroundDrawable(dw);
        }

        /// <summary>
        /// 设置弹出动画
        /// </summary>
        /// <returns></returns>
        protected virtual void SetDialogAnimation(WindowManagerLayoutParams attrs)
        {
            var dialogAnimateResId = this._context.Resources.GetIdentifier(_dialogConfig.DialogAnimationConfig, "style",
                _context.PackageName);
            attrs.WindowAnimations = dialogAnimateResId;
        }


        #endregion

        #region 设置Dialog的关闭方式
        public void SetDialogCloseWays(DialogConfig config, Android.App.Dialog dialog)
        {
            if (!config.IsCanCancel)
            {
                dialog.SetCancelable(false);
                return;
            }
            if (!config.IsCloseByTouchMask)
            {
                dialog.SetCancelable(true);
                dialog.SetCanceledOnTouchOutside(false);
            }
        }
        #endregion


        public override void OnStart()
        {
            base.OnStart();
            var window = Dialog.Window;
            var attrs = window.Attributes;
            SetDialogWindowSize(attrs);
            SetDialogWindowPosition(attrs);
            SetDialogWindowFlags(window, attrs);
            SetDialogAnimation(attrs);
            RegisterDialogElementEvent(Dialog);
            window.Attributes = attrs;
        }



        /// <summary>
        /// dialog关闭后，释放资源，销毁fragment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DialogDismiss_DisposeFragment(object sender, EventArgs e)
        {
            this.DismissAllowingStateLoss();
            this.FragmentManager.BeginTransaction().Remove(this).Commit();
        }



        /// <summary>
        /// 释放事件,防止内存泄漏
        /// </summary>
        protected virtual void UnRegisterEvent()
        {
            if (Dialog != null)
            {
                try
                {
                    if (_dialogElement != null)
                    {
                        Dialog.ShowEvent -= Dialog_ShowEvent;
                        Dialog.DismissEvent -= DialogDismiss_ElementClose;
                        Dialog.DismissEvent -= DialogDismiss_DisposeFragment;
                    }
                    Dialog.SetOnKeyListener(null);
                    Dialog.SetOnShowListener(null);
                    Dialog.SetOnDismissListener(null);
                    Dialog.SetOnCancelListener(null);
                }
                catch (Exception)
                {

                }
            }
        }


        public override void OnDestroy()
        {
            _dialogElement?.OnDestory();
            UnRegisterEvent();
            base.OnDestroy();
        }


    }
}