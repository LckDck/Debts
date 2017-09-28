using System;
namespace debtsTracker.Entities
{
    public class Transaction
    {
        public double Value { get; set;}
        public string Comment { get; set;}
        public DateTime Date { get; set;}
    }
}
