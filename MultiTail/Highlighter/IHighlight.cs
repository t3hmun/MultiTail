namespace T3h.MultiTail.Highlighter
{
    using System;

    internal interface IHighlight
    {
        ConsoleColor Foreground { get; }
        ConsoleColor Background { get; }
        string Name { get; }
        string[] ValidIn { get; }
    }
}