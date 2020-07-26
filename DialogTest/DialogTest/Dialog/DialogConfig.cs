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
        /// 触碰遮罩层是否能关闭dialog，默认为true
        /// </summary>
        public bool IsCloseByTouchMask { get; set; } = true;

        /// <summary>
        /// 是否能被返回键和触摸mask的方式关闭，默认为true
        /// </summary>
        public bool IsCanCancel { get; set; } = true;

        /// <summary>
        /// 默认为false，若为true，则不渲染遮罩层，如此也就不会再拦截遮罩层的touch事件。       
        /// </summary>
        public bool NotTouchModal { get; set; }

        /// <summary>
        /// X偏移距离
        /// </summary>
        public double XOffset { get; set; } 

        /// <summary>
        /// Y偏移距离
        /// </summary>
        public double YOffset { get; set; }

        /// <summary>
        /// 对话框动画效果，可在对应平台xml中编写 
        /// 安卓参考：Dialog_popup Dialog_fade Dialog_slide_top Dialog_slide_bottom Dialog_tooltip Dialog_grow_fade
        /// </summary>
        public string DialogAnimationConfig { get; set; } = "Dialog_slide_bottom";
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
