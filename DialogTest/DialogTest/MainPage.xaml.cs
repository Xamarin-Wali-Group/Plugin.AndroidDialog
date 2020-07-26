using Box.Plugs.Dialog;
using DialogTest.DialogTestView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DialogTest
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var dialogFac = DependencyService.Get<IUserDialogsFactory>();
            var dialog = dialogFac.CreateDialog(DialogType.Confirm, new ConfirmMsg()
            {
                Msg = "确定要这么做啊？如果你这么做，那么将会发生我也不知道的事情",
            });
            var result = await dialog.ShowAsync();
            if (result == "是")
            {

            }
            dialogFac.Toast("2233");            
        }

        private void Btn_Toast(object sender, EventArgs e)
        {
            var dialogFac = DependencyService.Get<IUserDialogsFactory>();
            dialogFac.Toast("34444");
        }

        private void Btn_Pupup(object sender, EventArgs e)
        {
            var dialogFac = DependencyService.Get<IUserDialogsFactory>();
            var dialog=dialogFac.PopupView(sender as View, new TestPopupView(), null, null) ;
            dialog.Show();
        }
    }
}
