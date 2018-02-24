namespace T3h.MultiTail.Highlighter
{
    internal class HighlightSection
    {
        /// <summary>
        ///     A period of a line of text that should have a highlighting applied to it.
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <param name="highlighter"></param>
        public HighlightSection(int startIndex, int length, IHighlight highlighter)
        {
            StartIndex = startIndex;
            Length = length;
            Highlighter = highlighter;
        }

        public int StartIndex { get; }
        public int Length { get; }
        public int EndIndex => StartIndex + Length;
        public IHighlight Highlighter { get; }
    }
}