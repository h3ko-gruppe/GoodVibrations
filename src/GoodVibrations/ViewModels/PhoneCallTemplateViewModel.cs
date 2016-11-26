using System.Threading.Tasks;
using ReactiveUI.Fody.Helpers;
using GoodVibrations.Interfaces.Services;
using GoodVibrations.Models;

namespace GoodVibrations.ViewModels
{
    public class PhoneCallTemplateViewModel : EditorViewModel
    {
        private readonly IPersistenceService _persistence;

        public PhoneCallTemplateViewModel(IPersistenceService persistence)
        {
            _persistence = persistence;
        }

        [Reactive]
        public PhoneCall PhoneCall { get; set; }

        [Reactive]
        public bool IsNewTemplate { get; set; }       

        [Reactive]
        public string TextLabel { get; set; }

        [Reactive]
        public string NamePlaceholder { get; set; }

        [Reactive]
        public string PhoneNumberPlaceholder { get; set; }

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

        protected override void SetUiTexts()
        {
            base.SetUiTexts();

            Title = IsNewTemplate ? "Create Template" : "Edit Template";
            NamePlaceholder = "Name";
            PhoneNumberPlaceholder = "123456";
            TextLabel = "Text";
        }

        protected override async Task OnSaveRequested()
        {
            await Task.Run(() => _persistence.PhoneCall.InsertOrReplace(PhoneCall));
        }

        protected override async Task OnDeletionRequested()
        {
            await Task.Run(() => _persistence.PhoneCall.Delete(PhoneCall));
        }

        protected override void SetImagePath(string imagePath)
        {
            PhoneCall.Icon = imagePath;
        }
    }
}
