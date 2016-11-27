using System;
using System.Collections.Generic;
using GoodVibrations.ViewModels.ItemViewModels;
using ReactiveUI;
using Xamarin.Forms;
using GoodVibrations.Extensions;
using System.Reactive.Linq;
using ReactiveUI.XamForms;

namespace GoodVibrations.Views.Cells
{
    public partial class NotificatorCell
    {
        public NotificatorCell()
        {
            InitializeComponent();

            this.WhenActivated(dispose =>
            {
                var viewModel = BindingContext as NotificationItemViewModel;

                if (viewModel == null)
                    return;

                dispose(this.SubscribeToTap(viewModel));
                dispose(this.SubscribeToDelete(viewModel));
            });
        }
    }
}
