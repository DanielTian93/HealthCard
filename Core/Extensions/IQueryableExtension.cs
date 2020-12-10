using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Extensions
{
    public static class IQueryableExtension
    {
        public static IQueryable<T> WhereIF<T>(this IQueryable<T> queryable, bool IsQuery, Expression<Func<T, bool>> whereLambda)
        {
            if (IsQuery)
            {
                queryable = queryable.Where(whereLambda);
            }
            return queryable;
        }
        public static IEnumerable<TSource> DistinctBy<TSource, Tkey>(this IEnumerable<TSource> source, Func<TSource, Tkey> keySelector)
        {
            HashSet<Tkey> hashSet = new HashSet<Tkey>();
            foreach (TSource item in source)
            {
                if (hashSet.Add(keySelector(item)))
                {
                    yield return item;
                }
            }
        }
    }

}
