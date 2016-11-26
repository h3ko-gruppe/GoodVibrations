using System;
using System.Threading.Tasks;

namespace GoodVibrations.Interfaces.Services
{
	public interface ISoundService
	{
		Task RecordSound();
		Task SaveSound();
		Task DeleteSound();
	}
}