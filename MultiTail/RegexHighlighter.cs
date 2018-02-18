namespace T3h.MultiTail
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    class RegexHighlighter : IHighlighter
    {
        private readonly Regex _regex; 
        public ConsoleColor Foreground { get; }
        public ConsoleColor Background { get; }
        public string Name { get; }
        public string[] ValidIn { get; }

        public RegexHighlighter(string name, string[] validIn, Regex regex,ConsoleColor foreground, ConsoleColor background)
        {
            _regex= regex;
            Foreground = foreground;
            Background = background;
            Name = name;
            ValidIn = validIn;
        }   
        
        public IEnumerable<HighlightSection> Apply(string text)
        {
            var match = _regex.Match(text);
            foreach (Capture capture in match.Captures)
            {
                yield return new HighlightSection(capture.Index, capture.Length, this);
            }
            
        }
    }
}