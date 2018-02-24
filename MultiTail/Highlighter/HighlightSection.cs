namespace T3h.MultiTail.Highlighter
{
    internal class HighlightSection
    {
        /// <summary>
        ///     A period of a line of text that should have a highlighting applied to it.
        /// </summary>
        /// <param name="startIndex">Start index on the original body of text that this highlight should be applied.</param>
        /// <param name="length">The length opf text to apply this highlight to.</param>
        /// <param name="highlighter">The highlighter containing details of the colour and where it is applicable.</param>
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