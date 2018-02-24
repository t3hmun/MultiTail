namespace T3h.MultiTail.Highlighter
{
    using System;
    using System.Collections.Generic;

    internal interface IHighlighter
    {
        ConsoleColor Foreground { get; }
        ConsoleColor Background { get; }
        string Name { get; }
        string[] ValidIn { get; }
        IEnumerable<HighlightSection> Apply(string text);
    }
}