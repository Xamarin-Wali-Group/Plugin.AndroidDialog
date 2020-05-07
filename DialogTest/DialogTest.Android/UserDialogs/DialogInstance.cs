using System;
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
using DialogTest.Droid.UserDialogs;
using DialogTest.Dialog;

namespace BoxApp.Droid.DroidRender.UserDialogs
{
    public class DialogInstance : Java.Lang.Object, IDialog
    {
        private BaseDialogFragment2 _dialogFragment;
        private FragmentManager _fragmentManage;
        private TaskCompletionSource<string> _misson;




        public Xamarin.Forms.View DialogView { get; private set; }


        /// <summary>
        /// 增加此构造，解决unable to activate instance of type 。。。
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public DialogInstance(IntPtr a, JniHandleOwnership b) : base(a, b)
        {
        }



        public DialogInstance(BaseDialogFragment2 dialogFragment, 
            FragmentManager fragmentManage, 
            Xamarin.Forms.View dialogView,
            TaskCompletionSource<string> misson = null)
        {
            this._dialogFragment = dialogFragment;
            this._fragmentManage = fragmentManage;
            this._misson = misson;
            this.DialogView = dialogView;
        }


        public void SetTaskMissonResult(string result)
        {
            _misson?.SetResult(result);
            Close();
        }


        void ShowDialog()
        {
            if (_dialogFragment.IsAdded)
            {
                return;
            }
            _dialogFragment.Show(_fragmentManage, null);
        }

        /// <summary>
        /// 打开Dialog
        /// </summary>
        public void Show()
        {
            ShowDialog();
        }


        /// <summary>
        /// 打开Dialog,异步等待结果
        /// </summary>
        /// <returns></returns>
        public async Task<string> ShowAsync()
        {

            ShowDialog();
            if (_misson != null)
            {
                var result = await _misson.Task;
                return result;
            }
            else
            {
                return null;
            }           
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


        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            DialogView = null;
            base.Dispose(disposing);
        }


    }

}