using System.Collections;
using System.Collections.Generic;

namespace MarkdownFromHtml.Utils
{
    internal static class EnumerableExt
    {
        public static bool TryCast<T>(this IEnumerable list, out List<T> casts)
        {
            casts = new List<T>();

            foreach (var e in list)
            {
                if (e is T t) casts.Add(t);
                else return false;
            }

            return true;
        }
    }
}
