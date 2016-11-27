using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using GoodVibrations.ViewModels.ItemViewModels;
using Plugin.Contacts;
using ReactiveUI;
using System.Reactive.Linq;
using Plugin.Contacts.Abstractions;

namespace GoodVibrations.ViewModels
{
	public class ContactsViewModel : BaseViewModel
	{
		public ContactsViewModel()
		{
            MenuItems = new ReactiveList<ContactItemViewModel>();
			MenuItems.ChangeTrackingEnabled = true;
			ItemSelected = ReactiveCommand.CreateFromTask<ContactItemViewModel, Unit>(OnItemSelected);
			SelectContact = new Interaction<Contact, Unit>();
		}

		public ReactiveList<ContactItemViewModel> MenuItems { get; }
		public ReactiveCommand ItemSelected { get; }

		public Interaction<Contact, Unit> SelectContact { get; }

		public void OnAppear()
		{
			LoadData().ConfigureAwait(false);
		}

		private async Task LoadData()
		{
			Title = "Contact";

			MenuItems.Clear();

			if (await CrossContacts.Current.RequestPermission())
			{
				CrossContacts.Current.PreferContactAggregation = false;

				if (CrossContacts.Current.Contacts == null)
					return;

                var contacts = CrossContacts.Current.Contacts.ToArray();

                MenuItems.AddRange(contacts.Where(x => !string.IsNullOrWhiteSpace(x.DisplayName))
                                   .OrderBy(x => x.DisplayName)
                                   .Select(item => new ContactItemViewModel() { Title = item.DisplayName, SelectedCommand = ItemSelected, Contact = item }));

                // forms workaround
                this.RaisePropertyChanged(nameof(MenuItems));
			}
		}

		#region Command Handler
		private async Task<Unit> OnItemSelected(ContactItemViewModel selectedItem)
		{
            if (selectedItem != null)
                await SelectContact.Handle(selectedItem.Contact);

			return Unit.Default;
		}
		#endregion
	}
}
