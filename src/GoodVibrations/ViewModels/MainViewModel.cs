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

        public MainViewModel(IPersistenceService persistence)
        {
            _persistence = persistence;

            MenuItems = new ReactiveList<SectionViewModel>();
            MenuItems.ChangeTrackingEnabled = true;

            // commands
            CreateNewPhoneCallTemplate = ReactiveCommand.CreateFromTask(OnShowNewPhoneCallTemplate);
            CreateNewNotificator = ReactiveCommand.CreateFromTask(OnCreateNewNotificator);
            ItemSelected = ReactiveCommand.CreateFromTask<BaseItemViewModel, Unit>(OnItemSelected);
            DeleteItem = ReactiveCommand.CreateFromTask<BaseItemViewModel,Unit>(OnDeleteItem);

            // interactions
            ShowSelectedNotificator = new Interaction<Models.Notification, Unit>();
            ShowSelectedPhoneCallTemplate = new Interaction<Models.PhoneCall, Unit>();

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
        #endregion

        public void OnAppear()
        {
            LoadData().ConfigureAwait(false);
        }

        private async Task LoadData()
        {
            Title = "Overview";

            MenuItems.Clear();
            // create sections
            _phoneCallTemplateSection = new SectionViewModel<PhoneCallTemplateItemViewModel>()
            {
                Title = "Phonecall templates"
            };

            _notificatorSection = new SectionViewModel<NotificationItemViewModel>()
            {
                Title = "Notificators"
            };

            //#region Demo
            //// demo
            //_notificatorSection.Items.Add(new NotificationItemViewModel() { Notification = new models.Notification() { Name = "Notificator 1" }, SelectedCommand = ItemSelected, DeleteCommand = DeleteItem });
            //_notificatorSection.Items.Add(new NotificationItemViewModel() { Notification = new models.Notification() { Name = "Notificator 2" }, SelectedCommand = ItemSelected, DeleteCommand = DeleteItem });
            //_notificatorSection.Items.Add(new NotificationItemViewModel() { Notification = new models.Notification() { Name = "Notificator 3" }, SelectedCommand = ItemSelected, DeleteCommand = DeleteItem });
            //_notificatorSection.Items.Add(new NotificationItemViewModel() { Notification = new models.Notification() { Name = "Notificator 4" }, SelectedCommand = ItemSelected, DeleteCommand = DeleteItem });

            //_phoneCallTemplateSection.Items.Add(new PhoneCallTemplateItemViewModel() { PhoneCall = new models.PhoneCall() { Name = "PhoneCall 1", DestinationNumber = "110", Icon = "dummy.png" }, SelectedCommand = ItemSelected, DeleteCommand = DeleteItem });
            //_phoneCallTemplateSection.Items.Add(new PhoneCallTemplateItemViewModel() { PhoneCall = new models.PhoneCall() { Name = "PhoneCall 2", DestinationNumber = "112", Icon = "dummy.png" }, SelectedCommand = ItemSelected, DeleteCommand = DeleteItem });
            //_phoneCallTemplateSection.Items.Add(new PhoneCallTemplateItemViewModel() { PhoneCall = new models.PhoneCall() { Name = "PhoneCall 3", DestinationNumber = "01721234567", Icon = "dummy.png" }, SelectedCommand = ItemSelected, DeleteCommand = DeleteItem });
            //_phoneCallTemplateSection.Items.Add(new PhoneCallTemplateItemViewModel() { PhoneCall = new models.PhoneCall() { Name = "PhoneCall 4", DestinationNumber = "030123456", Icon = "dummy.png" }, SelectedCommand = ItemSelected, DeleteCommand = DeleteItem });
            //#endregion

            IList<Models.Notification> notifications = null;
            IList<Models.PhoneCall> phoneCalls = null;

            await Task.Run(() =>
            {
                notifications = _persistence.Notification.LoadAll();
                phoneCalls = _persistence.PhoneCall.LoadAll();
            });

            foreach (var item in notifications)
                _notificatorSection.Items.Add(new NotificationItemViewModel() { Notification = item, SelectedCommand = ItemSelected, DeleteCommand = DeleteItem });

            foreach (var item in phoneCalls)
                _phoneCallTemplateSection.Items.Add(new PhoneCallTemplateItemViewModel() { PhoneCall = item, SelectedCommand = ItemSelected, DeleteCommand = DeleteItem });

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
                Title = "New Notificator",
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
