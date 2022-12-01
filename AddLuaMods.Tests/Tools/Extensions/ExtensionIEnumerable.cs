using System;
using System.Collections.Generic;

namespace AddLuaMods.Tests.Tools.Extensions
{
    public static class ExtensionIEnumerable
    {
        public static void ForEach<T>(this IEnumerable<T> values, Action<T> onItem)
        {
            foreach (var value in values)
            {
                onItem(value);
            }
        }
    }
}
