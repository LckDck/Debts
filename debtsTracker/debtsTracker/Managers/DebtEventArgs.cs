using System;
using debtsTracker.Entities;

namespace debtsTracker.Managers
{

    public class DebtEventArgs : EventArgs
    {
        readonly Debt _debt;
        public Debt Debt
        {
            get
            {
                return _debt;
            }
        }

        public DebtEventArgs(Debt debt)
        {
            _debt = debt;
        }
    }
}
