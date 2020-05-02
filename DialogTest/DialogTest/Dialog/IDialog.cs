using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Box.Plugs.Dialog
{
    public interface IDialog:IDisposable
    {      

        /// <summary>
        /// 对话框关闭事件
        /// </summary>
         event Action DialogDismissEvent;

        /// <summary>
        /// 对话框取消事件
        /// </summary>
         event Action DialogCancelEvent;

        /// <summary>
        /// Dialog出现事件
        /// </summary> 
         event Action DialogShowEvent; 

        Xamarin.Forms.View ContentView { get;  }

        /// <summary>
        /// 打开对话框
        /// </summary>
        void Show();

        /// <summary>
        ///  按钮响应可等待任务
        /// </summary>
        /// <param name="result"></param>
        void SetTaskMissonResult(string result);

        /// <summary>
        /// 打开对话框，同时启动可等待任务，等待用户的响应
        /// </summary>
        /// <returns></returns>

        Task<string> ShowAsync();

        /// <summary>
        /// 关闭对话框
        /// </summary>
        void Close();

    
    }

   
}
