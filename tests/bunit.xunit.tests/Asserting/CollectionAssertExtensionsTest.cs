using System;
using System.Diagnostics.CodeAnalysis;
using Shouldly;
using Xunit;
using Xunit.Sdk;

namespace Bunit
{
	public class CollectionAssertExtensionsTest
	{
		[Fact(DisplayName = "ShouldAllBe for Action<T> throws CollectionException when " +
							"the number of element inspectors does not match the " +
							"number of items in the collection")]
		public void Test001()
		{
			Exception? exception = null;

			var collection = new string[] { "foo", "bar" };
			try
			{
				collection.ShouldAllBe(x => { });
			}
			catch (Exception ex)
			{
				exception = ex;
			}

			var actual = exception.ShouldBeOfType<CollectionException>();
			actual.ActualCount.ShouldBe(collection.Length);
			actual.ExpectedCount.ShouldBe(1);
		}

		[Fact(DisplayName = "ShouldAllBe for Action<T> throws CollectionException if one of " +
							"the element inspectors throws")]
		[SuppressMessage("Minor Code Smell", "S3626:Jump statements should not be redundant", Justification = "Necessary for testing purposes.")]
		public void Test002()
		{
			Exception? exception = null;

			var collection = new string[] { "foo", "bar" };
			try
			{
				collection.ShouldAllBe(x => { }, x => throw new Exception());
			}
			catch (Exception ex)
			{
				exception = ex;
			}

			var actual = exception.ShouldBeOfType<CollectionException>();
			actual.IndexFailurePoint.ShouldBe(1);
		}

		[Fact(DisplayName = "ShouldAllBe for Action<T, int> throws CollectionException when " +
							"the number of element inspectors does not match the " +
							"number of items in the collection")]
		public void Test003()
		{
			Exception? exception = null;

			var collection = new string[] { "foo", "bar" };
			try
			{
				collection.ShouldAllBe((x, i) => { });
			}
			catch (Exception ex)
			{
				exception = ex;
			}

			var actual = exception.ShouldBeOfType<CollectionException>();
			actual.ActualCount.ShouldBe(collection.Length);
			actual.ExpectedCount.ShouldBe(1);
		}

		[Fact(DisplayName = "ShouldAllBe for Action<T, int> throws CollectionException if one of " +
							"the element inspectors throws")]
		[SuppressMessage("Minor Code Smell", "S3626:Jump statements should not be redundant", Justification = "Necessary for testing purposes.")]
		public void Test004()
		{
			Exception? exception = null;

			var collection = new string[] { "foo", "bar" };
			try
			{
				collection.ShouldAllBe((x, i) => { }, (x, i) => throw new Exception());
			}
			catch (Exception ex)
			{
				exception = ex;
			}

			var actual = exception.ShouldBeOfType<CollectionException>();
			actual.IndexFailurePoint.ShouldBe(1);
		}

		[Fact(DisplayName = "ShouldAllBe for Action<T> passes elements to " +
							"the element inspectors in the order of collection")]
		public void Test005()
		{
			var collection = new string[] { "foo", "bar" };

			collection.ShouldAllBe(
				x => x.ShouldBe(collection[0]),
				x => x.ShouldBe(collection[1]));
		}

		[Fact(DisplayName = "ShouldAllBe for Action<T, int> passes elements to " +
					"the element inspectors in the order of collection, " +
				   "with the matching index")]
		public void Test006()
		{
			var collection = new string[] { "foo", "bar" };

			collection.ShouldAllBe(
				(x, i) => { x.ShouldBe(collection[0]); i.ShouldBe(0); },
				(x, i) => { x.ShouldBe(collection[1]); i.ShouldBe(1); });
		}
	}
}
