using System;
using System.Data;
using System.Linq;
using NUnit.Framework;

namespace Celloc.DataTable.Aggregations.Tests
{
	[TestFixture]
	public class When_constructing_a_data_row_grouping
	{
		[Test]
		public void It_should_throw_an_exception_for_a_null_key()
		{
			var exception = Assert.Throws<ArgumentNullException>(() => new DataRowGrouping(null, Enumerable.Empty<DataRow>()));
			Assert.AreEqual($"Value cannot be null.{Environment.NewLine}Parameter name: key", exception.Message);
		}

		[Test]
		public void It_should_throw_an_exception_for_null_data_rows()
		{
			var exception = Assert.Throws<ArgumentNullException>(() => new DataRowGrouping("key", null));
			Assert.AreEqual($"Value cannot be null.{Environment.NewLine}Parameter name: dataRows", exception.Message);
		}

		[Test]
		public void It_should_expose_the_key_through_the_key_property()
		{
			const string key = "key";
			var grouping = new DataRowGrouping(key, Enumerable.Empty<DataRow>());
			Assert.AreEqual(key, grouping.Key);
		}

		[Test]
		public void It_should_expose_the_data_rows_through_the_enumerator()
		{
			var table = new System.Data.DataTable();
			var dataRow = table.NewRow();
			table.Rows.Add(dataRow);

			var grouping = new DataRowGrouping("key", table.Rows.OfType<DataRow>());

			Assert.AreEqual(dataRow, grouping.Single());
		}
	}
}
