using System;
using GoodVibrations.Models;
using GoodVibrations.ViewModels.ItemViewModels;
using ReactiveUI.Fody.Helpers;

namespace GoodVibrations.ViewModels
{
    public class EditNotificatorViewModel : BaseViewModel
    {
        [Reactive]
        public Notification Notification { get; private set; }

        public override void Init(object parameters)
        {
            Notification = parameters as Notification;
        }
    }
}
