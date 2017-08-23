using Android.App;
using Android.Widget;
using Android.OS;
using com.refractored.fab;
using Android.Support.V7.Widget;
using Plugin.CurrentActivity;
using debtsTracker.Adapters;
using System;
using System.Collections.Generic;
using debtsTracker.Entities;

namespace debtsTracker
{
    [Activity (Label = "@string/app_name", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            var listView = FindViewById<RecyclerView> (Resource.Id.list);

            var linearLayoutManager = new LinearLayoutManager (CrossCurrentActivity.Current.Activity);
            
            listView.SetLayoutManager(linearLayoutManager);
            //var items = GetItems () ;
            //var adapter = new DebtsAdapter(items);
            var items = GetItems ()[0].Transactions ;
            var adapter = new TransactionsAdapter(items);
            listView.SetAdapter(adapter);

            //adapter.ItemSelected += ((sender, e) =>
            //{
            //    if (Vm is IItemClickCommand<TItemVm>) (Vm as IItemClickCommand<TItemVm>).ItemClick.Execute(Vm.Model[e]);
            //});

            // Get our button from the layout resource,
            // and attach an event to it
            //Button button = FindViewById<Button> (Resource.Id.fab);

            //button.Click += delegate { button.Text = $"{count++} clicks!"; };
        }

        List<Debt> GetItems ()
        {
            var result = new List<Debt> () {

                new Debt { Name = "Anna", 
                    Transactions = new List<Transaction> {
                        new Transaction { Value = 1100, Date = DateTime.Now, Comment="Какой-то коммент"},
                        new Transaction { Value = -300, Date = DateTime.Now, Comment=""},
                        new Transaction { Value = 350, Date = DateTime.Now, Comment="Какой-то очень длинный супер пупер бубу сися пися коммент" }
                    }
                },
                new Debt { Name = "Michael", 
                    Transactions = new List<Transaction> {
                        new Transaction { Value = 2000, Date = DateTime.Now},
                        new Transaction { Value = 150, Date = DateTime.Now}
                    }
                }
            };

            return result;
        }
   }
}

