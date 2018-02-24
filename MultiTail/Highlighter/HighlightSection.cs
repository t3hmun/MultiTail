namespace T3h.MultiTail.Highlighter
{
    internal class HighlightSection
    {
        public HighlightSection(int startIndex, int length, IHighlighter highlighter)
        {
            StartIndex = startIndex;
            Length = length;
            Highlighter = highlighter;
        }

        public int StartIndex { get; }
        public int Length { get; }
        public int EndIndex => StartIndex + Length;
        public IHighlighter Highlighter { get; }
    }
}