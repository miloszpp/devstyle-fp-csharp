using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace devstylefpcsharp
{
    public static class FP
    {
        public static Func<T1, Func<T2, R>> Curry<T1, T2, R>(Func<T1, T2, R> func)
        {
            return x => y => func(x, y);
        }

        public static Func<T1, Func<T2, Func<T3, R>>> Curry<T1, T2, T3, R>(Func<T1, T2, T3, R> func)
        {
            return x => y => z => func(x, y, z);
        }

        public static Func<T1, T3> Compose<T1, T2, T3>(Func<T1, T2> f1, Func<T2, T3> f2) 
        {
            return x => f2(f1(x));
        }

        public static Func<T1, T4> Compose<T1, T2, T3, T4>(Func<T1, T2> f1, Func<T2, T3> f2, Func<T3, T4> f3)
        {
            return x => f3(f2(f1(x)));
        }

        public static Func<T1, T5> Compose<T1, T2, T3, T4, T5>(Func<T1, T2> f1, Func<T2, T3> f2, Func<T3, T4> f3, Func<T4, T5> f4)
        {
            return x => f4(f3(f2(f1(x))));
        }

        public static T Reduce<T>(Func<T, T, T> reducer, IEnumerable<T> items) 
        {
            return items.Aggregate(reducer);
        }

        public static Func<IEnumerable<T>, T> Reduce<T>(Func<T, T, T> reducer)
        {
            return items => items.Aggregate(reducer);
        }
    }
}
