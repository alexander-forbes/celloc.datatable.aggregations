using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Celloc.DataTable.Aggregations
{
	public static class Operators
	{
		private static readonly IDictionary<(string Operator, Type Type), Delegate> DelegateCache =
			new Dictionary<(string Operator, Type Type), Delegate>();

		public static T Add<T>(T lhs, T rhs)
			where T : struct
		{
			ArgumentGuards.GuardAgainstNonNumericType(typeof(T));

			var cacheKey = ("+", typeof(T));

			if (DelegateCache.ContainsKey(cacheKey))
				return (T)DelegateCache[cacheKey].DynamicInvoke(lhs, rhs);

			Compile<T, T>(Expression.Add, cacheKey);

			return (T)DelegateCache[cacheKey].DynamicInvoke(lhs, rhs);
		}

		public static bool GreaterThan<T>(T lhs, T rhs)
			where T : struct
		{
			var cacheKey = (">", typeof(T));

			if (DelegateCache.ContainsKey(cacheKey))
				return (bool)DelegateCache[cacheKey].DynamicInvoke(lhs, rhs);

			Compile<bool, T>(Expression.GreaterThan, cacheKey);

			return (bool)DelegateCache[cacheKey].DynamicInvoke(lhs, rhs);
		}

		public static bool LessThan<T>(T lhs, T rhs)
			where T : struct
		{
			var cacheKey = ("<", typeof(T));

			if (DelegateCache.ContainsKey(cacheKey))
				return (bool)DelegateCache[cacheKey].DynamicInvoke(lhs, rhs);

			Compile<bool, T>(Expression.LessThan, cacheKey);

			return (bool)DelegateCache[cacheKey].DynamicInvoke(lhs, rhs);
		}


		private static void Compile<TResult, TParam>(Func<ParameterExpression, ParameterExpression, BinaryExpression> binaryExpression, (string Operator, Type Type) cacheKey)
		{
			var leftParameter = Expression.Parameter(typeof(TParam), "lhs");
			var rightParameter = Expression.Parameter(typeof(TParam), "rhs");
			var body = binaryExpression(leftParameter, rightParameter);

			var func = Expression.Lambda<Func<TParam, TParam, TResult>>(body, leftParameter, rightParameter).Compile();

			DelegateCache.Add(cacheKey, func);
		}
	}
}
