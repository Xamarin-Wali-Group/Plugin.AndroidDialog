using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.PopUpDialog.Android;
using Plugin.PopUpDialog.Shared;

namespace DialogTest.Droid.UserDialogs
{
    public class PopupDialogFragment : BaseDialogFragment2
    {
        BaseViewRect _baseViewRect;
        public PopupDialogFragment(Activity activity, Xamarin.Forms.View contentView, DialogConfig dialogConfig, IDialogMsg dialogMsg, 
             BaseViewRect baseViewRect,IDialogResult dialogResult = null)
            : base(activity, contentView, dialogConfig, dialogMsg, dialogResult)
        {
            this._baseViewRect = baseViewRect;
        }

        /// <summary>
        /// 根据方向选择
        /// </summary>
        /// <param name="attrs"></param>
        protected override void SetDialogWindowPosition(WindowManagerLayoutParams attrs)
        {
            base.SetDialogWindowPosition(attrs);
         
            int lastX=0, lastY = 0;
            int xoffset =(int)Math.Ceiling( _dialogConfig.XOffset *Density);
            int yoffset = (int)Math.Ceiling(_dialogConfig.YOffset * Density);
            switch (_dialogConfig.DialogPosition)
            {              
                case DialogPosition.Top:
                    lastX = _baseViewRect.X + xoffset;
                    lastY = _baseViewRect.Y-_dialogViewSize.Height - yoffset;
                    break;            
                case DialogPosition.Buttom:
                    lastX = _baseViewRect.X + xoffset;
                    lastY = _baseViewRect.Y+_baseViewRect.Height+ yoffset;
                    break;
                default:
                    lastX = _baseViewRect.X + xoffset;
                    lastY = _baseViewRect.Y + _baseViewRect.Height + yoffset;
                    break;
            }
            int differenceX = lastX + _dialogViewSize.Width - this.WindowSize.X;
            if (differenceX>0)
            {
                lastX = lastX - differenceX;
            }
            attrs.Gravity = GravityFlags.Top | GravityFlags.Left;
            attrs.X = lastX;
            attrs.Y = lastY;
        }
    }

    public struct BaseViewRect 
    {
        public int X { get; set; }

        public int Y { get; set; }

        public int Width { get; set; }

     
        public int Height { get; set; }

        public BaseViewRect(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

    }
}