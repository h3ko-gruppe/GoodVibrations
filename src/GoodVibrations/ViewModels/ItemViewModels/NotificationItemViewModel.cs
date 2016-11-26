using GoodVibrations.Models;
using ReactiveUI.Fody.Helpers;

namespace GoodVibrations.ViewModels.ItemViewModels
{
    public class NotificationItemViewModel : SelectableItemViewModel
    {
        [Reactive]
        public Notification Notification { get; set; }
    }
}
