using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Celloc.DataTable.Aggregations
{
	public static class GroupByAggregation
	{
		public static IEnumerable<DataRowGrouping> GroupBy(this System.Data.DataTable dataTable, string range)
		{
			ArgumentGuards.GuardAgainstNullDataTable(dataTable);
			ArgumentGuards.GuardAgainstNullRange(range);

			var rangeTuple = dataTable.TranslateRange(range);

			return rangeTuple.HasValue ? GroupRows(dataTable, rangeTuple.Value) : null;
		}

		public static IEnumerable<DataRowGrouping> GroupBy(this System.Data.DataTable dataTable, ((int Column, int Row), (int Column, int Row)) range)
		{
			ArgumentGuards.GuardAgainstNullDataTable(dataTable);
			return dataTable.Contains(range) ? GroupRows(dataTable, range) : null;
		}

		private static IEnumerable<DataRowGrouping> GroupRows(System.Data.DataTable dataTable, ((int Column, int Row), (int Column, int Row)) range)
		{
			ArgumentGuards.GuardAgainstMultipleColumns(range);

			var groupings = new Dictionary<object, List<DataRow>>();

			for (var row = range.Item1.Row; row <= range.Item2.Row; row++)
			{
				var groupKey = dataTable.GetValue((range.Item1.Column, row));

				if (!groupings.ContainsKey(groupKey))
					groupings.Add(groupKey, new List<DataRow>());

				groupings[groupKey].Add(dataTable.Rows[row]);
			}

			return groupings.Select(kvp => new DataRowGrouping(kvp.Key, kvp.Value));
		}
	}
}
