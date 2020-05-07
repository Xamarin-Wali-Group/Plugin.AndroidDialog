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
    public partial class TestDialogView : ContentView, IDialogElement
    {
        public TestDialogView()
        {
            InitializeComponent();
        }

        public IDialogResult DialogResult { get; set; }

        public void OnClosed()
        {

        }



        public void OnCreated(IDialogMsg dialogMsg)
        {
            var dMsg = dialogMsg as TestDilaogMsg;
            this.btn.Text = dMsg.BtnMsg;
            this.lbl.Text = dMsg.Msg;
        }

        public void OnDestory()
        {

        }

        public void OnShowed()
        {

        }

        private void btn_Clicked(object sender, EventArgs e)
        {
            DialogResult.SetResult(this.btn.Text);
        }

        private void btn_Clicked2(object sender, EventArgs e)
        {
            DialogResult.SetResult(this.lbl.Text);
        }
    }

    public class TestDilaogMsg : IDialogMsg
    {
        public string Msg { get; set; }

        public string BtnMsg { get; set; }
    }

}