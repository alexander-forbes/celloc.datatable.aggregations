using System;
using System.Linq.Expressions;

namespace Celloc.DataTable.Aggregations
{
	public static class MathOperators
	{
		public static T Add<T>(T lhs, T rhs)
		{
			var left = Expression.Parameter(typeof(T), "lhs");
			var right = Expression.Parameter(typeof(T), "rhs");

			var expression = Expression.Add(left, right);

			var x = Expression.Lambda<Func<T,T,T>>(expression, left, right).Compile();

			return x(lhs, rhs);
		}
	}
}
