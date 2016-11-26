using System;
using System.Collections.Generic;
using GoodVibrations.Extensions;
using GoodVibrations.ViewModels.ItemViewModels;
using ReactiveUI;
using Xamarin.Forms;

namespace GoodVibrations.Pages
{
    public partial class PhoneCallTemplatePage
    {
        public PhoneCallTemplatePage(PhoneCallTemplateItemViewModel template)
        {
            InitializeComponent();
            this.AutoWireViewModel(template);

            this.WhenActivated(dispose =>
            {
                dispose(this.BindToTitle(ViewModel));
                dispose(this.BindToToolBarItems(ViewModel));
            });
        }
    }
}
