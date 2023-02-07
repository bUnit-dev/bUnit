using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Bunit.TestAssets.XunitExtensions;

public static class TheoryDataExtensions
{
	public static TheoryData<T> Clone<T>(this TheoryData<T> existing)
	{
		var result = new TheoryData<T>();
		foreach (var item in existing)
		{
			result.Add((T)item[0]);
		}
		return result;
	}

	public static TheoryData<T> AddRange<T>(this TheoryData<T> theoryData, IEnumerable<T> items)
	{
		foreach (var item in items)
		{
			theoryData.Add(item);
		}
		return theoryData;
	}

	public static TheoryData<T> AddRange<T>(this TheoryData<T> theoryData, params T[] items)
	{
		foreach (var item in items)
		{
			theoryData.Add(item);
		}
		return theoryData;
	}
}
