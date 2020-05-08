
using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.PopUpDialog.Shared
{
    /// <summary>
    /// 当View作为Dialog视图的时候，必须实现此接口
    /// </summary>
    public interface IDialogElement
    {
        IDialogResult DialogResult { get; set; }

        /// <summary>
        /// 对话框元素被构造时调用
        /// </summary>
        /// <param name="dialogMsg"></param>
        void OnCreated(IDialogMsg dialogMsg);

        /// <summary>
        /// dialog show后调用
        /// </summary>
        void OnShowed(); 

        /// <summary>
        /// dialog被关闭后调用
        /// </summary>
        void OnClosed();


        void OnDestory();
    }


}
