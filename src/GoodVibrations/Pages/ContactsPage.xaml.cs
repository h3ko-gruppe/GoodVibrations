using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using GoodVibrations.Extensions;
using Plugin.Contacts.Abstractions;
using ReactiveUI;
using Xamarin.Forms;

namespace GoodVibrations.Pages
{
	public partial class ContactsPage
	{
        private readonly Action<Contact> _selectedCallback;
        public ContactsPage(Action<Contact> selectedCallback)
        {
            _selectedCallback = selectedCallback;

            InitializeComponent();

            this.AutoWireViewModel();

            ListView.ItemTemplate = new DataTemplate(() =>
            {
                var cell = new TextCell();
                cell.SetBinding(TextCell.TextProperty, "Contact.DisplayName");
                cell.TextColor = Color.Black;
                return cell;
            });

            this.WhenActivated(dispose =>
             {
             dispose(this.BindToTitle(ViewModel));

             dispose(ListView.Events()
                     .ItemSelected
                     .Select(x => x.SelectedItem)
                     .InvokeCommand(ViewModel.ItemSelected));

                 dispose(ViewModel.WhenAnyValue(x => x.MenuItems)
                         .Subscribe(items => ListView.ItemsSource = items));

                 dispose(ViewModel.SelectContact.RegisterHandler(async contactItem =>
                {
                     _selectedCallback?.Invoke(contactItem.Input);
                     await Navigation.PopAsync();
                     contactItem.SetOutput(Unit.Default);
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
