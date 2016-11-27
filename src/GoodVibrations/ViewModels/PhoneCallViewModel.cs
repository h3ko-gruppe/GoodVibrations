using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodVibrations.Interfaces.Services;
using GoodVibrations.Models;
using Plugin.Contacts;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace GoodVibrations.ViewModels
{
	public class PhoneCallViewModel : BaseViewModel
	{
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

		private readonly IPhoneCallService _phoneCallService;
		private PhoneCall _phoneCall = new PhoneCall();

		public PhoneCallViewModel(IPhoneCallService phoneCallService)
		{
			_phoneCallService = phoneCallService;
			Call = ReactiveCommand.CreateFromTask(OnCall);
			Contact = ReactiveCommand.CreateFromTask(OnContact);
			SetUiTexts();
		}

		private async Task OnCall()
		{
			await Task.Run(() => _phoneCallService.StartCall(_phoneCall));
		}

		private async Task OnContact()
		{
			await ShowSelectedNotificator.Handle(createdNotificator.Notification);

			_phoneCall.DestinationNumber = "";
			_phoneCall.Text = "";
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
