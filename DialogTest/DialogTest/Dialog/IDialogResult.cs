using System;
using System.Collections.Generic;
using System.Text;

namespace DialogTest.Dialog
{
    public interface IDialogResult
    {
        /// <summary>
        /// 关闭Dialog
        /// </summary>
        void CloseDialog();

        /// <summary>
        /// 设置对话框结果，同时关闭Dialog
        /// </summary>
        /// <param name="resultMsg"></param>
        void SetResult(string resultMsg);
    }
}
