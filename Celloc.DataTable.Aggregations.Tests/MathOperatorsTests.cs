using System;
using NUnit.Framework;

namespace Celloc.DataTable.Aggregations.Tests
{
	[TestFixture]
	public class When_calling_add_on_math_operators
	{
		[Test]
		public void It_should_throw_an_exception_for_a_non_numeric_type()
		{
			var exception = Assert.Throws<ArgumentException>(
				() => MathOperators.Add(new DateTime(2018, 05, 22), new DateTime(2018, 05, 23))
			);

			Assert.AreEqual("The specified type is not a numeric type.", exception.Message);
		}

		[Test]
		public void It_should_add_the_two_integer_values_together()
		{
			Assert.AreEqual(3, MathOperators.Add(1, 2));
		}

		[Test]
		public void It_should_add_the_two_decimal_values_together()
		{
			Assert.AreEqual(3M, MathOperators.Add(1M, 2M));
		}
	}
}
