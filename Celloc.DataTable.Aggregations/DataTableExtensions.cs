using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Celloc.DataTable.Aggregations
{
	public static class DataTableExtensions
	{
		public static IEnumerable<DataRowGrouping> GroupBy(this System.Data.DataTable dataTable, string range)
		{
			GuardAgainstNullDataTable(dataTable);
			GuardAgainstNullRange(range);
			GuardAgainstInvalidRange(dataTable, range);

			var tuple = dataTable.TranslateRange(range);

			var groupings = new Dictionary<object, List<DataRow>>();
			
			for(var i = tuple.Value.Item1.Row; i <= tuple.Value.Item2.Row; i++)
			{
				var key = dataTable.Rows[i].ItemArray.ElementAt(tuple.Value.Item1.Column);

				if (!groupings.ContainsKey(key))
					groupings.Add(key, new List<DataRow>());
					
				groupings[key].Add(dataTable.Rows[i]);
			}

			return groupings.Select(kvp => new DataRowGrouping(kvp.Key, kvp.Value));
		}

		private static void GuardAgainstInvalidRange(System.Data.DataTable dataTable, string range)
		{
			var tuple = dataTable.TranslateRange(range);

			if (!tuple.HasValue)
				throw new ArgumentOutOfRangeException(nameof(range), "The specified range does not exist in the data table.");

			if (!Equals(tuple.Value.Item1.Column, tuple.Value.Item2.Column))
				throw new ArgumentException("A group by can only be performed on the same column.");
		}

		private static void GuardAgainstNullRange(string range)
		{
			if (string.IsNullOrEmpty(range))
				throw new ArgumentNullException(nameof(range));
		}

		private static void GuardAgainstNullDataTable(System.Data.DataTable table)
		{
			if (table == null)
				throw new ArgumentNullException(nameof(table));
		}
	}
}
