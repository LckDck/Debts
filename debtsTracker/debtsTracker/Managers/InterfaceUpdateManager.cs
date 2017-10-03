using System;
using debtsTracker.Entities;

namespace debtsTracker.Managers
{
    public class InterfaceUpdateManager
    {
        public event EventHandler UpdateMainScreen;

        public void InvokeUpdateMainScreen()
        {
            if (UpdateMainScreen != null)
            {
                UpdateMainScreen.Invoke(null, new EventArgs());
            }
        }

        public event EventHandler ItemRemoved;

        public void InvokeItemRemoved()
        {
            if (ItemRemoved != null)
            {
                ItemRemoved.Invoke(null, new EventArgs());
            }
        }


        public event EventHandler NameFocus;

        public void InvokeNameFocus()
        {
            if (NameFocus != null)
            {
                NameFocus.Invoke(null, new EventArgs());
            }
        }

        public int CurrrentTab { get; set; }

        public bool IsTabToMe
        {
            get
            {
                return CurrrentTab == 1;
            }
        }
    }
}
