using System;
using System.Threading.Tasks;
using GoodVibrations.ViewModels.ItemViewModels;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace GoodVibrations.ViewModels
{
    public class PhoneCallTemplateViewModel : BaseViewModel
    {
        public PhoneCallTemplateViewModel()
        {
            ChooseImage = ReactiveCommand.CreateFromTask(OnChooseImage);
            Save = ReactiveCommand.CreateFromTask(OnSave);
        }

        [Reactive]
        public PhoneCallTemplateItemViewModel PhoneCallTemplate { get; set; }

        [Reactive]
        public bool IsNewTemplate { get; set; }

        [Reactive]
        public string ChooseImageText { get; set; }

        [Reactive]
        public string TextLabel { get; set; }

        [Reactive]
        public string NamePlaceholder { get; set; }

        [Reactive]
        public string PhoneNumberPlaceholder { get; set; }

        public ReactiveCommand ChooseImage { get; }
        public ReactiveCommand Save { get; }

        public override void Init(object parameters)
        {
            PhoneCallTemplate = parameters as PhoneCallTemplateItemViewModel;

            if (PhoneCallTemplate == null)
            {
                IsNewTemplate = true;
                PhoneCallTemplate = new PhoneCallTemplateItemViewModel();
            }

            SetUiTexts();
        }

        protected override void CreateToolBarItems()
        {
            ToolBarItems.Clear();

            ToolBarItems.Add(new ActionItemViewModel()
            {
                Title = "Save",
                SelectedCommand = Save
            });
        }

        private void SetUiTexts()
        {
            Title = IsNewTemplate ? "Create Template" : "Edit Template";
            NamePlaceholder = "Name";
            PhoneNumberPlaceholder = "123456";
            ChooseImageText = "Choose Image";
            TextLabel = "Text";
        }

        private async Task OnSave()
        {
            // TODO: save template
        }

        private Task OnChooseImage()
        {
            // TODO: show ActionSheet => Gallary or Camera
            // TODO: Start Camera or Gallery
            // TODO: set ImagePath

            PhoneCallTemplate.ImagePath = "dummy.png";

            return Task.FromResult(true);
        }
    }
}
