using System;
using System.Linq;

namespace Celloc.DataTable.Aggregations
{
	public static class SumAggregation
	{
		public static T Sum<T>(this System.Data.DataTable dataTable, string range) where T : struct
		{
			ArgumentGuards.GuardAgainstNullDataTable(dataTable);
			ArgumentGuards.GuardAgainstNullRange(range);

			var rangeTuple = dataTable.TranslateRange(range);

			return rangeTuple.HasValue ? Sum<T>(dataTable, rangeTuple.Value) : default(T);
		}

		public static T Sum<T>(this System.Data.DataTable dataTable, ((int Column, int Row),(int Column, int Row)) range) where T : struct
		{
			ArgumentGuards.GuardAgainstNullDataTable(dataTable);
			ArgumentGuards.GuardAgainstMultipleColumns(range);

			if (!dataTable.Contains(range))
				return default(T);

			if(!IsNumericType(typeof(T)))
				throw new ArgumentException("The specified type is not a numeric type.");

			T total = default(T);

			for(var row = range.Item1.Row; row <= range.Item2.Row; row++)
			{
				var x = (T)Convert.ChangeType(dataTable.Rows[row].ItemArray.ElementAt(range.Item1.Column), typeof(T));
				total = MathOperators.Add(x, total);
			}

			return total;
		}

		private static bool IsNumericType(Type type)
		{
			return type == typeof(sbyte)
				|| type == typeof(byte)
				|| type == typeof(short)
				|| type == typeof(ushort)
				|| type == typeof(int)
				|| type == typeof(uint)
				|| type == typeof(long)
				|| type == typeof(ulong)
				|| type == typeof(float)
				|| type == typeof(double)
				|| type == typeof(decimal);
		}
	}
}
