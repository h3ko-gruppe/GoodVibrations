using System;
using System.Collections.Generic;
using System.Reactive;
using GoodVibrations.Extensions;
using ReactiveUI;
using Xamarin.Forms;

namespace GoodVibrations.Pages
{
    public partial class LoginPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.AutoWireViewModel();

            this.WhenActivated(dispose =>
            {
                dispose(this.BindToTitle(ViewModel));

                dispose(ViewModel.ShowMain.RegisterHandler(interaction =>
                {
                    App.Current.MainPage = new NavigationPage (new MainPage());
                    interaction.SetOutput(Unit.Default);
                }));

                dispose(ViewModel.ShowRegistration.RegisterHandler(interaction =>
                {
                    Navigation.PushAsync(new RegistrationPage());
                    interaction.SetOutput(Unit.Default);
                }));
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel?.CheckForCredentials();
        }
    }
}
