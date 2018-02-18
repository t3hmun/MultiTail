namespace T3h.MultiTail
{
    class HighlightSection
    {
        public int StartIndex { get; }
        public int Length { get; }
        public IHighlighter Highlighter { get; }

        public HighlightSection(int startIndex, int length, IHighlighter highlighter)
        {
            StartIndex = startIndex;
            Length = length;
            Highlighter = highlighter;
        }
    }
}