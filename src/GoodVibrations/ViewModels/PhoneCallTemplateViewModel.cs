using System;
using System.Reactive;
using System.Threading.Tasks;
using GoodVibrations.ViewModels.ItemViewModels;
using ReactiveUI;
using System.Reactive.Linq;
using ReactiveUI.Fody.Helpers;
using GoodVibrations.Interfaces.Services;
using GoodVibrations.Models;

namespace GoodVibrations.ViewModels
{
    public class PhoneCallTemplateViewModel : BaseViewModel
    {
        private readonly IPersistenceService _persistence;
        private readonly IPhoneCallService _phoneCallService;
        public PhoneCallTemplateViewModel(IPersistenceService persistence, IPhoneCallService phoneCallService)
        {
            _persistence = persistence;
            _phoneCallService = phoneCallService;
            ChooseImage = ReactiveCommand.CreateFromTask(OnChooseImage);
            Save = ReactiveCommand.CreateFromTask(OnSave);
            Test = ReactiveCommand.CreateFromTask(OnTest);
            Delete = ReactiveCommand.CreateFromTask(OnDelete);

            Close = new Interaction<Unit, Unit>();
        }

        [Reactive]
        public PhoneCall PhoneCall { get; set; }

        [Reactive]
        public bool IsNewTemplate { get; set; }

        [Reactive]
        public string ChooseImageText { get; set; }

        [Reactive]
        public string TextLabel { get; set; }

        [Reactive]
        public string NamePlaceholder { get; set; }

        [Reactive]
        public string SaveText { get; set; }

        [Reactive]
        public string DeleteText { get; set; }

        [Reactive]
        public string PhoneNumberPlaceholder { get; set; }

        public ReactiveCommand ChooseImage { get; }
        public ReactiveCommand Save { get; }
        public ReactiveCommand Delete { get; }
        public ReactiveCommand Test { get; }

        public Interaction<Unit, Unit> Close { get; }

        public override void Init(object parameters)
        {
            PhoneCall = parameters as PhoneCall;

            if (PhoneCall == null)
            {
                IsNewTemplate = true;
                PhoneCall = new PhoneCall();
            }

            SetUiTexts();
            CreateToolBarItems();
        }

        protected override void CreateToolBarItems()
        {
            ToolBarItems.Clear();

            ToolBarItems.Add(new ActionItemViewModel()
            {
                Title = "Test",
                SelectedCommand = Test
            });
        }

        private void SetUiTexts()
        {
            Title = IsNewTemplate ? "Create Template" : "Edit Template";
            NamePlaceholder = "Name";
            PhoneNumberPlaceholder = "123456";
            ChooseImageText = "Choose Image";
            TextLabel = "Text";
            SaveText = "Save";
            DeleteText = "Delete";
        }

        private async Task OnTest()
        {
            var isSuccessful = await _phoneCallService.StartCall (PhoneCall.Text, PhoneCall.DestinationNumber, string.Empty, string.Empty);
            var not = isSuccessful ? " " : "NOT ";
            await App.Current.MainPage.DisplayAlert("Phone call test", $"The test was {not}started successfully", "Ok");
        }

        private async Task OnSave()
        {
            await Task.Run(() => _persistence.PhoneCall.InsertOrReplace(PhoneCall));

            await Close.Handle(Unit.Default);
        }

        private async Task OnDelete()
        {
            var result = await App.Current.MainPage.DisplayActionSheet("Delete Template", "Cancel", DeleteText);

            if (result == DeleteText)
            {
                await Task.Run(() => _persistence.PhoneCall.Delete(PhoneCall));

                await Close.Handle(Unit.Default);
            }
        }

        private Task OnChooseImage()
        {
            // TODO: show ActionSheet => Gallary or Camera
            // TODO: Start Camera or Gallery
            // TODO: set ImagePath

            PhoneCall.Icon = "dummy.png";

            return Task.FromResult(true);
        }
    }
}
