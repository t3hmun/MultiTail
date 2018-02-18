namespace T3h.MultiTail
{
    internal interface IWriter
    {
        /// <summary>
        /// Write text to console with option appearace modifiers.
        /// </summary>
        /// <param name="text">Text to write.</param>
        /// <param name="highlighters"></param>
        void Write(string text, params IHighlighter[] highlighters);
    }
}