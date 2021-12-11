using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{
    public static class IEnumerableExtensions
    {

        public static IEnumerable<T> MergeWith<T>(this IEnumerable<T> mainEnumerable, params IEnumerable<T>[] additionalEnumerables)
        {
            if (mainEnumerable != null)
                foreach (T item in mainEnumerable)
                    yield return item;

            for (int i = 0; i < additionalEnumerables.Length; i++)
                if (additionalEnumerables[i] != null)
                    foreach (T item in additionalEnumerables[i])
                        yield return item;
        }

    }
}
