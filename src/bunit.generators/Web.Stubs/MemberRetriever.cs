using Microsoft.CodeAnalysis;

namespace Bunit.Web.Stubs;

internal static class MemberRetriever
{
	public static IEnumerable<ISymbol> GetAllMembersRecursively(this ITypeSymbol type)
	{
		var currentType = type;
		while (currentType is not null)
		{
			foreach (var member in currentType.GetMembers())
			{
				yield return member;
			}
			currentType = currentType.BaseType;
		}
	}
}
