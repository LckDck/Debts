using System;
namespace debtsTracker
{
    public static class Utils
    {
        public static string GetValueWithPrefix (int value) {
            return value >= 0 ? "+" + value :  value.ToString();
        }
    }
}
