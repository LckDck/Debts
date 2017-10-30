using Autofac;
using Autofac.Extras.CommonServiceLocator;
using debtsTracker.ViewModels;
using Microsoft.Practices.ServiceLocation;

namespace debtsTracker.Utilities
{
	public class ViewModelLocator
	{
		// Services registration
		public static void RegisterServices (ContainerBuilder registrations = null, bool registerFakes = false)
		{
			var builder = new ContainerBuilder ();

			// View model registration
            //builder.RegisterModule<CrossPlatformModule> ();
			
            builder.RegisterType<AddPageViewModel>();
            builder.RegisterType<AddTransactionPageViewModel>();
            builder.RegisterType<HistoryViewModel>();
            builder.RegisterType<MainViewModel>();

			var container = builder.Build ();

			registrations?.Update (container);

			ServiceLocator.SetLocatorProvider (() => new AutofacServiceLocator (container));
		}
	}
}

