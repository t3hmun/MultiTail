namespace T3h.MultiTail
{
    using Highlighter;

    internal interface IWriter
    {
        /// <summary>
        ///     Write text to console with option appearace modifiers.
        /// </summary>
        /// <param name="text">Text to write.</param>
        /// <param name="highlighters"></param>
        void WriteLine(string text, params IHighlighter[] highlighters);
    }
}