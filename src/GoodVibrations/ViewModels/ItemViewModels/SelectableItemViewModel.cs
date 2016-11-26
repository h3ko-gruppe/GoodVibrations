using System;
using System.Windows.Input;
using ReactiveUI.Fody.Helpers;

namespace GoodVibrations.ViewModels.ItemViewModels
{
    public abstract class SelectableItemViewModel : BaseItemViewModel
    {
        public SelectableItemViewModel()
        {
            Selectable = true;
            Deletable = true;
        }

        [Reactive]
        public bool Selectable { get; set; }

        [Reactive]
        public bool Deletable { get; set; }

        [Reactive]
        public ICommand SelectedCommand { get; set; }

        [Reactive]
        public ICommand DeleteCommand { get; set; }
    }
}
