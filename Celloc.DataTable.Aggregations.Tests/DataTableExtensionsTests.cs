using System;
using System.Linq;
using NUnit.Framework;

namespace Celloc.DataTable.Aggregations.Tests
{
	[TestFixture]
	public class When_calling_group_by_on_data_table_extensions
	{
		private System.Data.DataTable _DataTable;

		[SetUp]
		public void Setup()
		{
			_DataTable = new System.Data.DataTable();
			_DataTable.Columns.Add("Column-1");
			_DataTable.Columns.Add("Column-2");
			_DataTable.Columns.Add("Column-3");
			_DataTable.Columns.Add("Column-4");
		}

		[Test]
		public void It_should_throw_an_exception_when_the_table_parameter_is_null()
		{
			var exception = Assert.Throws<ArgumentNullException>(() => DataTableExtensions.GroupBy(null, "A1:A1"));
			Assert.AreEqual($"Value cannot be null.{Environment.NewLine}Parameter name: table", exception.Message);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_range_parameter_is_null()
		{
			var exception = Assert.Throws<ArgumentNullException>(() => DataTableExtensions.GroupBy(_DataTable, string.Empty));
			Assert.AreEqual($"Value cannot be null.{Environment.NewLine}Parameter name: range", exception.Message);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_range_does_not_exist_in_the_data_table()
		{
			var exception = Assert.Throws<ArgumentOutOfRangeException>(() => DataTableExtensions.GroupBy(_DataTable, "X1:X?"));
			Assert.AreEqual($"The specified range does not exist in the data table.{Environment.NewLine}Parameter name: range", exception.Message);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_range_does_not_specify_the_same_column()
		{
			_DataTable.Rows.Add("Value-1", "Value-2", "Value-3", "Value-4");
			
			var exception = Assert.Throws<ArgumentException>(() => DataTableExtensions.GroupBy(_DataTable, "A1:C1"));
			Assert.AreEqual("A group by can only be performed on the same column.", exception.Message);
		}

		[Test]
		public void It_should_group_the_rows_by_column_for_the_specified_range()
		{
			_DataTable.Rows.Add("Value-1", "Value-2", "Value-3", "Value-4");
			_DataTable.Rows.Add("Value-5", "Value-6", "Value-7", "Value-8");
			_DataTable.Rows.Add("Value-5", "Value-9", "Value-10", "Value-11");

			var groupings = _DataTable.GroupBy("A1:A?").ToArray();

			var grouping1 = DataRowGroupingBuilder.From(_DataTable)
				.Key("Value-1")
				.Row("Value-1", "Value-2", "Value-3", "Value-4")
				.Build();

			var grouping2 = DataRowGroupingBuilder.From(_DataTable)
				.Key("Value-5")
				.Row("Value-5", "Value-6", "Value-7", "Value-8")
				.Row("Value-5", "Value-9", "Value-10", "Value-11")
				.Build();

			var expected = new[] {grouping1, grouping2};

			Assert.IsTrue(groupings.SequenceEqual(expected, new DataRowGroupingComparer()));
		}
	}
}
