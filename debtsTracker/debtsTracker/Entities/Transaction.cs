using System;
namespace debtsTracker.Entities
{
    public class Transaction
    {
        public int Value { get; set;}
        public string Comment { get; set;}
        public DateTime Date { get; set;}
    }
}
