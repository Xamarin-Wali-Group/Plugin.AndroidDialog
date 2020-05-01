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
            TestDialogView testDialog = new TestDialogView();
            //打开一个自定义弹窗
            var dialog = DialogsServer.Instance.CustomDialog(testDialog,new DialogConfig() 
            {
                DialogPosition=DialogPosition.Center,
                DimAmount=.6f,
                DialogAnimation=DialogAnimation.PopupIn_PopupOut//弹窗打开动画
            });
             dialog.ShowDialog();
            //调试看看此View的值，就是弹窗View（TestDialogView）
            var view = dialog.ContentView;
        }
    }
}
