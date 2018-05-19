using NUnit.Framework;

namespace Celloc.DataTable.Aggregations.Tests
{
	[TestFixture]
	public class When_calling_add_on_math_operators
	{
		[Test]
		public void It_should_add_the_two_values_together()
		{
			Assert.AreEqual(3, MathOperators.Add(1, 2));
		}
	}
}
