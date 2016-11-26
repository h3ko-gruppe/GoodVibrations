using ReactiveUI.Fody.Helpers;

namespace GoodVibrations.ViewModels.ItemViewModels
{
    public class NotificatorItemViewModel : SelectableItemViewModel
    {
        [Reactive]
        public string Name { get; set; }
    }
}
