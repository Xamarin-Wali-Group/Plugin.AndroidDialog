using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;

namespace DialogTest.Droid
{
    public class Test: MapRenderer
    {
        public Test(Context context) :base(context){ }

        protected override MarkerOptions CreateMarker(Pin pin)
        {
           
            return base.CreateMarker(pin);
        }
    }

}