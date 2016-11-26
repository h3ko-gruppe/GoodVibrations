using System.Threading.Tasks;
using GoodVibrations.Interfaces.Services;
using GoodVibrations.Models;
using ReactiveUI.Fody.Helpers;

namespace GoodVibrations.ViewModels
{
    public class EditNotificatorViewModel : EditorViewModel
    {
        private readonly IPersistenceService _persistence;

        public EditNotificatorViewModel(IPersistenceService persistence)
        {
            _persistence = persistence;
        }

        [Reactive]
        public bool IsNewTemplate { get; set; }

        [Reactive]
        public Notification Notification { get; private set; }

        [Reactive]
        public string TextLabel { get; set; }

        [Reactive]
        public string NamePlaceholder { get; set; }

        [Reactive]
        public string PhoneNumberPlaceholder { get; set; }

        public override void Init(object parameters)
        {
            Notification = parameters as Notification;

            if (Notification == null)
            {
                IsNewTemplate = true;
                Notification = new Notification();
            }

            SetUiTexts();
            CreateToolBarItems();
        }

        protected override void SetUiTexts()
        {
            base.SetUiTexts();

            Title = IsNewTemplate ? "Create Notification" : "Edit Notification";
            NamePlaceholder = "Name";
            PhoneNumberPlaceholder = "123456";
            TextLabel = "Text";
        }

        protected override async Task OnTest()
        {
            await App.Current.MainPage.DisplayAlert("Test not implemented", $"{this.GetType().Name}.{nameof(OnTest)}", "Ok");
        }

        protected override async Task OnSaveRequested()
        {
            await Task.Run(() => _persistence.Notification.InsertOrReplace(Notification));
        }

        protected override async Task OnDeletionRequested()
        {
            await Task.Run(() => _persistence.Notification.Delete(Notification));
        }

        protected override void SetImagePath(string imagePath)
        {
            Notification.NotificationIcon = imagePath;
        }
    }
}
