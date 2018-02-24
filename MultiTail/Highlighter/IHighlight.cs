namespace T3h.MultiTail.Highlighter
{
    using System;

    internal interface IHighlight
    {
        /// <summary>
        ///     Text colour.
        /// </summary>
        ConsoleColor Foreground { get; }

        /// <summary>
        ///     Text bacground colour.
        /// </summary>
        ConsoleColor Background { get; }

        /// <summary>
        ///     The name of this highlighter, which should be unique. This name is used as the context name for nested highlights.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     The contexts that this highlighter can be applied in.
        /// </summary>
        string[] ValidIn { get; }
    }
}