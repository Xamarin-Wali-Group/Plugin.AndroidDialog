using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Box.Plugs.Dialog;
using Plugin.CurrentActivity;
using DialogFragment = Android.Support.V4.App.DialogFragment;
using System.Threading.Tasks;
using UserDialogs;
using Android;
using Xamarin.Forms.Platform.Android;

namespace BoxApp.Droid.DroidRender.UserDialogs
{
    public abstract class BaseDialogFragment : DialogFragment
    {

        public const string MessagingCenter_Tag = "nativeDialog";

        public event Action<Dialog> OnActivityCreatedEvent;

        public bool IsNative { get; protected set; }

        protected Window DWindow => this.Dialog.Window;
        protected Context _mContext;

        protected Point DPoint
        {
            get
            {
                return UserDialogsFactory.WindowPoint;
            }
        }

        protected Size _dialogSize;

        protected Xamarin.Forms.View _xfView;
        protected DialogConfig _dialogConfig;
        protected DialogMsg _dialogMsg;
        protected ImageView _blurView;

        public BaseDialogFragment(IntPtr a, Android.Runtime.JniHandleOwnership b)
        {

        }

        public BaseDialogFragment(Context context, Xamarin.Forms.View view, DialogConfig dialogConfig
            , DialogMsg dialogMsg)
        {
            if (view == null)
            {
                IsNative = true;
            }
            _xfView = view;
            _mContext = context;
            _dialogConfig = dialogConfig;
            _dialogMsg = dialogMsg;
        }

        #region 释放事件
        protected virtual void DisposeEvent()
        {

            if (Dialog != null)
            {
                try
                {
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
        #endregion

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            DisposeEvent();
            base.OnActivityCreated(savedInstanceState);
            OnActivityCreatedEvent?.Invoke(this.Dialog);
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (!IsNative)
            {
                //设定文本
                SetXFViewDialogMsgText(_dialogMsg);
                var xfSize = new Xamarin.Forms.Size();
                var droidView = _xfView.ConvertFormsToNative(_mContext);
                _dialogSize = SetXFDialogSize(ref xfSize);
                _xfView.Layout(new Xamarin.Forms.Rectangle(Xamarin.Forms.Point.Zero,
                    xfSize));
                return droidView;
            }
            else
            {
                return null;
            }
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            if (IsNative)
            {
                var dialog = SetNativeDialog();
                return dialog;
            }
            else
            {
                return base.OnCreateDialog(savedInstanceState);
            }

        }

        public override void OnStart()
        {
            base.OnStart();
            var attrs = Dialog.Window.Attributes;
            attrs.WindowAnimations = SetDialogAnimation();
            SetDialogWindowBGDrawable();
            SetDialogWindowPosition(attrs);
            SetDialogWindowSize(attrs);
            SetDialogWindowFlags(attrs);

            DWindow.Attributes = attrs;
            Dialog.SetCancelable(_dialogConfig.IsCloseByTouchMask);
            Dialog.SetOnKeyListener(new DialogBackKeyLister(_dialogConfig.IsLockBackKey));
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
            attrs.X = (int)Math.Ceiling(_dialogConfig.XOffset * UserDialogsFactory.Density);
            attrs.Y = (int)Math.Ceiling(_dialogConfig.YOffset * UserDialogsFactory.Density);
        }

        void SetDialogWindowSize(WindowManagerLayoutParams attrs)
        {
            if (_dialogSize != null)
            {
                attrs.Width = this._dialogSize.Width;
                attrs.Height = this._dialogSize.Height;
            }
        }

        void SetDialogWindowFlags(WindowManagerLayoutParams attrs)
        {
            if (_dialogConfig.NotTouchModal)
            {
                DWindow.ClearFlags(WindowManagerFlags.DimBehind);
                DWindow.AddFlags(WindowManagerFlags.NotTouchModal);
            }
            else
            {
                attrs.DimAmount = _dialogConfig.DimAmount;
            }
        }

        protected virtual void SetDialogWindowBGDrawable()
        {
            var andColor = _dialogConfig.BackgroundColor.ToAndroid();
            var dw = new ColorDrawable(andColor);
            DWindow.SetBackgroundDrawable(dw);
        }
        #endregion

        public override void OnAttach(Context context)
        {
            base.OnAttach(context);
            if (_dialogConfig.BlurConfig != null)
            {
                SetBlurWindowBackground(_dialogConfig.BlurConfig);
                _blurView.Animate().Alpha(1f).SetDuration(_dialogConfig.BlurConfig.FadeDuration)
                    .Start();
            }

        }

        public override void OnDetach()
        {
            base.OnDetach();
            if (_dialogConfig.BlurConfig != null)
            {
                var deView = CrossCurrentActivity.Current.Activity.Window.DecorView as ViewGroup;
                deView.RemoveView(_blurView);
                _blurView.Dispose();
                _blurView = null;
            }
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            DisposeEvent();
            _xfView = null;
        }




        /// <summary>
        /// 设置原生Dialog
        /// </summary>
        /// <returns></returns>
        protected abstract Dialog SetNativeDialog();

        /// <summary>
        /// 设置对话框的Size(安卓单位)
        /// </summary>
        /// <param name="xfSize"></param>
        /// <returns></returns>
        protected virtual Size SetXFDialogSize(ref Xamarin.Forms.Size xfSize)
        {
            xfSize = _xfView.Measure(DPoint.X, DPoint.Y).Request;
            var density = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density;
            int width = (int)Math.Ceiling(xfSize.Width * density);
            int height = (int)Math.Ceiling(xfSize.Height * density);
            _dialogSize = new Size(width, height);
            return _dialogSize;
        }

       

        protected virtual void SetBlurWindowBackground(DialogBlurConfig blurConfig)
        {
            var activitCapture = CaptureViewHelper.CaptureWindow(true, blurConfig.BmpScale);
            var blurBitmap = CaptureViewHelper.RsBlur(_mContext, activitCapture, blurConfig.BlurRadius);
            var decorView = CrossCurrentActivity.Current.Activity.Window.DecorView as ViewGroup;
            _blurView = new ImageView(_mContext);
            _blurView.Background = new BitmapDrawable(Resources, blurBitmap);
            _blurView.Alpha = 0;
            decorView.AddView(_blurView);
            blurBitmap.Dispose();
        }

        protected virtual int SetDialogAnimation()
        {
            int dialogAnimateResource = -1;
            switch (_dialogConfig.DialogAnimation)
            {
                case DialogAnimation.PopupIn_PopupOut:
                    dialogAnimateResource = DialogTest.Droid.Resource.Style.Dialog_popup;
                    break;
                case DialogAnimation.FadeIn_FadeOut:
                    dialogAnimateResource = DialogTest.Droid.Resource.Style.Dialog_fade;
                    break;
                case DialogAnimation.SlideInTop_SlideOutTop:
                    dialogAnimateResource = DialogTest.Droid.Resource.Style.Dialog_slide_top;
                    break;
                case DialogAnimation.SlideInButton_SlideOutButton:
                    dialogAnimateResource = DialogTest.Droid.Resource.Style.Dialog_slide_bottom;
                    break;
                case DialogAnimation.Tooltip:
                    dialogAnimateResource = DialogTest.Droid.Resource.Style.Dialog_tooltip;
                    break;
                case DialogAnimation.Grow_fade_in_Shrink_Fadeout:
                    dialogAnimateResource = DialogTest.Droid.Resource.Style.Dialog_grow_fade;
                    break;
                default:
                    break;
            }
            return dialogAnimateResource;
        }



        protected virtual void SetXFViewDialogMsgText(DialogMsg dialogMsg, Dialog dialog = null)
        {
            if (_xfView == null)
            {
                return;
            }
            if (_xfView is IDialogElement dialogEle)
            {
                dialogEle.SetDialogMsg(dialogMsg);
            }
        }


    }
}