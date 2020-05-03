﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using DialogFragment = Android.Support.V4.App.DialogFragment;
using Box.Plugs.Dialog;

namespace BoxApp.Droid.DroidRender.UserDialogs
{
    public class DialogInstance : Java.Lang.Object, IDialog, IDialogInterfaceOnShowListener,
        IDialogInterfaceOnDismissListener, IDialogInterfaceOnCancelListener
    {
        private BaseDialogFragment _dialogFragment;
        private FragmentManager _fragmentManage;
        private TaskCompletionSource<string> _mission;
 

        public event Action DialogShowEvent;

        public Xamarin.Forms.View ContentView { get;private set; }

        /// <summary>
        /// 对话框关闭事件
        /// </summary>
        public event Action DialogDismissEvent;

        /// <summary>
        /// 对话框取消事件
        /// </summary>
        public event Action DialogCancelEvent;


        /// <summary>
        /// 增加此构造，解决unable to activate instance of type 。。。
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public DialogInstance(IntPtr a, JniHandleOwnership b) : base(a, b)
        {
        }

        public DialogInstance(BaseDialogFragment dialogFragment, FragmentManager fragmentManage,
            Xamarin.Forms.View view)
        {
            this._dialogFragment = dialogFragment;
            this._fragmentManage = fragmentManage;          
            this.DialogDismissEvent += DestoryHasExistDialog;
            this.ContentView = view;
        }

        public DialogInstance(BaseDialogFragment dialogFragment, FragmentManager fragmentManage)
        {
            this._dialogFragment = dialogFragment;
            this._fragmentManage = fragmentManage;
        }


        public void SetTaskMissonResult(string result)
        {
            _mission.SetResult(result);
            Close();
        }





        /// <summary>
        /// 用户打开之前添加回调事件
        /// </summary>
        void OpenDialogAddListener()
        {
            if (_dialogFragment.IsAdded)
            {
                return;
            }
            InitDialogShowEvent();
            InitDialogDismissEvent();
            InitDialogCancelEvent();
            InitDialogNativeClicked();
      
            _dialogFragment.Show(_fragmentManage,null);
        }


        /// <summary>
        /// 打开Dialog
        /// </summary>
        public void Show()
        {
            OpenDialogAddListener();
        }


        /// <summary>
        /// 打开Dialog,异步等待结果
        /// </summary>
        /// <returns></returns>
        public async Task<string> ShowAsync()
        {
            _mission = new TaskCompletionSource<String>();
            OpenDialogAddListener();
            var result = await _mission.Task;
            return result;
        }



        /// <summary>
        /// 关闭Dialog
        /// </summary>
        public void Close()
        {
            if (_dialogFragment.IsHidden)
            {
                return;
            }
            _dialogFragment.Dismiss();
        }



        #region 注入事件
        void InitDialogShowEvent()
        {
            _dialogFragment.OnActivityCreatedEvent += (dialog) =>
            {
                dialog.SetOnShowListener(this);
            };
        }

        void InitDialogDismissEvent()
        {
            _dialogFragment.OnActivityCreatedEvent += (dialog) =>
            {
                dialog.SetOnDismissListener(this);
            };
        }

        void InitDialogCancelEvent()
        {
            _dialogFragment.OnActivityCreatedEvent += (dialog) =>
            {
                dialog.SetOnCancelListener(this);
            };
        }

        void InitDialogNativeClicked()
        {
            if (_dialogFragment.IsNative)
            {
                Xamarin.Forms.MessagingCenter.Subscribe<NativeDialogBtnClickListener, string>
                 (this, null, (sender, result) =>
                 {
                     SetTaskMissonResult(result);
                     Xamarin.Forms.MessagingCenter.Unsubscribe<NativeDialogBtnClickListener, string>(this,
                         null);
                 });
            }
        }

        public void OnShow(IDialogInterface dialog)
        {
            DialogShowEvent?.Invoke();
        }

        public void OnDismiss(IDialogInterface dialog)
        {
            DialogDismissEvent?.Invoke();
        }

        public void OnCancel(IDialogInterface dialog)
        {
            
            DialogCancelEvent?.Invoke();
        }


        #endregion

        #region 消除事件

        #endregion

        #region  关闭已经打开的Dialog,消除缓存
        /// <summary>
        /// 关闭已经打开的Dialog,消除缓存
        /// </summary>
        void DestoryHasExistDialog()
        {
            _dialogFragment.DismissAllowingStateLoss();
            _fragmentManage.BeginTransaction().Remove(_dialogFragment);
        }
        #endregion


        #region 释放资源



        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            DestoryHasExistDialog();
            _mission = null;
        }


        #endregion


    }
}