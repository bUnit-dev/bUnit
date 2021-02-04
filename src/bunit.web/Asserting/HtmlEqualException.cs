using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using AngleSharp;
using AngleSharp.Diffing.Core;
using Bunit;
using Bunit.Asserting;

namespace Bunit
{
	/// <summary>
	/// Represents an differences between pieces of markup.
	/// </summary>
	[Serializable]
	public sealed class HtmlEqualException : ActualExpectedAssertException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="HtmlEqualException"/> class.
		/// </summary>
		public HtmlEqualException(IEnumerable<IDiff> diffs, IMarkupFormattable expected, IMarkupFormattable actual, string? userMessage)
			: base(PrintHtml(actual), PrintHtml(expected), "Actual HTML", "Expected HTML", CreateUserMessage(diffs, userMessage))
		{
		}

		private static string CreateUserMessage(IEnumerable<IDiff> diffs, string? userMessage)
		{
			return $"HTML comparison failed. {userMessage}{Environment.NewLine}{Environment.NewLine}The following errors were found:{Environment.NewLine}{PrintDiffs(diffs)}";
		}

		[SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Node types should be in lower case.")]
		private static string PrintDiffs(IEnumerable<IDiff> diffs)
		{
			return string.Join(Environment.NewLine, diffs.Select((x, i) =>
			{
				var diffText = x switch
				{
					NodeDiff diff when diff.Target == DiffTarget.Text && diff.Control.Path.Equals(diff.Test.Path, StringComparison.Ordinal)
						=> $"The text in {diff.Control.Path} is different.",
					NodeDiff diff when diff.Target == DiffTarget.Text
						=> $"The expected {NodeName(diff.Control)} at {diff.Control.Path} and the actual {NodeName(diff.Test)} at {diff.Test.Path} is different.",
					NodeDiff diff when diff.Control.Path.Equals(diff.Test.Path, StringComparison.Ordinal)
						=> $"The {NodeName(diff.Control)}s at {diff.Control.Path} are different.",
					NodeDiff diff => $"The expected {NodeName(diff.Control)} at {diff.Control.Path} and the actual {NodeName(diff.Test)} at {diff.Test.Path} are different.",
					AttrDiff diff when diff.Control.Path.Equals(diff.Test.Path, StringComparison.Ordinal)
						=> $"The values of the attributes at {diff.Control.Path} are different.",
					AttrDiff diff => $"The value of the attribute {diff.Control.Path} and actual attribute {diff.Test.Path} are different.",
					MissingNodeDiff diff => $"The {NodeName(diff.Control)} at {diff.Control.Path} is missing.",
					MissingAttrDiff diff => $"The attribute at {diff.Control.Path} is missing.",
					UnexpectedNodeDiff diff => $"The {NodeName(diff.Test)} at {diff.Test.Path} was not expected.",
					UnexpectedAttrDiff diff => $"The attribute at {diff.Test.Path} was not expected.",
					_ => throw new InvalidOperationException($"Unknown diff type detected: {x.GetType()}"),
				};
				return $"  {i + 1}: {diffText}";
			})) + Environment.NewLine;

			static string NodeName(ComparisonSource source) => source.Node.NodeType.ToString().ToLowerInvariant();
		}

		private HtmlEqualException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext) { }

		private static string PrintHtml(IMarkupFormattable nodes) => nodes.ToDiffMarkup() + Environment.NewLine;
	}
}
