using System;
using GalaSoft.MvvmLight;
using System.Reflection;
using System.Linq.Expressions;
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Views;
using System.Threading.Tasks;
using debtsTracker.Utilities;

namespace debtsTracker.ViewModels
{
    public class BaseVm : ViewModelBase
    {
        protected IExtendedNavigationService NavigationService;
        public BaseVm ()
        {
            NavigationService = ServiceLocator.Current.GetInstance<IExtendedNavigationService> ();
        }

        public bool IsErrorProcessed { get; set; }
        private string errorMessage;
        public string ErrorMessage {
            get { return errorMessage; }
            protected set {
                IsErrorProcessed = value == null;
                errorMessage = value;
                RaisePropertyChanged ();
            }
        }

    }
}

