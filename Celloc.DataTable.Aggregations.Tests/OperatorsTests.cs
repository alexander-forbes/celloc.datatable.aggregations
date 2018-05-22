using System;
using NUnit.Framework;

namespace Celloc.DataTable.Aggregations.Tests
{
	[TestFixture]
	public class When_calling_add_on_operators
	{
		[Test]
		public void It_should_throw_an_exception_for_a_non_numeric_type()
		{
			var exception = Assert.Throws<ArgumentException>(
				() => Operators.Add(new DateTime(2018, 05, 22), new DateTime(2018, 05, 23))
			);

			Assert.AreEqual("The specified type is not a numeric type.", exception.Message);
		}

		[Test]
		public void It_should_add_the_two_integer_values_together()
		{
			Assert.AreEqual(3, Operators.Add(1, 2));
		}

		[Test]
		public void It_should_add_the_two_decimal_values_together()
		{
			Assert.AreEqual(3M, Operators.Add(1M, 2M));
		}
	}

	[TestFixture]
	public class When_calling_greater_than_on_operators
	{
		[Test]
		public void It_should_return_true_when_lhs_is_greater_than_the_rhs()
		{
			var lhs = new DateTime(2018, 03, 26);
			var rhs = new DateTime(2018, 03, 21);

			Assert.IsTrue(Operators.GreaterThan(lhs, rhs));
		}

		[Test]
		public void It_should_return_false_when_lhs_is_not_greater_than_the_rhs()
		{
			var lhs = new DateTime(2018, 03, 21);
			var rhs = new DateTime(2018, 03, 26);

			Assert.IsFalse(Operators.GreaterThan(lhs, rhs));
		}

		[Test]
		public void It_should_return_true_when_lhs_char_is_greater_than_the_rhs_char()
		{
			const char lhs = 'Z';
			const char rhs = 'A';

			Assert.IsTrue(Operators.GreaterThan(lhs, rhs));
		}

		[Test]
		public void It_should_return_false_when_the_lhs_char_is_not_greater_than_the_rhs_char()
		{
			const char lhs = 'A';
			const char rhs = 'B';

			Assert.IsFalse(Operators.GreaterThan(lhs, rhs));
		}
	}
}
