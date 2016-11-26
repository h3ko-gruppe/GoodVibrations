using System;
using GoodVibrations.ViewModels.ItemViewModels;
using ReactiveUI.Fody.Helpers;

namespace GoodVibrations.ViewModels
{
    public class EditNotificatorViewModel : BaseViewModel
    {
        [Reactive]
        public NotificatorItemViewModel Notificator { get; private set; }

        public override void Init(object parameters)
        {
            Notificator = parameters as NotificatorItemViewModel;
        }
    }
}
