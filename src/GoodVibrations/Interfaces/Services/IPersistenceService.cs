﻿using System;
using GoodVibrations.Models.Base;
using GoodVibrations.Persistence.Base;
using GoodVibrations.Persistence.DAOs;

namespace GoodVibrations.Interfaces.Services
{
	public interface IPersistenceService
	{
		void Initialize();
		UserDao User { get; }
		NotificationDao Notification  { get; }
		PhoneCallDao PhoneCall { get; }

    	GoodVibrationsBaseDao<T> GetDao<T>() where T : BaseModel, new();
	}
}
