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
using Java.Util;
using Android.Graphics;

namespace debtsTracker
{
    [Activity (Label = "@string/app_name", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            // Set our view from the "main" layout resource
            //SetContentView (Resource.Layout.Main);
            SetContentView (Resource.Layout.history);
            //var dateView = FindViewById<EditText> (Resource.Id.date);
            //dateView.Text = DateTime.Now.ToString (Utils.DatePattern);
            //dateView.Click += (sender, e) => {
            //    Calendar calendar = Calendar.GetInstance (Java.Util.TimeZone.Default);
            //    DatePickerDialog dialog = new DatePickerDialog (CrossCurrentActivity.Current.Activity, ChangeText,
            //                                                    calendar.Get (CalendarField.Year), calendar.Get (CalendarField.Month),
            //                                                    calendar.Get (CalendarField.DayOfMonth));
            //    dialog.Show ();
            //};

            //var amountView = FindViewById<EditText> (Resource.Id.amount);
            //amountView.TextChanged += (sender, e) => {
            //    var color = (e.Text.ToString ().StartsWith ("-")) ? Utils.DarkGray : Utils.Green;
            //    amountView.SetTextColor (color);
            //};

            var listView = FindViewById<RecyclerView> (Resource.Id.list);

            var linearLayoutManager = new LinearLayoutManager (CrossCurrentActivity.Current.Activity);
            
            listView.SetLayoutManager(linearLayoutManager);
            //var items = GetItems () ;
            //var adapter = new DebtsAdapter(items);
            var items = GetItems ()[0].Transactions ;
            var adapter = new TransactionsAdapter(items);
            listView.SetAdapter(adapter);


        }

        void ChangeText (object sender, DatePickerDialog.DateSetEventArgs e)
        {
            
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

