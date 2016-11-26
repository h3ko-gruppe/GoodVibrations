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
        public EditNotificatorPage(Models.Notification notification)
        {
            InitializeComponent();
            this.AutoWireViewModel(notification);

            this.WhenActivated(dispose =>
            {
                dispose(this.BindToTitle(ViewModel));
                dispose(this.BindToToolBarItems(ViewModel));
                dispose(ViewModel.Close.RegisterHandler(async param =>
                {
                    await Navigation.PopAsync();
                    param.SetOutput(Unit.Default);
                }));
            });
        }
    }
}
