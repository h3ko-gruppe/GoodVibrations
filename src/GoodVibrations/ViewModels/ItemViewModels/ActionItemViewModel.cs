using System;
using System.Windows.Input;
using ReactiveUI.Fody.Helpers;

namespace GoodVibrations.ViewModels.ItemViewModels
{
    public class ActionItemViewModel : BaseItemViewModel
    {
        [Reactive]
        public string Title { get; set; }

        [Reactive]
        public ICommand SelectedCommand { get; set; }
    }
}
