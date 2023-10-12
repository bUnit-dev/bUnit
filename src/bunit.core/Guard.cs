using System.Runtime.CompilerServices;

static class Guard
{
    public static T NotNull<T>(
        [NotNull] T? argument,
        [CallerArgumentExpression(nameof(argument))] string? name = null)
    {
        if (argument is null)
        {
            throw new ArgumentNullException(name);
        }

        return argument;
    }

    public static string NotNullOrEmpty(
        [NotNull] string? argument,
        [CallerArgumentExpression(nameof(argument))] string? name = null)
    {
        if (string.IsNullOrEmpty(argument))
        {
            throw new ArgumentNullException(name);
        }

        return argument;
    }
}
