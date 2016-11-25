using System;
using GoodVibrations.ViewModels.ItemViewModels;
using ReactiveUI.Fody.Helpers;

namespace GoodVibrations.ViewModels
{
    public class EditNotificatorViewModel : BaseViewModel
    {
        public EditNotificatorViewModel()
        {
        }

        [Reactive]
        public NotificatorItemViewModel Notificator { get; set; }
    }
}
