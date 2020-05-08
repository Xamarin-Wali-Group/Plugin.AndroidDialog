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
using DialogTest.Dialog;

namespace DialogTest.Droid.UserDialogs
{
    public class DialogResultManager
    {
        TaskCompletionSource<string> _mission;
        IDialogResult _dialogResult;

        public void Build() 
        {            
            _mission = new TaskCompletionSource<string>();
            _dialogResult = new DialogResult(_mission);
        }

        public TaskCompletionSource<string> GetResultMission() 
        {
            return _mission;
        }

        public IDialogResult GetDialogResult() 
        {
            return _dialogResult;
        }
       
    }


    public class DialogResult : IDialogResult
    {
        TaskCompletionSource<string> _mission;
     
        public DialogResult(TaskCompletionSource<string> mission)
        {
            _mission = mission;
        }

       

        private void MessagingToCloseDialog()
        {
            Xamarin.Forms.MessagingCenter.Send<DialogResult>(this, "close");
        }

        public void CloseDialog()
        {
            MessagingToCloseDialog();
        }

        public void SetResult(string resultMsg)
        {                        
            if (_mission.TrySetResult(resultMsg))
            {
                MessagingToCloseDialog();
            }
        }
    }
}