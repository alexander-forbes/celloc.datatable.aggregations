using System;

namespace Celloc.DataTable.Aggregations
{
	internal class TypeChanger
	{
		public static T ChangeType<T>(object value)
		{
			if (value == DBNull.Value)
				return default(T);

			try
			{
				return (T)Convert.ChangeType(value, typeof(T));
			}
			catch (Exception ex)
			{
				throw new Exception($"Value {value} could not be converted to {typeof(T).Name}.", ex);
			}
		}
	}
}
