using Box.Plugs.Dialog;
using DialogTest.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DialogTest.DialogTestView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ToastView : ContentView,IDialogElement
    {
        public ToastView()
        {
            InitializeComponent();
        }

        public IDialogResult DialogResult { get ; set ; }

        public void OnClosed()
        {
            
        }

        public void OnCreated(IDialogMsg dialogMsg)
        {
            this.lbl.Text = dialogMsg.Msg;
        }

        public void OnDestory()
        {
           
        }

        public void OnShowed()
        {
            
        }
    }
}