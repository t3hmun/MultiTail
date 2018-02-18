namespace T3h.MultiTail
{
    using System;
    using System.Collections.Generic;

    interface IHighlighter
    {
        IEnumerable<HighlightSection> Apply(string text);
        ConsoleColor Foreground { get; }
        ConsoleColor Background { get; }
        string Name { get; }
        string[] ValidIn { get; }
    }
}