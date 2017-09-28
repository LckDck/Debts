using System;
namespace debtsTracker.Managers
{
    public class InterfaceUpdateManager
    {
        public event EventHandler UpdateMainScreen;

        public void InvokeUpdateMainScreen() {
            if (UpdateMainScreen != null) { 
                UpdateMainScreen.Invoke(null, new EventArgs());
            }
        }
    }
}
