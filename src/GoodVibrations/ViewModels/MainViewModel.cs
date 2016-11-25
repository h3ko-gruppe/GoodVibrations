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
            MenuItems = new ReactiveList<BaseItemViewModel>();

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

        public ReactiveList<BaseItemViewModel> MenuItems { get; }

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

        private Task LoadData()
        {
            // demo
            MenuItems.Add(new NotificatorItemViewModel() { Name = "Notificator 1" });
            MenuItems.Add(new PhoneCallTemplateItemViewModel() { Name = "PhoneCall 1" });
            MenuItems.Add(new NotificatorItemViewModel() { Name = "Notificator 2" });
            MenuItems.Add(new PhoneCallTemplateItemViewModel() { Name = "PhoneCall 2" });
            MenuItems.Add(new NotificatorItemViewModel() { Name = "Notificator 3" });
            MenuItems.Add(new PhoneCallTemplateItemViewModel() { Name = "PhoneCall 3" });
            MenuItems.Add(new NotificatorItemViewModel() { Name = "Notificator 4" });
            MenuItems.Add(new PhoneCallTemplateItemViewModel() { Name = "PhoneCall 4" });

            Title = "Overview";

            return Task.FromResult(true);
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
