using System;
using System.Reactive;
using System.Threading.Tasks;
using GoodVibrations.ViewModels.ItemViewModels;
using ReactiveUI;

namespace GoodVibrations.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            MenuItems = new ReactiveList<SectionViewModel>();
            MenuItems.ChangeTrackingEnabled = true;

            // commands
            CreateNewPhoneCallTemplate = ReactiveCommand.Create(OnShowNewPhoneCallTemplate);
            CreateNewNotificator = ReactiveCommand.Create(OnCreateNewNotificator);
            ItemSelected = ReactiveCommand.Create<BaseItemViewModel,Unit>(OnItemSelected);

            // interactions
            ShowNewPhoneCallTemplate = new Interaction<Unit, Unit>();
            ShowSelectedNotificator = new Interaction<NotificatorItemViewModel, Unit>();
            ShowSelectedPhoneCallTemplate = new Interaction<PhoneCallTemplateItemViewModel, Unit>();

            LoadData().ConfigureAwait(false);
        }

        public ReactiveList<SectionViewModel> MenuItems { get; }

        #region Commands
        public ReactiveCommand CreateNewPhoneCallTemplate { get; }

        public ReactiveCommand CreateNewNotificator { get; }

        public ReactiveCommand ItemSelected { get; }
        #endregion

        #region Interactions
        public Interaction<Unit, Unit> ShowNewPhoneCallTemplate { get; }
        public Interaction<NotificatorItemViewModel, Unit> ShowSelectedNotificator { get; }
        public Interaction<PhoneCallTemplateItemViewModel,Unit> ShowSelectedPhoneCallTemplate { get; }
        #endregion

        private async Task LoadData()
        {
            Title = "Overview";

            // create sections
            var phoneCallSection = new SectionViewModel<PhoneCallTemplateItemViewModel>()
            {
                Title = "Phonecall templates"
            };

            var notificatorsSection = new SectionViewModel<NotificatorItemViewModel>()
            {
                Title = "Notificators"
            };

            // demo
            notificatorsSection.Items.Add(new NotificatorItemViewModel() { Name = "Notificator 1" });
            notificatorsSection.Items.Add(new NotificatorItemViewModel() { Name = "Notificator 2" });
            notificatorsSection.Items.Add(new NotificatorItemViewModel() { Name = "Notificator 3" });
            notificatorsSection.Items.Add(new NotificatorItemViewModel() { Name = "Notificator 4" });

            phoneCallSection.Items.Add(new PhoneCallTemplateItemViewModel() { Name = "PhoneCall 1", PhoneNumber="110", ImagePath="dummy.png" });
            phoneCallSection.Items.Add(new PhoneCallTemplateItemViewModel() { Name = "PhoneCall 2", PhoneNumber = "112", ImagePath = "dummy.png" });
            phoneCallSection.Items.Add(new PhoneCallTemplateItemViewModel() { Name = "PhoneCall 3", PhoneNumber = "01721234567", ImagePath = "dummy.png" });
            phoneCallSection.Items.Add(new PhoneCallTemplateItemViewModel() { Name = "PhoneCall 4", PhoneNumber = "030123456", ImagePath = "dummy.png" });

            MenuItems.Add(phoneCallSection);
            MenuItems.Add(notificatorsSection);
        }

        #region Command Handler
        private Unit OnItemSelected(BaseItemViewModel selectedItem)
        {
            if (selectedItem is NotificatorItemViewModel)
                ShowSelectedNotificator.Handle((NotificatorItemViewModel)selectedItem);
            else if (selectedItem is PhoneCallTemplateItemViewModel)
                ShowSelectedPhoneCallTemplate.Handle((PhoneCallTemplateItemViewModel)selectedItem);

            return Unit.Default;
        }

        private void OnCreateNewNotificator()
        {
            // TODO: Show BarcodeScanner, Create notificator
            NotificatorItemViewModel createdNotificator = null;

            // show edit
            ShowSelectedNotificator.Handle(createdNotificator);
        }

        private void OnShowNewPhoneCallTemplate()
        {
            ShowNewPhoneCallTemplate.Handle(Unit.Default);
        }
        #endregion
    }
}
