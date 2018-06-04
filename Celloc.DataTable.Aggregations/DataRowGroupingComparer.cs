using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Celloc.DataTable.Aggregations
{
	public class DataRowGroupingComparer : IEqualityComparer<DataRowGrouping>
	{
		public bool Equals(DataRowGrouping x, DataRowGrouping y)
		{
			bool CompareKeys()
			{
				if (x.Key is IEnumerable xKey && y.Key is IEnumerable yKey)
					return xKey.Cast<object>().SequenceEqual(yKey.Cast<object>());

				return Equals(x.Key, y.Key);
			}

			if(x == null && y == null)
				return true;

			if(x == null || y == null)
				return false;

			return CompareKeys() && x.SequenceEqual(y, DataRowComparer.Default);
		}

		public int GetHashCode(DataRowGrouping obj)
		{
			unchecked
			{
				var hash = 0;

				if (obj.Key is IEnumerable key)
					foreach (var element in key)
						hash ^= element.GetHashCode() * 397;
				else
					hash = obj.Key.GetHashCode();

				foreach(var dataRow in obj)
					hash ^= DataRowComparer.Default.GetHashCode(dataRow) * 397;

				return hash;
			}
		}
	}
}
