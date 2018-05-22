using System;
using System.Collections.Generic;
using System.Linq;

namespace Celloc.DataTable.Aggregations
{
	internal static class ArgumentGuards
	{
		public static void GuardAgainstNullRange(string range)
		{
			if (string.IsNullOrEmpty(range))
				throw new ArgumentNullException(nameof(range));
		}

		public static void GuardAgainstNullDataTable(System.Data.DataTable table)
		{
			if (table == null)
				throw new ArgumentNullException(nameof(table));
		}

		public static void GuardAgainstMultipleColumns(((int Column, int Row), (int Column, int Range)) range)
		{
			if (!CellRange.IsSameColumn(range))
				throw new ArgumentException("The specified aggregation can only be performed on a single column.");
		}

		public static void GuardAgainstNonNumericType(Type type)
		{
			if (!IsNumericType(type))
				throw new ArgumentException("The specified type is not a numeric type.");
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

		public static void GuardAgainstNullDataRowGroupings(IEnumerable<DataRowGrouping> dataRowGroupings)
		{
			if (dataRowGroupings == null)
				throw new ArgumentNullException(nameof(dataRowGroupings));
		}

		public static void GuardAgainstInvalidColumnIndex(IEnumerable<DataRowGrouping> dataRowGroupings, int columnIndex)
		{
			if (dataRowGroupings.SelectMany(grouping => grouping).Any(row => columnIndex < 0 || columnIndex > row.ItemArray.Length))
				throw new ArgumentOutOfRangeException(nameof(columnIndex), $"DataRow does not have a column at index {columnIndex}.");
		}
	}
}
