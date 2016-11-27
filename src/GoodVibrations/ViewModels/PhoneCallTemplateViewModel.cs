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
        
        public PhoneCallTemplateViewModel(IPersistenceService persistence)
        {
            _persistence = persistence;

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
            PhoneNumberPlaceholder = "+49162111111";
            ChooseImageText = "Choose Image";
            TextLabel = "Text";
            SaveText = "Save";
            DeleteText = "Delete";
        }

        private async Task OnTest()
        {
            await App.Current.MainPage.DisplayAlert("Test not implemented", $"{this.GetType().Name}.{nameof(OnTest)}", "Ok");
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
