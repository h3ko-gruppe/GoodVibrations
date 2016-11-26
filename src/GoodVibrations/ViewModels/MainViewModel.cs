using System;
using System.Reactive;
using System.Threading.Tasks;
using GoodVibrations.ViewModels.ItemViewModels;
using ReactiveUI;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace GoodVibrations.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private SectionViewModel<NotificatorItemViewModel> _notificatorSection;
        private SectionViewModel<PhoneCallTemplateItemViewModel> _phoneCallTemplateSection;

        public MainViewModel()
        {
            MenuItems = new ReactiveList<SectionViewModel>();
            MenuItems.ChangeTrackingEnabled = true;

            // commands
            CreateNewPhoneCallTemplate = ReactiveCommand.CreateFromTask(OnShowNewPhoneCallTemplate);
            CreateNewNotificator = ReactiveCommand.CreateFromTask(OnCreateNewNotificator);
            ItemSelected = ReactiveCommand.CreateFromTask<BaseItemViewModel, Unit>(OnItemSelected);
            DeleteItem = ReactiveCommand.Create<BaseItemViewModel,Unit>(OnDeleteItem);

            // interactions
            ShowSelectedNotificator = new Interaction<NotificatorItemViewModel, Unit>();
            ShowSelectedPhoneCallTemplate = new Interaction<PhoneCallTemplateItemViewModel, Unit>();

            LoadData().ConfigureAwait(false);
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
        public Interaction<NotificatorItemViewModel, Unit> ShowSelectedNotificator { get; }
        public Interaction<PhoneCallTemplateItemViewModel,Unit> ShowSelectedPhoneCallTemplate { get; }
        #endregion

        private async Task LoadData()
        {
            Title = "Overview";

            MenuItems.Clear();
            // create sections
            _phoneCallTemplateSection = new SectionViewModel<PhoneCallTemplateItemViewModel>()
            {
                Title = "Phonecall templates"
            };

            _notificatorSection = new SectionViewModel<NotificatorItemViewModel>()
            {
                Title = "Notificators"
            };

            // demo
            _notificatorSection.Items.Add(new NotificatorItemViewModel() { Name = "Notificator 1", SelectedCommand = ItemSelected, DeleteCommand = DeleteItem });
            _notificatorSection.Items.Add(new NotificatorItemViewModel() { Name = "Notificator 2", SelectedCommand = ItemSelected, DeleteCommand = DeleteItem });
            _notificatorSection.Items.Add(new NotificatorItemViewModel() { Name = "Notificator 3", SelectedCommand = ItemSelected, DeleteCommand = DeleteItem });
            _notificatorSection.Items.Add(new NotificatorItemViewModel() { Name = "Notificator 4", SelectedCommand = ItemSelected, DeleteCommand = DeleteItem });

            _phoneCallTemplateSection.Items.Add(new PhoneCallTemplateItemViewModel() { Name = "PhoneCall 1", PhoneNumber="110", ImagePath="dummy.png", SelectedCommand = ItemSelected, DeleteCommand = DeleteItem });
            _phoneCallTemplateSection.Items.Add(new PhoneCallTemplateItemViewModel() { Name = "PhoneCall 2", PhoneNumber = "112", ImagePath = "dummy.png", SelectedCommand = ItemSelected, DeleteCommand = DeleteItem });
            _phoneCallTemplateSection.Items.Add(new PhoneCallTemplateItemViewModel() { Name = "PhoneCall 3", PhoneNumber = "01721234567", ImagePath = "dummy.png", SelectedCommand = ItemSelected, DeleteCommand = DeleteItem });
            _phoneCallTemplateSection.Items.Add(new PhoneCallTemplateItemViewModel() { Name = "PhoneCall 4", PhoneNumber = "030123456", ImagePath = "dummy.png", SelectedCommand = ItemSelected, DeleteCommand = DeleteItem });

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
            if (selectedItem is NotificatorItemViewModel)
                await ShowSelectedNotificator.Handle((NotificatorItemViewModel)selectedItem);
            else if (selectedItem is PhoneCallTemplateItemViewModel)
                await ShowSelectedPhoneCallTemplate.Handle((PhoneCallTemplateItemViewModel)selectedItem);

            return Unit.Default;
        }

        private Unit OnDeleteItem(BaseItemViewModel selectedItem)
        {
            if (selectedItem is NotificatorItemViewModel)
                _notificatorSection.Items.Remove((NotificatorItemViewModel)selectedItem);
            else if (selectedItem is PhoneCallTemplateItemViewModel)
                _phoneCallTemplateSection.Items.Remove((PhoneCallTemplateItemViewModel)selectedItem);

            return Unit.Default;
        }

        private async Task OnCreateNewNotificator()
        {
            // TODO: Show BarcodeScanner, Create notificator
            NotificatorItemViewModel createdNotificator = null;

            // show edit
            await ShowSelectedNotificator.Handle(createdNotificator);
        }

        private async Task OnShowNewPhoneCallTemplate()
        {
            await ShowSelectedPhoneCallTemplate.Handle(null);
        }
        #endregion
    }
}
