using System;
using System.Collections.Generic;
using System.Reactive;
using GoodVibrations.Extensions;
using ReactiveUI;
using Xamarin.Forms;

namespace GoodVibrations.Pages
{
	public partial class ContactsPage
	{
		public ContactsPage()
		{
			InitializeComponent();

			this.AutoWireViewModel();

			this.WhenActivated(dispose =>
		 	{
				 dispose(this.BindToTitle(ViewModel));

				dispose(ViewModel.SelectContact.RegisterHandler(async input =>
			   {
					await Navigation.PushAsync(new ContactsPage()).ConfigureAwait(false);
				   input.SetOutput(Unit.Default);
			   }));
			 });
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			ViewModel.OnAppear();
		}
	}
}
