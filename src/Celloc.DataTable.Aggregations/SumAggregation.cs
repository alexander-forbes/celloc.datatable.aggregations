using System;
using System.Collections.Generic;
using System.Linq;

namespace Celloc.DataTable.Aggregations
{
	public static class SumAggregation
	{
		public static T Sum<T>(this System.Data.DataTable dataTable, string range) 
			where T : struct
		{
			ArgumentGuards.GuardAgainstNullDataTable(dataTable);
			ArgumentGuards.GuardAgainstNullRange(range);

			var rangeTuple = dataTable.TranslateRange(range);
		
			return rangeTuple.HasValue ? Sum<T>(dataTable, rangeTuple.Value) : default(T);
		}

		public static T Sum<T>(this System.Data.DataTable dataTable, ((int Column, int Row), (int Column, int Row)) range) 
			where T : struct
		{
			ArgumentGuards.GuardAgainstNullDataTable(dataTable);
			ArgumentGuards.GuardAgainstMultipleColumns(range);
			ArgumentGuards.GuardAgainstNonNumericType(typeof(T));

			if (!dataTable.Contains(range))
				return default(T);

			var column = range.Item1.Column;
			var total = default(T);

			for (var row = range.Item1.Row; row <= range.Item2.Row; row++)
			{
				var value = dataTable.GetValue((column, row));
				var rhs = TypeChanger.ChangeType<T>(value);
				total = Operators.Add(total, rhs);
			}

			return total;
		}

		public static IEnumerable<(object Key, T Total)> Sum<T>(this IEnumerable<DataRowGrouping> dataRowGroupings, int columnIndex) 
			where T : struct
		{
			ArgumentGuards.GuardAgainstNullDataRowGroupings(dataRowGroupings);
			ArgumentGuards.GuardAgainstInvalidColumnIndex(dataRowGroupings, columnIndex);

			var groupTotals = new List<(object Key, T Total)>();

			foreach (var grouping in dataRowGroupings)
			{
				var groupTotal = SumGroup<T>(grouping, columnIndex);
				groupTotals.Add((grouping.Key, groupTotal));
			}

			return groupTotals;
		}

		private static T SumGroup<T>(DataRowGrouping dataRowGrouping, int columnIndex)
			where T : struct
		{
			var total = default(T);

			foreach (var row in dataRowGrouping)
			{
				var value = row.ItemArray.ElementAt(columnIndex);
				var rhs = TypeChanger.ChangeType<T>(value);
				total = Operators.Add(total, rhs);
			}

			return total;
		}
	}
}
