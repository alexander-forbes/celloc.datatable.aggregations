using NUnit.Framework;

namespace Celloc.DataTable.Aggregations.Tests
{
	[TestFixture]
	public class When_calling_equals_on_data_row_grouping_comparer
	{
		private DataRowGroupingComparer _Comparer;
		private System.Data.DataTable _DataTable;

		[SetUp]
		public void Setup()
		{
			_DataTable = new System.Data.DataTable();
			_Comparer = new DataRowGroupingComparer();
		}

		[Test]
		public void It_should_return_true_when_both_data_row_groupings_are_null()
		{
			Assert.IsTrue(_Comparer.Equals(null, null));
		}

		[Test]
		public void It_should_return_false_when_the_first_data_row_grouping_is_null_and_the_second_is_not()
		{
			var y = DataRowGroupingBuilder.UseSchema(_DataTable).Key("key").Row().Build();
			
			Assert.IsFalse(_Comparer.Equals(null, y));
		}

		[Test]
		public void It_should_return_false_when_the_first_data_row_grouping_is_not_null_and_the_second_is_null()
		{
			var x = DataRowGroupingBuilder.UseSchema(_DataTable).Key("key").Row().Build();

			Assert.IsFalse(_Comparer.Equals(x, null));
		}

		[Test]
		public void It_should_return_false_when_the_keys_are_not_equal()
		{
			var x = DataRowGroupingBuilder.UseSchema(_DataTable).Key("key-1").Row().Build();
			var y = DataRowGroupingBuilder.UseSchema(_DataTable).Key("key-2").Row().Build();

			Assert.IsFalse(_Comparer.Equals(x, y));
		}

		[Test]
		public void It_should_return_false_when_the_keys_are_not_string_and_not_equal()
		{
			var x = DataRowGroupingBuilder.UseSchema(_DataTable).Key(1).Row().Build();
			var y = DataRowGroupingBuilder.UseSchema(_DataTable).Key(2).Row().Build();

			Assert.IsFalse(_Comparer.Equals(x, y));
		}

		[Test]
		public void It_should_return_true_when_the_keys_are_not_string_and_equal()
		{
			var x = DataRowGroupingBuilder.UseSchema(_DataTable).Key(1).Row().Build();
			var y = DataRowGroupingBuilder.UseSchema(_DataTable).Key(1).Row().Build();

			Assert.IsTrue(_Comparer.Equals(x, y));
		}

		[Test]
		public void It_should_return_false_when_the_data_rows_are_not_equal()
		{
			_DataTable.Columns.Add("Column");

			var x = DataRowGroupingBuilder.UseSchema(_DataTable).Key("key-1").Row("Value-1").Build();
			var y = DataRowGroupingBuilder.UseSchema(_DataTable).Key("key-1").Row("Value-2").Build();

			Assert.IsFalse(_Comparer.Equals(x, y));
		}
		
		[Test]
		public void It_should_return_true_when_the_keys_and_data_rows_are_equal()
		{
			_DataTable.Columns.Add("Value");

			var x = DataRowGroupingBuilder.UseSchema(_DataTable).Key("key-1").Row("Value-1").Build();
			var y = DataRowGroupingBuilder.UseSchema(_DataTable).Key("key-1").Row("Value-1").Build();

			Assert.IsTrue(_Comparer.Equals(x, y));
		}

		[Test]
		public void It_should_return_false_when_the_first_key_is_an_enumerable_and_the_second_is_not()
		{
			var x = DataRowGroupingBuilder.UseSchema(_DataTable).Key(new object[] { "key-1", "key-2" }).Row().Build();
			var y = DataRowGroupingBuilder.UseSchema(_DataTable).Key("key-1").Row().Build();

			Assert.IsFalse(_Comparer.Equals(x, y));
		}

		[Test]
		public void It_should_return_false_when_the_second_key_is_an_enumerable_and_the_first_is_not()
		{
			var x = DataRowGroupingBuilder.UseSchema(_DataTable).Key("key-1").Row().Build();
			var y = DataRowGroupingBuilder.UseSchema(_DataTable).Key(new object[] { "key-1", "key-2" }).Row().Build();

			Assert.IsFalse(_Comparer.Equals(x, y));
		}

		[Test]
		public void It_should_return_false_when_both_keys_are_enumerable_and_different()
		{
			var x = DataRowGroupingBuilder.UseSchema(_DataTable).Key(new object[] { "key-1" }).Row().Build();
			var y = DataRowGroupingBuilder.UseSchema(_DataTable).Key(new object[] { "key-1", "key-2" }).Row().Build();

			Assert.IsFalse(_Comparer.Equals(x, y));
		}

		[Test]
		public void It_should_return_true_when_both_keys_are_enumerable_and_the_same()
		{
			var x = DataRowGroupingBuilder.UseSchema(_DataTable).Key(new object[] { "key-1", "key-2" }).Row().Build();
			var y = DataRowGroupingBuilder.UseSchema(_DataTable).Key(new object[] { "key-1", "key-2" }).Row().Build();

			Assert.IsTrue(_Comparer.Equals(x, y));
		}
	}

	[TestFixture]
	public class When_calling_get_hash_code_on_data_row_grouping_comparer
	{
		private System.Data.DataTable _DataTable;
		private DataRowGroupingComparer _Comparer;

		[SetUp]
		public void Setup()
		{
			_DataTable = new System.Data.DataTable();
			_Comparer = new DataRowGroupingComparer();
		}

		[Test]
		public void It_should_return_a_different_hash_code_for_a_different_key()
		{
			var x = DataRowGroupingBuilder.UseSchema(_DataTable).Key("key-1").Row().Build();
			var y = DataRowGroupingBuilder.UseSchema(_DataTable).Key("key-2").Row().Build();

			Assert.AreNotEqual(_Comparer.GetHashCode(x), _Comparer.GetHashCode(y));
		}

		[Test]
		public void It_should_return_a_different_hash_code_for_different_values()
		{
			_DataTable.Columns.Add("Column");

			var x = DataRowGroupingBuilder.UseSchema(_DataTable).Key("key-1").Row("Value-1").Build();
			var y = DataRowGroupingBuilder.UseSchema(_DataTable).Key("key-1").Row("Value-2").Build();

			Assert.AreNotEqual(_Comparer.GetHashCode(x), _Comparer.GetHashCode(y));
		}

		[Test]
		public void It_should_return_a_different_hash_code_for_different_keys()
		{
			_DataTable.Columns.Add("Column");

			var x = DataRowGroupingBuilder.UseSchema(_DataTable).Key(new object[] { "key-1" }).Row("Value-1").Build();
			var y = DataRowGroupingBuilder.UseSchema(_DataTable).Key(new object[] { "key-2" }).Row("Value-1").Build();

			Assert.AreNotEqual(_Comparer.GetHashCode(x), _Comparer.GetHashCode(y));
		}

		[Test]
		public void It_should_return_the_same_hash_code_for_the_same_keys()
		{
			_DataTable.Columns.Add("Column");

			var x = DataRowGroupingBuilder.UseSchema(_DataTable).Key(new object[] { "key-1" }).Row("Value-1").Build();
			var y = DataRowGroupingBuilder.UseSchema(_DataTable).Key(new object[] { "key-1" }).Row("Value-1").Build();

			Assert.AreEqual(_Comparer.GetHashCode(x), _Comparer.GetHashCode(y));
		}

		[Test]
		public void It_should_return_the_same_hash_code_for_the_same_key_and_values()
		{
			_DataTable.Columns.Add("Column");

			var x = DataRowGroupingBuilder.UseSchema(_DataTable).Key("key-1").Row("Value-1").Build();
			var y = DataRowGroupingBuilder.UseSchema(_DataTable).Key("key-1").Row("Value-1").Build();

			Assert.AreEqual(_Comparer.GetHashCode(x), _Comparer.GetHashCode(y));
		}
	}
}
