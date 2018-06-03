using System;
using System.Data;
using System.Linq;
using NUnit.Framework;

namespace Celloc.DataTable.Aggregations.Tests
{
	[TestFixture]
	public class When_calling_group_by_on_group_by_aggregation
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
			var exception = Assert.Throws<ArgumentNullException>(() => GroupByAggregation.GroupBy(null, "A1:A1"));
			Assert.AreEqual($"Value cannot be null.{Environment.NewLine}Parameter name: table", exception.Message);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_range_parameter_is_null()
		{
			var exception = Assert.Throws<ArgumentNullException>(() => GroupByAggregation.GroupBy(_DataTable, string.Empty));
			Assert.AreEqual($"Value cannot be null.{Environment.NewLine}Parameter name: range", exception.Message);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_range_does_not_specify_the_same_column()
		{
			_DataTable.Rows.Add("Value-1", "Value-2", "Value-3", "Value-4");

			var exception = Assert.Throws<ArgumentException>(() => GroupByAggregation.GroupBy(_DataTable, "A1:C1"));
			Assert.AreEqual("The specified aggregation can only be performed on a single column.", exception.Message);
		}

		[Test]
		public void It_should_return_null_when_the_range_does_not_exist_in_the_data_table()
		{
			Assert.IsNull(_DataTable.GroupBy("X1:X1"));
		}

		[Test]
		public void It_should_return_null_when_the_range_tuple_does_not_exist_in_the_data_table()
		{
			Assert.IsNull(_DataTable.GroupBy(((23, 1), (23, 1))));
		}

		[Test]
		public void It_should_group_the_rows_by_column_for_the_specified_range()
		{
			_DataTable.Rows.Add("Value-1", "Value-2", "Value-3", "Value-4");
			_DataTable.Rows.Add("Value-5", "Value-6", "Value-7", "Value-8");
			_DataTable.Rows.Add("Value-5", "Value-9", "Value-10", "Value-11");

			var groupings = _DataTable.GroupBy("A1:A?").ToArray();

			var grouping1 = DataRowGroupingBuilder.UseSchema(_DataTable)
				.Key("Value-1")
				.Row("Value-1", "Value-2", "Value-3", "Value-4")
				.Build();

			var grouping2 = DataRowGroupingBuilder.UseSchema(_DataTable)
				.Key("Value-5")
				.Row("Value-5", "Value-6", "Value-7", "Value-8")
				.Row("Value-5", "Value-9", "Value-10", "Value-11")
				.Build();

			var expected = new[] { grouping1, grouping2 };

			Assert.IsTrue(groupings.SequenceEqual(expected, new DataRowGroupingComparer()));
		}

		[Test]
		public void It_should_group_the_rows_by_column_for_the_specified_range_tuple()
		{
			_DataTable.Rows.Add("Value-1", "Value-2", "Value-3", "Value-4");
			_DataTable.Rows.Add("Value-5", "Value-6", "Value-7", "Value-8");
			_DataTable.Rows.Add("Value-5", "Value-9", "Value-10", "Value-11");

			var groupings = _DataTable.GroupBy(((0, 0), (0, 2))).ToArray();

			var grouping1 = DataRowGroupingBuilder.UseSchema(_DataTable)
				.Key("Value-1")
				.Row("Value-1", "Value-2", "Value-3", "Value-4")
				.Build();

			var grouping2 = DataRowGroupingBuilder.UseSchema(_DataTable)
				.Key("Value-5")
				.Row("Value-5", "Value-6", "Value-7", "Value-8")
				.Row("Value-5", "Value-9", "Value-10", "Value-11")
				.Build();

			var expected = new[] { grouping1, grouping2 };

			Assert.IsTrue(groupings.SequenceEqual(expected, new DataRowGroupingComparer()));
		}
	}

	[TestFixture]
	public class When_calling_group_by_on_group_by_aggregation_with_data_row_groupings
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
		public void It_should_throw_an_exception_when_the_data_row_groupings_parameter_is_null()
		{
			var exception = Assert.Throws<ArgumentNullException>(() => GroupByAggregation.GroupBy(null, 0));
			Assert.AreEqual($"Value cannot be null.{Environment.NewLine}Parameter name: dataRowGroupings", exception.Message);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_column_index_is_invalid()
		{
			var grouping = DataRowGroupingBuilder.UseSchema(_DataTable)
				.Key("key")
				.Row("Value-1", 200M)
				.Row("Value-2", 300M)
				.Build();

			var exception = Assert.Throws<ArgumentOutOfRangeException>(() => GroupByAggregation.GroupBy(new [] { grouping }, -1));
			Assert.AreEqual($"DataRow does not have a column at index -1.{Environment.NewLine}Parameter name: columnIndex", exception.Message);
		}

		[Test]
		public void It_should_group_the_data_row_groupings_by_the_specified_column_index()
		{
			_DataTable.Rows.Add("Value-1", "Value-2", "Value-3", "Value-4");
			_DataTable.Rows.Add("Value-5", "Value-2", "Value-7", "Value-8");
			_DataTable.Rows.Add("Value-5", "Value-3", "Value-10", "Value-11");

			var groupings = _DataTable.GroupBy("A1:A?").GroupBy(1).ToArray();

			var grouping1 = DataRowGroupingBuilder.UseSchema(_DataTable)
				.Key("Value-2")
				.Row("Value-1", "Value-2", "Value-3", "Value-4")
				.Build();

			var grouping2 = DataRowGroupingBuilder.UseSchema(_DataTable)
				.Key("Value-2")
				.Row("Value-5", "Value-2", "Value-7", "Value-8")
				.Build();

			var grouping3 = DataRowGroupingBuilder.UseSchema(_DataTable)
				.Key("Value-3")
				.Row("Value-5", "Value-3", "Value-10", "Value-11")
				.Build();

			var expected = new[] { grouping1, grouping2, grouping3 };

			Assert.IsTrue(groupings.SequenceEqual(expected, new DataRowGroupingComparer()));
		}
	}
}
