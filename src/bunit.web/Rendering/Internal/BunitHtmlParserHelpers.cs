using System.Diagnostics.CodeAnalysis;

namespace Bunit.Rendering
{
	internal static class BunitHtmlParserHelpers
	{
		internal static bool StartsWithElements(this string markup, string[] tags, int startIndex, [NotNullWhen(true)] out string? matchedElement)
		{
			matchedElement = null;

			for (int i = 0; i < tags.Length; i++)
			{
				if (markup.StartsWithElement(tags[i], startIndex))
				{
					matchedElement = tags[i];
					return true;
				}
			}

			return false;
		}

		internal static bool StartsWithElement(this string markup, string tag, int startIndex)
		{
			var matchesTag = tag.Length + 1 < markup.Length - startIndex;
			var charIndexAfterTag = tag.Length + startIndex + 1;

			if (matchesTag)
			{
				var charAfterTag = markup[charIndexAfterTag];
				matchesTag = char.IsWhiteSpace(charAfterTag) ||
							 charAfterTag == '>' ||
							 charAfterTag == '/';
			}

			// match characters in tag
			for (int i = 0; i < tag.Length && matchesTag; i++)
			{
				matchesTag = char.ToUpperInvariant(markup[startIndex + i + 1]) == tag[i];
			}

			// look for start tags end - '>'
			for (int i = charIndexAfterTag; i < markup.Length && matchesTag; i++)
			{
				if (markup[i] == '>') break;
			}

			return matchesTag;
		}

		internal static bool IsTagStart(this char c) => c == '<';

		internal static int IndexOfFirstNonWhitespaceChar(this string markup)
		{
			for (int i = 0; i < markup.Length; i++)
			{
				if (!char.IsWhiteSpace(markup, i))
					return i;
			}

			return 0;
		}
	}
}
