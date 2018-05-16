using System;
using NUnit.Framework;

namespace Celloc.DataTable.Aggregations.Tests
{
	[TestFixture]
	public class When_calling_sum_on_sum_aggregation
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
			Assert.AreEqual($"Value cannot be null.{Environment.NewLine}Parameter name: table", exception.Message);
		}

		[Test]
		public void It_should_throw_an_exception_when_the_range_parameter_is_null()
		{
			var exception = Assert.Throws<ArgumentNullException>(() => SumAggregation.Sum<int>(_DataTable, string.Empty));
			Assert.AreEqual($"Value cannot be null.{Environment.NewLine}Parameter name: range", exception.Message);
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
		public void It_should_sum_the_specified_range()
		{
			_DataTable.Rows.Add("Price", 18.25);
			_DataTable.Rows.Add("Tax", 1.75);
			_DataTable.Rows.Add("Delivery", 5.00);

			var total = _DataTable.Sum<float>("B1:B?");

			Assert.AreEqual(25.00F, total);
		}
	}
}
