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




        public Xamarin.Forms.View ContentView { get; private set; }
      

        /// <summary>
        /// 增加此构造，解决unable to activate instance of type 。。。
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public DialogInstance(IntPtr a, JniHandleOwnership b) : base(a, b)
        {
        }



        public DialogInstance(BaseDialogFragment2 dialogFragment, FragmentManager fragmentManage,TaskCompletionSource<string> misson)
        {
            this._dialogFragment = dialogFragment;
            this._fragmentManage = fragmentManage;
            this._misson = misson;
        }


        public void SetTaskMissonResult(string result)
        {
            _misson?.SetResult(result);
            Close();
        }





        /// <summary>
        /// 用户打开之前添加回调事件
        /// </summary>
        void OpenDialog()
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
            OpenDialog();
        }


        /// <summary>
        /// 打开Dialog,异步等待结果
        /// </summary>
        /// <returns></returns>
        public async Task<string> ShowAsync()
        {
            
            OpenDialog();
            if (_dialogResult!=null)
            {
                //var result = await _dialogResult.Mission.Task;
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



   

        #region 释放资源



        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);               
        }


        #endregion


    }

}