using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Box.Plugs.Dialog
{
    public class DialogConfig
    {
        public DialogBlurConfig BlurConfig { get; set; }     

        public DialogAnimation DialogAnimation { get; set; } = DialogAnimation.PopupIn_PopupOut;

        public DialogPosition DialogPosition { get; set; } = DialogPosition.Center;
        /// <summary>
        /// 遮罩层透明度,默认为.4
        /// </summary>
        public float DimAmount { get; set; } = .4f;

        public Color BackgroundColor { get; set; } = Color.Transparent;

        /// <summary>
        /// 触碰Mask是否能关闭dialog，默认为true
        /// </summary>
        public bool IsCloseByTouchMask { get; set; } = true;

        /// <summary>
        /// 是否能被返回键和触摸mask的方式关闭，默认为true
        /// </summary>
        public bool IsCanCancel { get; set; } = true;
       
        /// <summary>
        /// 默认为false，若为true，则Mask会消失，则不再拦截Mask的touch事件。       
        /// </summary>
        public bool NotTouchModal { get; set; }

        /// <summary>
        /// X偏移距离
        /// </summary>
        public float XOffset { get; set; } 

        /// <summary>
        /// Y偏移距离
        /// </summary>
        public float YOffset { get; set; }

        public DialogConfig()
        {

        }
    }

    public enum DialogPosition
    {
        Left = 3,
        Right = 5,
        Top = 48,
        Center = 17,
        Buttom = 80,
        Custom,
        ToastDefault
    }

    public enum DialogAnimation
    {
        Custom,
        PopupIn_PopupOut,
        FadeIn_FadeOut,
        SlideInTop_SlideOutTop,
        SlideInButton_SlideOutButton,
        Tooltip,
        Grow_fade_in_Shrink_Fadeout
    }

    public class DialogBlurConfig 
    {

        public int BlurRadius { get; set; } = 5;

        public float BmpScale { get; set; } = .25f;

        public int FadeDuration { get; set; } = 350;
    }
}
