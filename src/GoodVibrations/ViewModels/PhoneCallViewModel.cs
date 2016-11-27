using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using GoodVibrations.Interfaces.Services;
using GoodVibrations.Models;
using Plugin.Contacts.Abstractions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;

namespace GoodVibrations.ViewModels
{
    public class PhoneCallViewModel : BaseViewModel
    {
        private readonly IPhoneCallService _phoneCallService;

        [Reactive]
        public string CallText { get; set; }

        [Reactive]
        public string ContactText { get; set; }

        [Reactive]
        public string TextLabel { get; set; }

        [Reactive]
        public string PhoneNumberPlaceholder { get; set; }

        [Reactive]
        public string DestinationNumber { get; set; }

        [Reactive]
        public string MessageText { get; set; }

        public ReactiveCommand Call { get; }
        public ReactiveCommand Contact { get; }

        public Interaction<Action<Contact>, Unit> SelectContact { get; }

        public PhoneCallViewModel(IPhoneCallService phoneCallService)
        {
            _phoneCallService = phoneCallService;

            Call = ReactiveCommand.CreateFromTask(OnCall);
            Contact = ReactiveCommand.CreateFromTask(OnContact);
            SelectContact = new Interaction<Action<Contact>, Unit>();

            SetUiTexts();
        }

        public void OnSelectedContact(Contact contact)
        {
            if (contact == null)
                return;

            DestinationNumber = contact.Phones.FirstOrDefault()?.Number ?? string.Empty;
            MessageText = contact.DisplayName;
        }

        private async Task OnCall()
        {
            await Task.Run(() => _phoneCallService.StartCall(MessageText, DestinationNumber, string.Empty, string.Empty));
        }

        private async Task OnContact()
        {
            await SelectContact.Handle(OnSelectedContact);
        }

        private void SetUiTexts()
        {
            Title = "Direct Call";
            PhoneNumberPlaceholder = "+49162111111";
            CallText = "Call";
            TextLabel = "Messagetext";
            ContactText = "Contacts";
        }
    }
}
