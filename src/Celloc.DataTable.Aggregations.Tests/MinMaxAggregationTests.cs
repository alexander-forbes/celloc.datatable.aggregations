using System;
using NUnit.Framework;

namespace Celloc.DataTable.Aggregations.Tests
{
	[TestFixture]
	public class When_calling_max_on_min_max_aggregation_with_a_data_table
	{
		private System.Data.DataTable _DataTable;

		[SetUp]
		public void Setup()
		{
			_DataTable = new System.Data.DataTable();
			_DataTable.Columns.Add("Column-1");
			_DataTable.Columns.Add("Column-2");
		}

		[Test]
		public void It_should_throw_an_exception_when_the_table_parameter_is_null()
		{
			var exception = Assert.Throws<ArgumentNullException>(() => MinMaxAggregation.Max<int>(null, ((0, 0), (0, 0))));
			Assert.AreEqual($"Value cannot be null. (Parameter 'table')", exception.Message);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_table_parameter_is_null_for_a_range()
		{
			var exception = Assert.Throws<ArgumentNullException>(() => MinMaxAggregation.Max<int>(null, "A1:A3"));
			Assert.AreEqual($"Value cannot be null. (Parameter 'table')", exception.Message);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_range_tuple_does_not_specify_the_same_column()
		{
			_DataTable.Rows.Add("Value-1", "Value-2");

			var exception = Assert.Throws<ArgumentException>(() => MinMaxAggregation.Max<int>(_DataTable, ((0, 0), (1, 0))));
			Assert.AreEqual("The specified aggregation can only be performed on a single column.", exception.Message);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_range_does_not_specify_the_same_column()
		{
			_DataTable.Rows.Add("Value-1", "Value-2");

			var exception = Assert.Throws<ArgumentException>(() => MinMaxAggregation.Max<int>(_DataTable, "A1:B1"));
			Assert.AreEqual("The specified aggregation can only be performed on a single column.", exception.Message);
		}

		[Test]
		public void It_should_return_a_null_when_the_range_does_not_exist_in_the_data_table()
		{
			Assert.IsNull(_DataTable.Max<int>("X1:X1"));
		}

		[Test]
		public void It_should_find_the_max_value_in_the_given_range_tuple()
		{
			var dataRows = new[]
			{
				_DataTable.LoadDataRow(new object[] {"Item-1", 3.25}, true),
				_DataTable.LoadDataRow(new object[] {"Item-2", 5.75}, true),
				_DataTable.LoadDataRow(new object[] {"Item-3", 2.00}, true)
			};

			var maxDataRow = _DataTable.Max<decimal>(((1, 0), (1, 2)));

			Assert.AreEqual(dataRows[1], maxDataRow);
		}

		[Test]
		public void It_should_find_the_max_value_in_the_given_range()
		{
			var dataRows = new[]
			{
				_DataTable.LoadDataRow(new object[] {"Item-1", 3.25}, true),
				_DataTable.LoadDataRow(new object[] {"Item-2", 5.75}, true),
				_DataTable.LoadDataRow(new object[] {"Item-3", 2.00}, true)
			};

			var maxDataRow = _DataTable.Max<decimal>("B1:B?");

			Assert.AreEqual(dataRows[1], maxDataRow);
		}
	}

	[TestFixture]
	public class When_calling_min_on_min_max_aggregation_with_a_data_table
	{
		private System.Data.DataTable _DataTable;

		[SetUp]
		public void Setup()
		{
			_DataTable = new System.Data.DataTable();
			_DataTable.Columns.Add("Column-1");
			_DataTable.Columns.Add("Column-2");
		}

		[Test]
		public void It_should_throw_an_exception_when_the_table_parameter_is_null()
		{
			var exception = Assert.Throws<ArgumentNullException>(() => MinMaxAggregation.Min<int>(null, ((0, 0), (0, 0))));
			Assert.AreEqual($"Value cannot be null. (Parameter 'table')", exception.Message);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_table_parameter_is_null_for_a_range()
		{
			var exception = Assert.Throws<ArgumentNullException>(() => MinMaxAggregation.Min<int>(null, "A1:A3"));
			Assert.AreEqual($"Value cannot be null. (Parameter 'table')", exception.Message);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_range_tuple_does_not_specify_the_same_column()
		{
			_DataTable.Rows.Add("Value-1", "Value-2");

			var exception = Assert.Throws<ArgumentException>(() => MinMaxAggregation.Min<int>(_DataTable, ((0, 0), (1, 0))));
			Assert.AreEqual("The specified aggregation can only be performed on a single column.", exception.Message);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_range_does_not_specify_the_same_column()
		{
			_DataTable.Rows.Add("Value-1", "Value-2");

			var exception = Assert.Throws<ArgumentException>(() => MinMaxAggregation.Min<int>(_DataTable, "A1:B1"));
			Assert.AreEqual("The specified aggregation can only be performed on a single column.", exception.Message);
		}

		[Test]
		public void It_should_return_a_null_when_the_range_does_not_exist_in_the_data_table()
		{
			Assert.IsNull(_DataTable.Min<int>("X1:X1"));
		}

		[Test]
		public void It_should_find_the_min_value_in_the_given_range_tuple()
		{
			var dataRows = new[]
			{
				_DataTable.LoadDataRow(new object[] {"Item-1", 3.25}, true),
				_DataTable.LoadDataRow(new object[] {"Item-2", 5.75}, true),
				_DataTable.LoadDataRow(new object[] {"Item-3", 2.00}, true)
			};

			var minDataRow = _DataTable.Min<decimal>(((1, 0), (1, 2)));

			Assert.AreEqual(dataRows[2], minDataRow);
		}

		[Test]
		public void It_should_find_the_min_value_in_the_given_range()
		{
			var dataRows = new[]
			{
				_DataTable.LoadDataRow(new object[] {"Item-1", 3.25}, true),
				_DataTable.LoadDataRow(new object[] {"Item-2", 5.75}, true),
				_DataTable.LoadDataRow(new object[] {"Item-3", 2.00}, true)
			};

			var minDataRow = _DataTable.Min<decimal>("B1:B?");

			Assert.AreEqual(dataRows[2], minDataRow);
		}
	}

	[TestFixture]
	public class When_calling_max_on_max_aggregation_with_data_row_groupings
	{
		private System.Data.DataTable _DataTable;

		[SetUp]
		public void Setup()
		{
			_DataTable = new System.Data.DataTable();
			_DataTable.Columns.Add("Column-1");
			_DataTable.Columns.Add("Column-2");
		}

		[Test]
		public void It_should_throw_an_exception_when_the_data_row_groupings_parameter_is_null()
		{
			var exception = Assert.Throws<ArgumentNullException>(() => MinMaxAggregation.Max<int>(null, ((0, 0), (0, 0))));
			Assert.AreEqual($"Value cannot be null. (Parameter 'table')", exception.Message);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_column_does_not_exist_in_the_row()
		{
			var grouping = DataRowGroupingBuilder.UseSchema(_DataTable)
				.Key("key")
				.Row("Value-1", 200M)
				.Row("Value-2", 300M)
				.Build();

			var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new [] { grouping }.Max<int>(3));
			Assert.AreEqual($"DataRow does not have a column at index 3. (Parameter 'columnIndex')", exception.Message);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_column_index_is_less_than_zero()
		{
			var grouping = DataRowGroupingBuilder.UseSchema(_DataTable)
				.Key("key")
				.Row("Value-1", 200M)
				.Row("Value-2", 300M)
				.Build();

			var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new [] { grouping }.Max<int>(-1));
			Assert.AreEqual($"DataRow does not have a column at index -1. (Parameter 'columnIndex')", exception.Message);
		}

		[Test]
		public void It_should_find_the_max_value_in_the_specified_column()
		{
			var dataRows = new[]
			{
				_DataTable.LoadDataRow(new object[] {"Price", 18.25}, true),
				_DataTable.LoadDataRow(new object[] {"Price", 22.75}, true),
				_DataTable.LoadDataRow(new object[] {"Price", 3}, true),
				_DataTable.LoadDataRow(new object[] {"Tax", 15}, true),
				_DataTable.LoadDataRow(new object[] {"Delivery", 5}, true),
				_DataTable.LoadDataRow(new object[] {"Delivery", 7.5}, true)
			};

			var maxPerGroup = _DataTable.GroupBy("A1:A?").Max<decimal>(1);

			CollectionAssert.AreEquivalent(new [] { 
				("Price", dataRows[1]),
				("Tax", dataRows[3]),
				("Delivery", dataRows[5])
			}, maxPerGroup);
		}

		[Test]
		public void It_should_use_the_default_value_when_encountering_a_null_value_in_the_column()
		{
			var dataRows = new[]
			{
				_DataTable.LoadDataRow(new object[] {"Price", 18.25}, true),
				_DataTable.LoadDataRow(new object[] {"Price", 22.75}, true),
				_DataTable.LoadDataRow(new object[] {"Price", 3}, true),
				_DataTable.LoadDataRow(new object[] {"Tax", null}, true),
				_DataTable.LoadDataRow(new object[] {"Delivery", 5}, true),
				_DataTable.LoadDataRow(new object[] {"Delivery", 7.5}, true)
			};

			var maxPerGroup = _DataTable.GroupBy("A1:A?").Max<decimal>(1);

			CollectionAssert.AreEquivalent(new [] { 
				("Price", dataRows[1]),
				("Tax", dataRows[3]),
				("Delivery", dataRows[5])
			}, maxPerGroup);
		}
	}

	[TestFixture]
	public class When_calling_min_on_max_aggregation_with_data_row_groupings
	{
		private System.Data.DataTable _DataTable;

		[SetUp]
		public void Setup()
		{
			_DataTable = new System.Data.DataTable();
			_DataTable.Columns.Add("Column-1");
			_DataTable.Columns.Add("Column-2");
		}

		[Test]
		public void It_should_throw_an_exception_when_the_data_row_groupings_parameter_is_null()
		{
			var exception = Assert.Throws<ArgumentNullException>(() => MinMaxAggregation.Min<int>(null, ((0, 0), (0, 0))));
			Assert.AreEqual($"Value cannot be null. (Parameter 'table')", exception.Message);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_column_does_not_exist_in_the_row()
		{
			var grouping = DataRowGroupingBuilder.UseSchema(_DataTable)
				.Key("key")
				.Row("Value-1", 200M)
				.Row("Value-2", 300M)
				.Build();

			var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new [] { grouping }.Min<int>(3));
			Assert.AreEqual($"DataRow does not have a column at index 3. (Parameter 'columnIndex')", exception.Message);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_column_index_is_less_than_zero()
		{
			var grouping = DataRowGroupingBuilder.UseSchema(_DataTable)
				.Key("key")
				.Row("Value-1", 200M)
				.Row("Value-2", 300M)
				.Build();

			var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new [] { grouping }.Min<int>(-1));
			Assert.AreEqual($"DataRow does not have a column at index -1. (Parameter 'columnIndex')", exception.Message);
		}

		[Test]
		public void It_should_find_the_min_value_in_the_specified_column()
		{
			var dataRows = new[]
			{
				_DataTable.LoadDataRow(new object[] {"Price", 18.25}, true),
				_DataTable.LoadDataRow(new object[] {"Price", 22.75}, true),
				_DataTable.LoadDataRow(new object[] {"Price", 3}, true),
				_DataTable.LoadDataRow(new object[] {"Tax", 15}, true),
				_DataTable.LoadDataRow(new object[] {"Delivery", 7.5}, true),
				_DataTable.LoadDataRow(new object[] {"Delivery", 5}, true)
			};

			var minPerGroup = _DataTable.GroupBy("A1:A?").Min<decimal>(1);

			CollectionAssert.AreEquivalent(new [] { 
				("Price", dataRows[2]),
				("Tax", dataRows[3]),
				("Delivery", dataRows[5])
			}, minPerGroup);
		}

		[Test]
		public void It_should_use_the_default_value_when_encountering_a_null_value_in_the_column()
		{
			var dataRows = new[]
			{
				_DataTable.LoadDataRow(new object[] {"Price", 18.25}, true),
				_DataTable.LoadDataRow(new object[] {"Price", 22.75}, true),
				_DataTable.LoadDataRow(new object[] {"Price", 3}, true),
				_DataTable.LoadDataRow(new object[] {"Tax", null}, true),
				_DataTable.LoadDataRow(new object[] {"Delivery", 5}, true),
				_DataTable.LoadDataRow(new object[] {"Delivery", 7.5}, true)
			};

			var minPerGroup = _DataTable.GroupBy("A1:A?").Min<decimal>(1);

			CollectionAssert.AreEquivalent(new [] { 
				("Price", dataRows[2]),
				("Tax", dataRows[3]),
				("Delivery", dataRows[4])
			}, minPerGroup);
		}
	}
}
