namespace Egil.RazorComponents.Testing.Rendering.HtmlDomWrappers
{
    /// <summary>
    /// Represents a type that can wrap another type.
    /// </summary>
    public interface IWrapper
    {
        /// <summary>
        /// Marks the wrapped object as stale, and forces the wrapper to retrieve it again.
        /// </summary>
        void MarkAsStale();
    }
}
