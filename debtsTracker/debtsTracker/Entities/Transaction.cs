using System;
using debtsTracker.Managers;
using Microsoft.Practices.ServiceLocation;

namespace debtsTracker.Entities
{
    public class Transaction : IDisposable
    {
        public double Value { get; set;}
        public string Comment { get; set;}
        public DateTime Date { get; set;}
        public string Name { get; set;}

		DebtsManager _debtsManager;
		DebtsManager DebtsManager
		{
			get
			{
				return _debtsManager ?? (_debtsManager = ServiceLocator.Current.GetInstance<DebtsManager>());
			}
		}

		InterfaceUpdateManager _interfaceUpdateManager;
		InterfaceUpdateManager InterfaceUpdateManager
		{
			get
			{
				return _interfaceUpdateManager ?? (_interfaceUpdateManager = ServiceLocator.Current.GetInstance<InterfaceUpdateManager>());
			}
		}

        public void Dispose()
        {
            DebtsManager.RemoveTransaction(this);
            InterfaceUpdateManager.InvokeItemRemoved();
        }
    }
}
