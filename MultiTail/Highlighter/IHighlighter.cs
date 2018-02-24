namespace T3h.MultiTail.Highlighter
{
    using System.Collections.Generic;

    internal interface IHighlighter : IHighlight
    {
        IEnumerable<HighlightSection> Apply(string text);
    }
}