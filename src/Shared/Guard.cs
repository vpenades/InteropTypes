using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;



namespace System
{
    public static class Guard
    {
        public static void NotNull<T>(string name, T value) where T:class
        {
            if (value == null) throw new ArgumentNullException(name);
        }

        public static void IsTrue(string name, bool value)
        {
            if (!value) throw new ArgumentException("Not true", name);
        }

        public static void GreaterThan<T>(string name, T value, T refval) where T : IComparable
        {
            if (value.CompareTo(refval) == 1) throw new ArgumentOutOfRangeException(nameof(name));
        }


    }
}
