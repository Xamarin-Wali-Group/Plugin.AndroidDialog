using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Box.Plugs.Dialog
{
    public interface IDialog:IDisposable
    {      

        

        Xamarin.Forms.View DialogView { get;  }

        /// <summary>
        /// 打开对话框
        /// </summary>
        void Show();

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
