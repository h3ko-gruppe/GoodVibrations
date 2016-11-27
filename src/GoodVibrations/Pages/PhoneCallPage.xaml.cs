using System;
using System.Collections.Generic;
using GoodVibrations.Extensions;
using ReactiveUI;
using Xamarin.Forms;

namespace GoodVibrations.Pages
{
	public partial class PhoneCallPage
	{
		public PhoneCallPage()
		{
			InitializeComponent();
			this.AutoWireViewModel();

			this.WhenActivated(dispose =>
		 	{
				 dispose(this.BindToTitle(ViewModel));
			 });
		}
	}
}
