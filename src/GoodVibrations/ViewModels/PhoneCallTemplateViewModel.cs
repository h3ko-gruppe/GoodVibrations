using System;
using GoodVibrations.ViewModels.ItemViewModels;
using ReactiveUI.Fody.Helpers;

namespace GoodVibrations.ViewModels
{
    public class PhoneCallTemplateViewModel : BaseViewModel
    {
        [Reactive]
        public PhoneCallTemplateItemViewModel PhoneCallTemplate { get; set; }

        public override void Init(object parameters)
        {
            PhoneCallTemplate = parameters as PhoneCallTemplateItemViewModel;
        }
    }
}
