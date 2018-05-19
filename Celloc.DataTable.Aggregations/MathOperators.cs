using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Celloc.DataTable.Aggregations
{
	public static class MathOperators
	{
		private static readonly IDictionary<Type, Delegate> AddExpressions = new Dictionary<Type, Delegate>();

		public static T Add<T>(T lhs, T rhs)
		{
			if(AddExpressions.ContainsKey(typeof(T)))
				return (T)AddExpressions[typeof(T)].DynamicInvoke(lhs, rhs);
			
			var leftParameter = Expression.Parameter(typeof(T), "lhs");
			var rightParameter = Expression.Parameter(typeof(T), "rhs");
			var addExpression = Expression.Add(leftParameter, rightParameter);

			var func = Expression.Lambda<Func<T,T,T>>(addExpression, leftParameter, rightParameter).Compile();

			return (T)func.DynamicInvoke(lhs, rhs);
		}
	}
}
