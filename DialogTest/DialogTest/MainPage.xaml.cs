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
            var factory=DependencyService.Get<IUserDialogsFactory>();
            var dialog=factory.CreateDialog(DialogType.Confirm, new TestDilaogMsg() 
            {
                BtnMsg="测试按钮信息",
                Msg=null
            });
            var result=await dialog.ShowAsync();
            //dialog.Close();
            factory.Toast("2233");            
        }
    }
}
