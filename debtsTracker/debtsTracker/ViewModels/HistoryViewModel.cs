﻿using System;
using System.Collections.Generic;
using debtsTracker.Entities;
using debtsTracker.Managers;
using debtsTracker.Utilities;
using Microsoft.Practices.ServiceLocation;

namespace debtsTracker.ViewModels
{
    public class HistoryViewModel : BaseVm
    {
        public HistoryViewModel()
        {
            Subscribe();
        }

        Debt _debt;
        public Debt Debt
        {
            get
            {
                return _debt;
            }
            internal set
            {
                _debt = value;
                TotalText = GetTotalText(_debt);
            }
        }

        internal List<Transaction> GetItems()
        {
            return Debt.Transactions;
        }

        internal void AddPage()
        {
            NavigationService.NavigateTo(Page.AddPage);
        }




        private string totalText;
        public string TotalText
        {
            get { return totalText; }
            set
            {
                Set(() => TotalText, ref totalText, value);
            }
        }

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

        internal void Subscribe()
        {
            InterfaceUpdateManager.ItemRemoved += OnItemRemoved;
        }

        internal void Unsubscribe()
        {
            InterfaceUpdateManager.ItemRemoved -= OnItemRemoved;
        }

        private void OnItemRemoved(object sender, EventArgs e)
        {
            var newDebt = DebtsManager.GetDebt(Debt.Name);
            if (newDebt != null)
            {
                Debt = newDebt;
                TotalText = GetTotalText(Debt);
            }
        }

        private string GetTotalText(Debt debt)
        {
            var str = Utils.GetStringFromResource(Resource.String.total);
            return string.Format(str, debt.Value.ToString());
        }
    }
}
