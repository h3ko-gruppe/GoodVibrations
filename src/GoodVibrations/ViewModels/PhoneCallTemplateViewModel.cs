using System;
using GoodVibrations.ViewModels.ItemViewModels;
using ReactiveUI.Fody.Helpers;

namespace GoodVibrations.ViewModels
{
    public class PhoneCallTemplateViewModel : BaseViewModel
    {
        public PhoneCallTemplateViewModel()
        {
        }

        [Reactive]
        public PhoneCallTemplateItemViewModel PhoneCallTemplate { get; set; }
    }
}
