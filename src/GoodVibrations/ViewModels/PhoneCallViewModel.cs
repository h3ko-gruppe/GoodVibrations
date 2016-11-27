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
using Plugin.Geolocator;
using GoodVibrations.Interfaces;

namespace GoodVibrations.ViewModels
{
	public class PhoneCallViewModel : BaseViewModel
	{
		private readonly IPhoneCallService _phoneCallService;
		private readonly ILocationManager _locationManager;

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

		[Reactive]
		public string DestinationNumberLabel { get; set; }

		public ReactiveCommand Call { get; }
		public ReactiveCommand Contact { get; }

		public Interaction<Action<Contact>, Unit> SelectContact { get; }

		public PhoneCallViewModel(IPhoneCallService phoneCallService, ILocationManager locationManager)
		{
			_phoneCallService = phoneCallService;
			_locationManager = locationManager;

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
			//MessageText = contact.DisplayName;
		}

		private async Task Locate()
		{
			if (MessageText.Contains("*loc*")) //Constants Placeholder
			{
				var locator = CrossGeolocator.Current;

				locator.DesiredAccuracy = 0;
				var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
				var address = await _locationManager.LoadAddress(position.Latitude, position.Longitude);

				MessageText = MessageText.Replace("*loc*", address);
			}
		}

		private async Task OnCall()
		{
			await Locate();
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
			DestinationNumberLabel = "Telephonenumber";
			MessageText = string.Empty;
		}
	}
}
