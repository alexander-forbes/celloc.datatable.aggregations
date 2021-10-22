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

		public static IEnumerable<DataRowGrouping> GroupBy(this System.Data.DataTable dataTable, int[] columns)
		{
			ArgumentGuards.GuardAgainstNullDataTable(dataTable);

			var groupings = new Dictionary<IEnumerable<object>, List<DataRow>>();

			foreach (DataRow row in dataTable.Rows)
			{
				var rowKey = columns.Select(index => row.ItemArray.ElementAt(index));
				var matchingKey = groupings.Keys.SingleOrDefault(grp => grp.SequenceEqual(rowKey));

				if (matchingKey == null)
					groupings.Add(rowKey, new List<DataRow> { row });
				else
					groupings[matchingKey].Add(row);
			}

			return groupings.Select(kvp => new DataRowGrouping(kvp.Key, kvp.Value));
		}

		public static IEnumerable<DataRowGrouping> GroupBy(this IEnumerable<DataRowGrouping> dataRowGroupings, int columnIndex)
		{
			ArgumentGuards.GuardAgainstNullDataRowGroupings(dataRowGroupings);
			ArgumentGuards.GuardAgainstInvalidColumnIndex(dataRowGroupings, columnIndex);

			var groupings = new List<DataRowGrouping>();

			foreach (var grouping in dataRowGroupings)
			{
				var groupRows = new Dictionary<object, List<DataRow>>();

				foreach (var row in grouping)
				{
					var groupKey = row.ItemArray.ElementAt(columnIndex);

					if (!groupRows.ContainsKey(groupKey))
						groupRows.Add(groupKey, new List<DataRow>());

					groupRows[groupKey].Add(row);
				}

				var enumerable = groupRows.Select(kvp => new DataRowGrouping(kvp.Key, kvp.Value));
				groupings.AddRange(enumerable);
			}

			return groupings;
		}
	}
}
