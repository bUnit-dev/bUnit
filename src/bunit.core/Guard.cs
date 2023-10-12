using System.Runtime.CompilerServices;

static class Guard
{
    public static T NotNull<T>(
        [NotNull] T? argument,
        [CallerArgumentExpression("argument")] string? name = null)
    {
        if (argument is null)
        {
            throw new ArgumentNullException(name);
        }

        return argument;
    }
}
