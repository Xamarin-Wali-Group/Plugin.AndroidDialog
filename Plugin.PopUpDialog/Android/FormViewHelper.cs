﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using View= Android.Views.View;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Android.Views;

namespace Plugin.PopUpDialog.Android
{
    public static class FormViewHelper
    {
        public static View ConvertFormsToNative(this Xamarin.Forms.View view, Context context)
        {
            var vRenderer = view.GetRenderer();
            if (vRenderer == null)
            {
                Platform.SetRenderer(view, Platform.CreateRendererWithContext(view, context));
                vRenderer = view.GetRenderer();
            }
            var nativeView = vRenderer.View;
            nativeView.RemoveFromParent();
            vRenderer.Tracker.UpdateLayout();
            return nativeView;
        }

        public static View ConvertFormsToNative(this Xamarin.Forms.View view)
        {
            var vRenderer = view.GetRenderer();
            var nativeView = vRenderer.View;
            return nativeView;
        }
    }
}