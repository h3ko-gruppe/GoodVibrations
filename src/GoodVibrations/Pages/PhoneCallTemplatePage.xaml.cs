using System;
using System.Collections.Generic;
using GoodVibrations.Extensions;
using GoodVibrations.ViewModels.ItemViewModels;
using Xamarin.Forms;

namespace GoodVibrations.Pages
{
    public partial class PhoneCallTemplatePage
    {
        public PhoneCallTemplatePage(PhoneCallTemplateItemViewModel template)
        {
            InitializeComponent();
            this.AutoWireViewModel();

            ViewModel.PhoneCallTemplate = template;
        }
    }
}
