using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Celloc.DataTable.Aggregations
{
	public static class MinMaxAggregation
	{
		public static DataRow Max<T>(this System.Data.DataTable dataTable, ((int Column, int Row), (int Column, int Row)) range) 
			where T : struct
		{
			ArgumentGuards.GuardAgainstNullDataTable(dataTable);
			ArgumentGuards.GuardAgainstMultipleColumns(range);

			if (!dataTable.Contains(range))
				return null;

			var column = range.Item1.Column;
			var maxRowIndex = range.Item1.Row;

			var max = default(T);

			for(var row = range.Item1.Row; row <= range.Item2.Row; row++)
			{
				var value = dataTable.GetValue((column, row));
				var lhs = TypeChanger.ChangeType<T>(value);

				if (Operators.GreaterThan(lhs, max))
				{
					max = lhs;
					maxRowIndex = row;
				}
			}

			return dataTable.Rows[maxRowIndex];
		}

		public static DataRow Max<T>(this System.Data.DataTable dataTable, string range)
			where T : struct
		{
			ArgumentGuards.GuardAgainstNullDataTable(dataTable);
			ArgumentGuards.GuardAgainstNullRange(range);

			var rangeTuple = dataTable.TranslateRange(range);

			return rangeTuple.HasValue ? Max<T>(dataTable, rangeTuple.Value) : null;
		}

		public static IEnumerable<(object, DataRow)> Max<T>(this IEnumerable<DataRowGrouping> dataRowGroupings, int columnIndex) 
			where T : struct
		{
			ArgumentGuards.GuardAgainstNullDataRowGroupings(dataRowGroupings);
			ArgumentGuards.GuardAgainstInvalidColumnIndex(dataRowGroupings, columnIndex);

			var maxDataRowPerGroup = new List<(object, DataRow)>();

			foreach(var group in dataRowGroupings)
			{
				var maxRow = FindMaxRowInGroup<T>(group, columnIndex);
				maxDataRowPerGroup.Add((group.Key, maxRow));
			}

			return maxDataRowPerGroup;
		}

		private static DataRow FindMaxRowInGroup<T>(DataRowGrouping group, int columnIndex) where T : struct
		{
			var max = default(T);
			var maxRow = group.First();

			foreach (var row in group)
			{
				var value = row.ItemArray.ElementAt(columnIndex);
				var lhs = TypeChanger.ChangeType<T>(value);

				if (Operators.GreaterThan(lhs, max))
				{
					max = lhs;
					maxRow = row;
				}
			}

			return maxRow;
		}
	}
}
