using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using GoodVibrations.Extensions;
using GoodVibrations.ViewModels;
using ReactiveUI;
using Xamarin.Forms;

namespace GoodVibrations.Pages
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.AutoWireViewModel();

            this.WhenActivated(dispose =>
            {
                dispose(this.BindToTitle(ViewModel));

                dispose(ViewModel.WhenAnyValue(x => x.MenuItems)
                     .ObserveOn(RxApp.MainThreadScheduler)
                        .Subscribe(newValue => ListView.ItemsSource = newValue));

                dispose(ViewModel.ShowSelectedNotificator.RegisterHandler(OnShowNotificator));

                dispose(ViewModel.ShowSelectedPhoneCallTemplate.RegisterHandler(async phoneCallTemplate =>
                {
                    await Navigation.PushAsync(new PhoneCallTemplatePage(phoneCallTemplate.Input)).ConfigureAwait(false);
                    phoneCallTemplate.SetOutput(Unit.Default);
                }));
            });
        }

        private async Task OnShowNotificator(InteractionContext<ViewModels.ItemViewModels.NotificatorItemViewModel, Unit> notificator)
        {
            await Navigation.PushAsync(new EditNotificatorPage(notificator.Input)).ConfigureAwait(false);
            notificator.SetOutput(Unit.Default);
        }
   }
}
