using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Plugin.PopUpDialog.Shared
{
    public interface IDialog : IDisposable
    {
        /// <summary>
        /// 对话框关闭事件
        /// </summary>
        event Action DialogDismissEvent;

        /// <summary>
        /// 对话框取消事件
        /// </summary>
        event Action DialogCancelEvent;

        View ContentView { get; }

        /// <summary>
        /// Dialog出现事件
        /// </summary> 
        event Action DialogShowEvent;

        void Show();

        Task<string> ShowAsync();

        void Close();


    }
}
