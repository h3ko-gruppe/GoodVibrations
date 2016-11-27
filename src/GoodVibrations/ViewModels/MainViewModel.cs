using System;
using System.Reactive;
using System.Threading.Tasks;
using GoodVibrations.ViewModels.ItemViewModels;
using ReactiveUI;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using models = GoodVibrations.Models;
using GoodVibrations.Interfaces.Services;
using System.Collections.Generic;

namespace GoodVibrations.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
		private readonly IPersistenceService _persistence;

		private SectionViewModel<NotificationItemViewModel> _notificatorSection;
		private SectionViewModel<PhoneCallTemplateItemViewModel> _phoneCallTemplateSection;
		private SectionViewModel<ActionItemViewModel> _directCallSection;
        private List<IDisposable> _itemSubscriptions;

		public MainViewModel(IPersistenceService persistence)
		{
			_persistence = persistence;

            _itemSubscriptions = new List<IDisposable>();
            MenuItems = new ReactiveList<SectionViewModel>();
            MenuItems.ChangeTrackingEnabled = true;

			// commands
			CreateNewPhoneCallTemplate = ReactiveCommand.CreateFromTask(OnShowNewPhoneCallTemplate);
			CreateNewNotificator = ReactiveCommand.CreateFromTask(OnCreateNewNotificator);
			ItemSelected = ReactiveCommand.CreateFromTask<BaseItemViewModel, Unit>(OnItemSelected);
			DeleteItem = ReactiveCommand.CreateFromTask<BaseItemViewModel, Unit>(OnDeleteItem);

			// interactions
			ShowSelectedNotificator = new Interaction<Models.Notification, Unit>();
			ShowSelectedPhoneCallTemplate = new Interaction<Models.PhoneCall, Unit>();
			ShowPhoneCall = new Interaction<Unit, Unit>();

			CreateToolBarItems();
		}

		public ReactiveList<SectionViewModel> MenuItems { get; }

		#region Commands
		public ReactiveCommand CreateNewPhoneCallTemplate { get; }

		public ReactiveCommand CreateNewNotificator { get; }

		public ReactiveCommand ItemSelected { get; }
		public ReactiveCommand DeleteItem { get; }
		#endregion

		#region Interactions
		public Interaction<Models.Notification, Unit> ShowSelectedNotificator { get; }
		public Interaction<Models.PhoneCall, Unit> ShowSelectedPhoneCallTemplate { get; }
		public Interaction<Unit, Unit> ShowPhoneCall { get; }
		#endregion

		public void OnAppear()
		{
			LoadData().ConfigureAwait(false);
		}

		private async Task LoadData()
		{
			Title = "Overview";

            MenuItems.Clear();

            foreach (var item in _itemSubscriptions)
                item?.Dispose();

            _itemSubscriptions.Clear();

            // create sections
            _phoneCallTemplateSection = new SectionViewModel<PhoneCallTemplateItemViewModel>()
            {
                Title = "Phonecall templates"
            };

			_notificatorSection = new SectionViewModel<NotificationItemViewModel>()
            {
                Title = "Notificators"
            };

            _directCallSection = new SectionViewModel<ActionItemViewModel>()
            {
                Title = "Directcall"
            };

            IList<Models.Notification> notifications = null;
            IList<Models.PhoneCall> phoneCalls = null;
            // load items
            await Task.Run(() =>
            {
                notifications = _persistence.Notification.LoadAll();
                phoneCalls = _persistence.PhoneCall.LoadAll();
            });

            // map notifications
            foreach (var item in notifications)
            {
                _notificatorSection.Items.Add(new NotificationItemViewModel() { Notification = item, SelectedCommand = ItemSelected, DeleteCommand = DeleteItem });
                _itemSubscriptions.Add(item.WhenAnyValue(x => x.Active).Subscribe(_ => Task.Run(() => _persistence.Notification.InsertOrReplace(item))));
            }

            // map phone calls
            foreach (var item in phoneCalls)
                _phoneCallTemplateSection.Items.Add(new PhoneCallTemplateItemViewModel() { PhoneCall = item, SelectedCommand = ItemSelected, DeleteCommand = DeleteItem });

			_directCallSection.Items.Add(new ActionItemViewModel() { Title = "Do directcall", SelectedCommand = ItemSelected });

			MenuItems.Add(_directCallSection);
			MenuItems.Add(_phoneCallTemplateSection);
			MenuItems.Add(_notificatorSection);
		}

		protected override void CreateToolBarItems()
		{
			ToolBarItems.Clear();

			ToolBarItems.Add(new ActionItemViewModel()
			{
				Title = "New Tile",
				SelectedCommand = CreateNewPhoneCallTemplate
			});

			ToolBarItems.Add(new ActionItemViewModel()
			{
				Title = "New Notifier",
				SelectedCommand = CreateNewNotificator
			});
		}

		#region Command Handler
		private async Task<Unit> OnItemSelected(BaseItemViewModel selectedItem)
		{
			if (selectedItem is NotificationItemViewModel)
			{
				var notificationItem = (NotificationItemViewModel)selectedItem;
				await ShowSelectedNotificator.Handle(notificationItem.Notification);
			}
			else if (selectedItem is PhoneCallTemplateItemViewModel)
			{
				var phoneCallItem = (PhoneCallTemplateItemViewModel)selectedItem;
				await ShowSelectedPhoneCallTemplate.Handle(phoneCallItem.PhoneCall);
			}
			else if (selectedItem is ActionItemViewModel)
				await ShowPhoneCall.Handle(Unit.Default);

			return Unit.Default;
		}

		private async Task<Unit> OnDeleteItem(BaseItemViewModel selectedItem)
		{
			if (selectedItem is NotificationItemViewModel)
			{
				var notificationItem = (NotificationItemViewModel)selectedItem;
				_notificatorSection.Items.Remove(notificationItem);
				await Task.Run(() => _persistence.Notification.Delete(notificationItem.Notification));
			}
			else if (selectedItem is PhoneCallTemplateItemViewModel)
			{
				var phoneCallItem = (PhoneCallTemplateItemViewModel)selectedItem;
				_phoneCallTemplateSection.Items.Remove(phoneCallItem);
				await Task.Run(() => _persistence.PhoneCall.Delete(phoneCallItem.PhoneCall));
			}

			return Unit.Default;
		}

		private async Task OnCreateNewNotificator()
		{
			// show edit
			await ShowSelectedNotificator.Handle(null);
		}

		private async Task OnShowNewPhoneCallTemplate()
		{
			await ShowSelectedPhoneCallTemplate.Handle(null);
		}
		#endregion
	}
}
