using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Celloc.DataTable.Aggregations
{
	public class DataRowGroupingComparer : IEqualityComparer<DataRowGrouping>
	{
		public bool Equals(DataRowGrouping x, DataRowGrouping y)
		{
			if(x == null && y == null)
				return true;

			if(x == null || y == null)
				return false;

			return Equals(x.Key, y.Key) && 
			       x.SequenceEqual(y, DataRowComparer.Default);
		}

		public int GetHashCode(DataRowGrouping obj)
		{
			unchecked
			{
				var hash = obj.Key.GetHashCode();

				foreach(var dataRow in obj)
					hash ^= DataRowComparer.Default.GetHashCode(dataRow) * 397;

				return hash;
			}
		}
	}
}
