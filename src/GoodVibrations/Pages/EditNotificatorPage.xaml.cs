using System;
using System.Collections.Generic;
using System.Reactive;
using GoodVibrations.Extensions;
using GoodVibrations.ViewModels.ItemViewModels;
using ReactiveUI;
using Xamarin.Forms;

namespace GoodVibrations.Pages
{
    public partial class EditNotificatorPage
    {
        public EditNotificatorPage(NotificatorItemViewModel notificator)
        {
            InitializeComponent();
            this.AutoWireViewModel();

            ViewModel.Notificator = notificator;
        }
    }
}
