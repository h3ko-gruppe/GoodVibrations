using System;
using System.Collections.Generic;
using System.Reactive;
using GoodVibrations.Extensions;
using ReactiveUI;
using Xamarin.Forms;

namespace GoodVibrations.Pages
{
	public partial class PhoneCallPage
	{
		public PhoneCallPage()
		{
			InitializeComponent();
			this.AutoWireViewModel();

            this.WhenActivated(dispose =>
             {
                 dispose(this.BindToTitle(ViewModel));

                 dispose(ViewModel.SelectContact.RegisterHandler(async contactItem =>
                 {
                     await Navigation.PushAsync(new ContactsPage(contactItem.Input));
                     contactItem.SetOutput(Unit.Default);
                }));
            });
		}
	}
}
