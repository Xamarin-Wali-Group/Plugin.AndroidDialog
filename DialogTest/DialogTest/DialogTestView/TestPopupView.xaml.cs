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
    public partial class TestPopupView : ContentView
    {
        public TestPopupView()
        {
            InitializeComponent();            
            List<string> sts = new List<string>();
            for (int i = 0; i < 7; i++)
            {
                sts.Add($"第{i+1}项");
            }
            BindableLayout.SetItemsSource(test, sts);
            
        }
    }
}