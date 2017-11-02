using System;
namespace Debts.Interfaces
{
	public interface ITimerInstance
	{
		event EventHandler TimerElapsed;
		void StopTimer ();
		void StartTimer ();
		void Dispose ();
		void SetInterval (double milliseconds);
	}
}
