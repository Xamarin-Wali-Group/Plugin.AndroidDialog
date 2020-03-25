using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Box.Plugs.Dialog
{
    public class DialogsServer
    {
        #region Toast
        public static Func<View> ToastDialogsFunc { get; private set; }
        public static void SetDefalutDialog_Toast(Func<View> toastDialogsFunc)
        {
            ToastDialogsFunc = toastDialogsFunc;
        }
        #endregion
      
        #region LoadDialog
        public static Func<View> LoadDialogsFunc { get; private set; }
        public static void SetDefalutDialog_Load(Func<View> loadDialogs)
        {
            LoadDialogsFunc = loadDialogs;
        }
        #endregion

        #region ButtonDialog
        public static Func<View> ButtonDialogsFunc { get; private set; }
        public void SetDefalutDialog_Button(Func<View> buttonDialogs)
        {
            ButtonDialogsFunc = buttonDialogs;
        }
        #endregion     

        public static IUserDialogsFactory Instance
        {
            get
            {
                var service = DependencyService.Get<IUserDialogsFactory>();
                return service;
            }
        }


        public static void ClearToast() 
        {
            
        
        }

        public static void ClearDialog() 
        {
        
        }

        public static void ClearDialog(IDialog lwyDialog)
        {

        }
    }

   
}
