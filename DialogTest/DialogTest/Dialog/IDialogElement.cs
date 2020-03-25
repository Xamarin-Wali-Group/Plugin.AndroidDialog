using System;
using System.Collections.Generic;
using System.Text;

namespace Box.Plugs.Dialog
{
    /// <summary>
    /// 当View作为Dialog视图的时候，必须实现此接口
    /// </summary>
    public interface IDialogElement
    {      

        IDialog LwyDialog { get; set; }
        

        /// <summary>
        /// 获取该View默认的对话框配置
        /// </summary>
        /// <returns></returns>
        DialogConfig GetViewDefaultDialogConfig();

        /// <summary>
        /// 获取对象，配置文本
        /// </summary>
        /// <param name="dialogMsg"></param>
        void SetDialogMsg(DialogMsg dialogMsg);
    }

    
}
