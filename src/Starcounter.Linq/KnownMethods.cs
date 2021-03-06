using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

// ReSharper disable StaticMemberInGenericType
// ReSharper disable InconsistentNaming

namespace Starcounter.Linq
{
    //What is this?
    //This is a cache of known generic methods that the translator uses
    //This way, we can have a zero cost cache of methods for each entity type
    //public static fields in a generic class are often a bug, but not in this case
    //we explicitly want to be able from one generic class get hold of the generic methods of the same type w/o lookups
    internal static class KnownMethods<TEntity>
    {
        private static readonly IQueryable<TEntity> IQueryable = null;
        private static readonly Queryable<TEntity> Queryable = null;

        public static readonly MethodInfo IQueryableTake = MethodFromExample(() => IQueryable.Take(0));
        public static readonly MethodInfo IQueryableSkip = MethodFromExample(() => IQueryable.Skip(0));

        public static readonly MethodInfo QueryableFirstOrDefaultPred = MethodFromExample(() => Queryable.FirstOrDefault(i => true));
        public static readonly MethodInfo IQueryableFirstOrDefaultPred = MethodFromExample(() => IQueryable.FirstOrDefault(i => true));
        public static readonly MethodInfo QueryableFirstPred = MethodFromExample(() => Queryable.First(i => true));
        public static readonly MethodInfo IQueryableFirstPred = MethodFromExample(() => IQueryable.First(i => true));
        public static readonly MethodInfo IQueryableWhere = MethodFromExample(() => IQueryable.Where(i => true));
        public static readonly MethodInfo IQueryableCountPredicate = MethodFromExample(() => IQueryable.Count(i => true));

        //This takes an expression lambda and extracts the contained method.
        //This way, we can by example specify exactly what overload we want, instead of looking up by name and args
        private static MethodInfo MethodFromExample<TIgnore>(Expression<Func<TIgnore>> fun)
        {
            if (fun.Body is MethodCallExpression call)
            {
                return call.Method;
            }
            throw new NotSupportedException();
        }
    }

    //These are all the non generic known methods
    internal static class KnownMethods
    {
        private static readonly int[] Enumerable = null;
        private static readonly IQueryable<int> IQueryable = null;

        public static readonly MethodInfo EnumerableContains = MethodFromExample(() => Enumerable.Contains(0));
        public static readonly MethodInfo StringContains = MethodFromExample(() => "".Contains(""));
        public static readonly MethodInfo StringStartsWith = MethodFromExample(() => "".StartsWith(""));
        public static readonly MethodInfo StringEndsWith = MethodFromExample(() => "".EndsWith(""));

        //OrderBy et al have a generic arity of two, <TEntity,TValue> this means we cannot use the generic cache to store
        //references to the generic method instances, we simply don't know the TValue at compiletime/startup
        public static readonly MethodInfo IQueryableOrderBy = MethodFromExample(() => IQueryable.OrderBy(i => i));
        public static readonly MethodInfo IQueryableOrderByDesc = MethodFromExample(() => IQueryable.OrderByDescending(i => i));
        public static readonly MethodInfo IQueryableThenBy = MethodFromExample(() => IQueryable.OrderBy(i => i).ThenBy(i => i));
        public static readonly MethodInfo IQueryableThenByDesc = MethodFromExample(() => IQueryable.OrderByDescending(i => i).ThenByDescending(i => i));
        public static readonly MethodInfo IQueryableSum = MethodFromExample(() => IQueryable.Sum(i => i));
        public static readonly MethodInfo IQueryableAverage = MethodFromExample(() => IQueryable.Average(i => i));
        public static readonly MethodInfo IQueryableMin = MethodFromExample(() => IQueryable.Min(i => i));
        public static readonly MethodInfo IQueryableMax = MethodFromExample(() => IQueryable.Max(i => i));
        public static readonly MethodInfo IQueryableCount = MethodFromExample(() => IQueryable.Count());

        private static MethodInfo MethodFromExample<TIgnore>(Expression<Func<TIgnore>> fun)
        {
            if (fun.Body is MethodCallExpression call)
            {
                var method = call.Method;
                if (method.IsGenericMethod)
                    method = method.GetGenericMethodDefinition();
                return method;
            }
            throw new NotSupportedException();
        }
    }
}