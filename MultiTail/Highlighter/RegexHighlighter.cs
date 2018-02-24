namespace T3h.MultiTail.Highlighter
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <summary>
    ///     Higlighted implementation based on Regex matching.
    /// </summary>
    internal class RegexHighlighter : IHighlighter
    {
        private readonly Regex _regex;

        /// <summary>
        ///     A highlighter that can be used to match section of text using a regex.
        /// </summary>
        /// <param name="name">The name of this highlight, which is also the context for any nested highlights.</param>
        /// <param name="validIn">The contexts that this highlight is valid in.</param>
        /// <param name="regex">The regex used to match areas that this highlight may apply to.</param>
        /// <param name="foreground">Colour.</param>
        /// <param name="background">Colour.</param>
        public RegexHighlighter(string name, string[] validIn, Regex regex, ConsoleColor foreground,
            ConsoleColor background)
        {
            _regex = regex;
            Foreground = foreground;
            Background = background;
            Name = name;
            ValidIn = validIn;
        }

        /// <inheritdoc />
        public ConsoleColor Foreground { get; }

        /// <inheritdoc />
        public ConsoleColor Background { get; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public string[] ValidIn { get; }

        /// <inheritdoc />
        public IEnumerable<HighlightSection> Apply(string text)
        {
            var match = _regex.Match(text);
            foreach (Capture capture in match.Captures)
                yield return new HighlightSection(capture.Index, capture.Length, this);
        }
    }
}