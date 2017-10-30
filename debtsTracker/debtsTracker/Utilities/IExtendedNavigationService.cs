using System;
namespace debtsTracker.Utilities
{
    public interface IExtendedNavigationService
    {
        void NavigateTo (Page page, object parameter);
        void NavigateTo (Page page, object parameter1, object parameter2);
        void NavigateTo (Page page);
        void NavigateUrl (string url);
        void GoBack ();
        void PopToRoot ();
        int BackStackCount { get; }

        event EventHandler<Page> BeforeNavigateEvent;
        void Configure (PageConfigEntity pageEntity);
    }
}