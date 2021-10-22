using System;
using NUnit.Framework;

namespace Celloc.DataTable.Aggregations.Tests
{
	[TestFixture]
	public class When_calling_sum_on_sum_aggregation_with_a_data_table
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
			var exception = Assert.Throws<ArgumentNullException>(() => SumAggregation.Sum<int>(null, "A1:A1"));
			Assert.AreEqual($"Value cannot be null. (Parameter 'table')", exception.Message);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_range_parameter_is_null()
		{
			var exception = Assert.Throws<ArgumentNullException>(() => SumAggregation.Sum<int>(_DataTable, string.Empty));
			Assert.AreEqual($"Value cannot be null. (Parameter 'range')", exception.Message);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_range_does_not_specify_the_same_column()
		{
			_DataTable.Rows.Add("Value-1", "Value-2");

			var exception = Assert.Throws<ArgumentException>(() => SumAggregation.Sum<int>(_DataTable, "A1:B1"));
			Assert.AreEqual("The specified aggregation can only be performed on a single column.", exception.Message);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_range_tuple_does_not_specify_the_same_column()
		{
			_DataTable.Rows.Add("Value-1", "Value-2");

			var exception = Assert.Throws<ArgumentException>(() => SumAggregation.Sum<int>(_DataTable, ((0,0),(1,0))));
			Assert.AreEqual("The specified aggregation can only be performed on a single column.", exception.Message);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_type_is_not_a_numeric_type()
		{
			_DataTable.Rows.Add("Value-1", "Value-2");

			var exception = Assert.Throws<ArgumentException>(() => SumAggregation.Sum<DateTime>(_DataTable, ((0,0),(0,0))));
			Assert.AreEqual("The specified type is not a numeric type.", exception.Message);
		}

		[Test]
		public void It_should_return_the_default_for_the_type_when_the_range_does_not_exist_in_the_data_table()
		{
			Assert.AreEqual(0, _DataTable.Sum<int>("X1:X1"));
		}

		[Test]
		public void It_should_return_the_default_for_the_type_when_the_range_tuple_does_not_exist_in_the_data_table()
		{
			Assert.AreEqual(0, _DataTable.Sum<int>(((23, 0), (23, 0))));
		}

		[Test]
		public void It_should_sum_the_specified_column()
		{
			_DataTable.Rows.Add("Price", 18.25);
			_DataTable.Rows.Add("Tax", 1.75);
			_DataTable.Rows.Add("Delivery", 5.00);

			var total = _DataTable.Sum<decimal>("B1:B?");

			Assert.AreEqual(25.00M, total);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_value_cannot_be_converted()
		{
			_DataTable.Rows.Add("Price", 18.25);
			_DataTable.Rows.Add("Tax", "1.75%");
			_DataTable.Rows.Add("Delivery", 5.00);

			var exception = Assert.Throws<Exception>(() => _DataTable.Sum<decimal>("B1:B?"));

			Assert.AreEqual("Value 1.75% could not be converted to Decimal.", exception.Message);
		}

		[Test]
		public void It_should_use_the_default_value_when_encountering_a_null_value_in_the_column()
		{
			_DataTable.Rows.Add("Price", 18.25);
			_DataTable.Rows.Add("Tax", null);
			_DataTable.Rows.Add("Delivery", 5.00);

			var total = _DataTable.Sum<decimal>("B1:B?");

			Assert.AreEqual(23.25M, total);
		}
	}

	[TestFixture]
	public class When_calling_sum_on_sum_aggregation_with_a_data_row_grouping
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
			var exception = Assert.Throws<ArgumentNullException>(() => SumAggregation.Sum<int>(null, 0));
			Assert.AreEqual($"Value cannot be null. (Parameter 'dataRowGroupings')", exception.Message);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_column_does_not_exist_in_the_row()
		{
			var grouping = DataRowGroupingBuilder.UseSchema(_DataTable)
				.Key("key")
				.Row("Value-1", 200M)
				.Row("Value-2", 300M)
				.Build();

			var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new [] { grouping }.Sum<int>(3));
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

			var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new [] { grouping }.Sum<int>(-1));
			Assert.AreEqual($"DataRow does not have a column at index -1. (Parameter 'columnIndex')", exception.Message);
		}

		[Test]
		public void It_should_sum_the_values_in_the_specified_column()
		{
			_DataTable.Rows.Add("Price", 22.75);
			_DataTable.Rows.Add("Price", 18.25);
			_DataTable.Rows.Add("Price", 3);
			_DataTable.Rows.Add("Tax", 15);
			_DataTable.Rows.Add("Delivery", 5);

			var groupTotals = _DataTable.GroupBy("A1:A?").Sum<decimal>(1);

			CollectionAssert.AreEquivalent(new [] { 
				("Price", 44M),
				("Tax", 15M),
				("Delivery", 5M)
			}, groupTotals);
		}

		[Test]
		public void It_should_use_the_default_value_when_encountering_a_null_value_in_the_column()
		{
			_DataTable.Rows.Add("Price", 22.75);
			_DataTable.Rows.Add("Price", 18.25);
			_DataTable.Rows.Add("Price", 3);
			_DataTable.Rows.Add("Tax", null);
			_DataTable.Rows.Add("Delivery", 5);

			var groupTotals = _DataTable.GroupBy("A1:A?").Sum<decimal>(1);

			CollectionAssert.AreEquivalent(new [] { 
				("Price", 44M),
				("Tax", 0M),
				("Delivery", 5M)
			}, groupTotals);
		}
	}
}
