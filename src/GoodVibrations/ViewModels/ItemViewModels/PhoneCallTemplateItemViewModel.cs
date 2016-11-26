using System;
using ReactiveUI.Fody.Helpers;

namespace GoodVibrations.ViewModels.ItemViewModels
{
    public class PhoneCallTemplateItemViewModel : BaseItemViewModel
    {
        [Reactive]
        public string Name { get; set;}
    }
}
