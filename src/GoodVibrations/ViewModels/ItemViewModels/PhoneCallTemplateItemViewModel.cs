using System;
using GoodVibrations.Models;
using ReactiveUI.Fody.Helpers;

namespace GoodVibrations.ViewModels.ItemViewModels
{
    public class PhoneCallTemplateItemViewModel : SelectableItemViewModel
    {
        [Reactive]
        public PhoneCall PhoneCall { get; set; }
    }
}
