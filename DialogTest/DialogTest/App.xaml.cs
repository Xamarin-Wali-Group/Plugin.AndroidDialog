﻿using Box.Plugs.Dialog;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DialogTest
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DialogsInitize dialogsServer = DialogsInitize.Instance();
            dialogsServer.MapDialogFromContentView(DialogType.Confirm,
                () => new DialogTestView.TestDialogView()
                 )
                .MapDialogConfig(new DialogConfig()
                {
                    IsCloseByTouchMask = false
                });


            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
