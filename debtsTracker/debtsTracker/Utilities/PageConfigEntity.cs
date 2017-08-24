using System;
namespace debtsTracker.Utilities
{
	public class PageConfigEntity
	{
        public Page Page { get; set; }
		public bool IsRoot { get; set; }
		public Type Type { get; set; }
	}
}

