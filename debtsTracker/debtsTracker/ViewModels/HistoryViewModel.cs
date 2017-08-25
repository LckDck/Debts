﻿using System;
using System.Collections.Generic;
using debtsTracker.Entities;

namespace debtsTracker.ViewModels
{
    public class HistoryViewModel : BaseVm
    {
        public HistoryViewModel ()
        {
        }
        public Debt Debt { get; internal set; }

        internal List<Transaction> GetItems ()
        {
            return Debt.Transactions;
        }
    }
}
