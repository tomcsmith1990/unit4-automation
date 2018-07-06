using System;

namespace Unit4.Automation.Model
{
    internal static class ObjectExtensions
    {
        public static int NullSafeCompareTo<T>(this T a, T b) where T : IComparable<T>
        {
            if (a == null && b == null) return 0;
            if (a == null) return -1;
            if (b == null) return 1;
            return a.CompareTo(b);
        }
    }
}