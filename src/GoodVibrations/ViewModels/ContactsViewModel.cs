using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using GoodVibrations.ViewModels.ItemViewModels;
using Plugin.Contacts;
using ReactiveUI;

namespace GoodVibrations.ViewModels
{
	public class ContactsViewModel : BaseViewModel
	{
		private SectionViewModel<ActionItemViewModel> _contactsSection;

		public ContactsViewModel()
		{
			MenuItems = new ReactiveList<SectionViewModel>();
			MenuItems.ChangeTrackingEnabled = true;
			ItemSelected = ReactiveCommand.CreateFromTask<BaseItemViewModel, Unit>(OnItemSelected);
			SelectContact = new Interaction<Unit, Unit>();
		}

		public ReactiveList<SectionViewModel> MenuItems { get; }
		public ReactiveCommand ItemSelected { get; }

		public Interaction<Unit, Unit> SelectContact { get; }

		public void OnAppear()
		{
			LoadData().ConfigureAwait(false);
		}

		private async Task LoadData()
		{
			Title = "Contact";

			MenuItems.Clear();

			// create sections
			_contactsSection = new SectionViewModel<ActionItemViewModel>()
			{
				Title = "oO Contacts Oo"
			};

			if (await CrossContacts.Current.RequestPermission())
			{
				CrossContacts.Current.PreferContactAggregation = false;

				if (CrossContacts.Current.Contacts == null)
					return;

				var contacts = CrossContacts.Current.Contacts.ToList();

				foreach (var item in contacts)
					_contactsSection.Items.Add(new ActionItemViewModel() { Title = item.DisplayName, SelectedCommand = ItemSelected });
			}

			MenuItems.Add(_contactsSection);

		}

		#region Command Handler
		private async Task<Unit> OnItemSelected(BaseItemViewModel selectedItem)
		{
			//if (selectedItem is ActionItemViewModel)
			//	await SelectContact.Handle(Unit.Default);

			return Unit.Default;
		}
		#endregion
	}
}
