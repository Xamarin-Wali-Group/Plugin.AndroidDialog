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

        public void OnCreated(IDialogMsg dialogMsg)
        {

            var confirmMsg = dialogMsg as ConfirmMsg;
            this.msg.Text = dialogMsg.Msg;
            this.noBtn.Text = confirmMsg.NoBtn;
            this.okBtn.Text = confirmMsg.OkBtn;
        }

        public void OnDestory()
        {

        }

        public void OnShowed()
        {
            Console.WriteLine();
        }

        private void Cliked_Btn(object sender, EventArgs e)
        {
            this.DialogResult.SetResult(((Label)sender).Text);
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            return base.OnMeasure(widthConstraint * .7, heightConstraint);
        }

        public void OnClosed()
        {
            var a = this.Width;
            var b = this.boxTest.Width;
        }
    }

    public class ConfirmMsg : IDialogMsg
    {
        public string Msg { get; set; }

        public string OkBtn { get; set; } = "是";

        public string NoBtn { get; set; } = "否";
    }

}