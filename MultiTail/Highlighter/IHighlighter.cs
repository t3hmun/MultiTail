namespace T3h.MultiTail.Highlighter
{
    using System.Collections.Generic;

    /// <summary>
    ///     Represents a highligter that can be applied to text.
    /// </summary>
    internal interface IHighlighter : IHighlight
    {
        /// <summary>
        ///     Apply the highlighter to a line of text. The context is not considered at this point in time.
        /// </summary>
        /// <param name="text">A complete line of text.</param>
        /// <returns>All the sections of the text the highlighter might apply to assuming the context is applicable.</returns>
        IEnumerable<HighlightSection> Apply(string text);
    }
}