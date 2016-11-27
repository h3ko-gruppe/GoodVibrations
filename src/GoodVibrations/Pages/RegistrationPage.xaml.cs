using System;
using System.Collections.Generic;
using System.Reactive;
using GoodVibrations.Extensions;
using ReactiveUI;
using Xamarin.Forms;

namespace GoodVibrations.Pages
{
    public partial class RegistrationPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
            this.AutoWireViewModel();

            this.WhenActivated(dispose =>
            {
                dispose(this.BindToTitle(ViewModel));
                
                dispose(ViewModel.ShowLogin.RegisterHandler(interaction =>
                {
                    Navigation.PopAsync();
                    interaction.SetOutput(Unit.Default);
                }));
            });
        }
    }
}
