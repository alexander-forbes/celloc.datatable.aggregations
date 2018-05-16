using System;

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

		public static void GuardAgainstMultipleColumns(((int Column, int Row), (int Column, int Range)) tuple)
		{
			if (!Equals(tuple.Item1.Column, tuple.Item2.Column))
				throw new ArgumentException("The specified aggregation can only be performed on a single column.");
		}
	}
}
