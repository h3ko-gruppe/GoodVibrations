using System;
using ReactiveUI.Fody.Helpers;

namespace GoodVibrations.ViewModels.ItemViewModels
{
    public class PhoneCallTemplateItemViewModel : SelectableItemViewModel
    {
        [Reactive]
        public string Name { get; set; }

        [Reactive]
        public string PhoneNumber { get; set; }

        [Reactive]
        public string ImagePath { get; set; }
    }
}
