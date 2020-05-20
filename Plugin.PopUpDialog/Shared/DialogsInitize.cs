using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Plugin.PopUpDialog.Shared
{
    public class DialogsInitize
    {

        private Dictionary<DialogType, Func<IDialogElement>> _dialogTypeViews;
        private Dictionary<DialogType, DialogConfig> _dialogTypeConfigs;
        private Func<View> _toastViewFunc;
        private DialogType? _tempType;
        private static DialogsInitize DialogsServerInstance;

        private DialogsInitize()
        {
            _dialogTypeViews = new Dictionary<DialogType, Func<IDialogElement>>();
            _dialogTypeConfigs = new Dictionary<DialogType, DialogConfig>();
        }




        public DialogsInitize MapDialogFromContentView(DialogType dialogType, Func<IDialogElement> viewCreator)
        {
            if (_dialogTypeViews.ContainsKey(dialogType))
            {
                return this;
            }
            _tempType = dialogType;
            _dialogTypeViews.Add(dialogType, viewCreator);
            return this;
        }

        public DialogsInitize MapDialogConfig(DialogConfig defaultDialogConfig)
        {
            if (!_tempType.HasValue)
            {
                return this;
            }
            _dialogTypeConfigs.Add(_tempType.Value, defaultDialogConfig);
            return this;
        }

        public DialogsInitize InitToast(Func<View> toastViewFunc)
        {
            if (toastViewFunc != null)
            {
                _toastViewFunc = toastViewFunc;
            }
            return this;
        }

        public static DialogsInitize Instance()
        {
            if (DialogsServerInstance == null)
            {
                DialogsServerInstance = new DialogsInitize();
            }
            return DialogsServerInstance;
        }

        public View GetInitDialogContentView(DialogType dialogType) 
        {
            return _dialogTypeViews.ContainsKey(dialogType) ? _dialogTypeViews[dialogType].Invoke() as View
                    : null;
        }

        public DialogConfig GetInitDialogConfig(DialogType dialogType)
        {           
            return _dialogTypeConfigs.ContainsKey(dialogType) ? _dialogTypeConfigs[dialogType] 
                : null;
        }

        public View GetInitToastView() 
        {
            return _toastViewFunc?.Invoke();
        }
    }


}
