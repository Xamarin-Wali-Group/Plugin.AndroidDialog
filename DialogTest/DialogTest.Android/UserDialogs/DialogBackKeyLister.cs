using System;
using Android.Content;
using Android.Runtime;
using Android.Views;

namespace BoxApp.Droid.DroidRender.UserDialogs
{
    public class DialogBackKeyLister : Java.Lang.Object, IDialogInterfaceOnKeyListener
    {
        private bool _isLockByBack;
        public DialogBackKeyLister(bool isLockByBack)
        {
            _isLockByBack = isLockByBack;
        }
        public DialogBackKeyLister(IntPtr handle, JniHandleOwnership transfer)
        {

        }
        public bool OnKey(IDialogInterface dialog, [GeneratedEnum] Keycode keyCode, KeyEvent e)
        {          
            if (keyCode == Keycode.Back)
            {
                if (!_isLockByBack)
                {
                    dialog.Cancel();
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
    }
}