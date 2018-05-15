using System.Data;
using System.Linq;

namespace Celloc.DataTable.Aggregations.Tests
{
	internal static class DataRowGroupingBuilder
	{
		public static KeyBuilder From(System.Data.DataTable schemaDataTable)
		{
			var table = new System.Data.DataTable();

			foreach (DataColumn column in schemaDataTable.Columns)
				table.Columns.Add(column.ColumnName);

			return new KeyBuilder(table);
		}

		internal class KeyBuilder
		{
			private readonly System.Data.DataTable _DataTable;

			internal KeyBuilder(System.Data.DataTable dataTable)
			{
				_DataTable = dataTable;
			}

			public RowBuilder Key(object key)
			{
				return new RowBuilder(_DataTable, key);
			}
		}

		internal class RowBuilder
		{
			private readonly System.Data.DataTable _DataTable;
			private readonly object _Key;

			public RowBuilder(System.Data.DataTable dataTable, object key)
			{
				_DataTable = dataTable;
				_Key = key;
			}

			public RowBuilder Row(params object[] values)
			{
				_DataTable.Rows.Add(values);
				return this;
			}

			public DataRowGrouping Build()
			{
				return new DataRowGrouping(_Key, _DataTable.Rows.OfType<DataRow>());
			}
		}
	}
}
