using System;
using GoodVibrations.Persistence.DAOs;

namespace GoodVibrations.Interfaces.Services
{
	public interface IPersistenceService
	{
		UserDao User { get; }
		SoundDao Sound  { get; }
		PhoneCallDao PhoneCall { get; }
		void Initialize();
	}
}
