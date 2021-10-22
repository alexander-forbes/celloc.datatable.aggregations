using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Celloc.DataTable.Aggregations
{
	public static class MinMaxAggregation
	{
		public static DataRow Max<T>(this System.Data.DataTable dataTable, string range)
			where T : struct
		{
			ArgumentGuards.GuardAgainstNullDataTable(dataTable);
			ArgumentGuards.GuardAgainstNullRange(range);

			var rangeTuple = dataTable.TranslateRange(range);

			return rangeTuple.HasValue ? Max<T>(dataTable, rangeTuple.Value) : null;
		}

		public static DataRow Max<T>(this System.Data.DataTable dataTable, ((int Column, int Row), (int Column, int Row)) range)
			where T : struct
		{
			return ApplyOperator<T>(dataTable, range, Operators.GreaterThan);
		}

		public static DataRow Min<T>(this System.Data.DataTable dataTable, string range)
			where T : struct
		{
			ArgumentGuards.GuardAgainstNullDataTable(dataTable);
			ArgumentGuards.GuardAgainstNullRange(range);

			var rangeTuple = dataTable.TranslateRange(range);

			return rangeTuple.HasValue ? Min<T>(dataTable, rangeTuple.Value) : null;
		}

		public static DataRow Min<T>(this System.Data.DataTable dataTable, ((int Column, int Row), (int Column, int Row)) range)
			where T : struct
		{
			return ApplyOperator<T>(dataTable, range, Operators.LessThan);
		}

		private static DataRow ApplyOperator<T>(System.Data.DataTable dataTable, ((int Column, int Row), (int Column, int Row)) range, Func<T, T, bool> @delegate) 
			where T : struct
		{
			ArgumentGuards.GuardAgainstNullDataTable(dataTable);
			ArgumentGuards.GuardAgainstMultipleColumns(range);

			if (!dataTable.Contains(range))
				return null;

			var column = range.Item1.Column;
			var rowIndex = range.Item1.Row;

			var operand = TypeChanger.ChangeType<T>(dataTable.GetValue((column, rowIndex)));

			for(var row = range.Item1.Row; row <= range.Item2.Row; row++)
			{
				var value = dataTable.GetValue((column, row));
				var lhs = TypeChanger.ChangeType<T>(value);

				if (@delegate(lhs, operand))
				{
					operand = lhs;
					rowIndex = row;
				}
			}

			return dataTable.Rows[rowIndex];
		}

		public static IEnumerable<(object, DataRow)> Max<T>(this IEnumerable<DataRowGrouping> dataRowGroupings, int columnIndex) 
			where T : struct
		{
			return ApplyOperator<T>(dataRowGroupings, columnIndex, Operators.GreaterThan);
		}

		public static IEnumerable<(object, DataRow)> Min<T>(this IEnumerable<DataRowGrouping> dataRowGroupings, int columnIndex) 
			where T : struct
		{
			return ApplyOperator<T>(dataRowGroupings, columnIndex, Operators.LessThan);
		}
		
		private static IEnumerable<(object, DataRow)> ApplyOperator<T>(this IEnumerable<DataRowGrouping> dataRowGroupings, int columnIndex, Func<T, T, bool> @delegate) 
			where T : struct
		{
			ArgumentGuards.GuardAgainstNullDataRowGroupings(dataRowGroupings);
			ArgumentGuards.GuardAgainstInvalidColumnIndex(dataRowGroupings, columnIndex);

			var dataRowPerGroup = new List<(object, DataRow)>();

			foreach(var group in dataRowGroupings)
			{
				var row = FindRowInGroup(group, columnIndex, @delegate);
				dataRowPerGroup.Add((group.Key, row));
			}

			return dataRowPerGroup;
		}

		private static DataRow FindRowInGroup<T>(DataRowGrouping group, int columnIndex, Func<T, T, bool> @delegate) 
			where T : struct
		{
			var operand = TypeChanger.ChangeType<T>(group.First().ItemArray.ElementAt(columnIndex));
			var dataRow = group.First();

			foreach (var row in group)
			{
				var value = row.ItemArray.ElementAt(columnIndex);
				var lhs = TypeChanger.ChangeType<T>(value);

				if (@delegate(lhs, operand))
				{
					operand = lhs;
					dataRow = row;
				}
			}

			return dataRow;
		}
	}
}
