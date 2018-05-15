using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Celloc.DataTable.Aggregations
{
	public class DataRowGrouping : IGrouping<object, DataRow>
	{
		private readonly IEnumerable<DataRow> _DataRows;

		public DataRowGrouping(object key, IEnumerable<DataRow> dataRows)
		{
			Key = key ?? throw new ArgumentNullException(nameof(key));
			_DataRows = dataRows ?? throw new ArgumentNullException(nameof(dataRows));
		}

		public object Key { get; }

		public IEnumerator<DataRow> GetEnumerator()
		{
			return _DataRows.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
