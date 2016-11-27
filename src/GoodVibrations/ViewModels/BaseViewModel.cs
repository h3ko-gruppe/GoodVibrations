using System;
using GoodVibrations.ViewModels.ItemViewModels;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace GoodVibrations.ViewModels
{
    public abstract class BaseViewModel : ReactiveObject
    {
        public BaseViewModel()
        {
            ToolBarItems = new ReactiveList<ActionItemViewModel>();
        }

        [Reactive]
        public string Title { get; set; }

        public ReactiveList<ActionItemViewModel> ToolBarItems { get; }

        protected virtual void CreateToolBarItems()
        {

        }

        public virtual void Init(object parameters)
        {

        }
    }
}
