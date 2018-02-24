namespace T3h.MultiTail
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Highlighter;
    using JetBrains.Annotations;

    /// <summary>
    ///     Class that writes to the console in a pretty way.
    /// </summary>
    internal class Window : IWriter
    {
        private readonly ConsoleColor _defualtBackground;
        private readonly ConsoleColor _defualtForeground;

        /// <summary>
        ///     This is intended for use in pretty printing or box art or formatting wrapping.
        /// </summary>
        private int _width;

        public Window()
        {
            Height = Console.WindowHeight;
            _width = Console.WindowWidth;
            _defualtForeground = Console.ForegroundColor;
            _defualtBackground = Console.BackgroundColor;
        }

        public int Height { get; }

        /// <summary>
        ///     Write line of text to the console, colorised by a bunch of highlighters.
        /// </summary>
        /// <param name="text">Line of text</param>
        /// <param name="highlighters">Highlighers.</param>
        public void WriteLine(string text, params IHighlighter[] highlighters)
        {
            var sections = highlighters.Select(h => h.Apply(text)).SelectMany(s => s);

            var changes = GeneratePushPopSequenceFromSections(sections);

            WritePushPopHighlightSequence(text, changes);
        }

        private void WritePushPopHighlightSequence(string text,
            List<(int index, bool push, IHighlighter highlighter, int depth)> changes)
        {
            var changesSorted = changes.OrderBy(c => c.index);

            var prevIndex = 0;
            var currentForeground = _defualtForeground;
            var currentBackground = _defualtBackground;
            var changeStack = new Stack<(int index, bool push, IHighlighter highlighter, int depth)>();
            foreach (var change in changesSorted)
            {
                // Before we make the change write everythign upto this change.
                var toWrite = text.Substring(prevIndex, change.index - prevIndex);
                WriteLine(toWrite, currentForeground, currentBackground);

                if (change.push)
                {
                    if (changeStack.Peek().highlighter.Name == change.highlighter.Name)
                    {
                        // Do nothing, a sections starting within itself is an error.
                        // Better to keep the stack low when somthing is wrong.
                    }
                    else
                    {
                        changeStack.Push(change);
                    }
                }
                else
                {
                    // Only pop somthing if it is in the stack somwhere.
                    // It can be missing if there are bad overlaps.
                    // TODO: Use depth to make a better early cut-off for when sections don't next properly.
                    if (changeStack.Any(c => c.highlighter.Name == change.highlighter.Name))
                        while (true)
                        {
                            var popped = changeStack.Pop();
                            if (popped.highlighter.Name == change.highlighter.Name) break;
                        }
                }

                if (changeStack.Any())
                {
                    var highlighter = changeStack.Peek().highlighter;
                    currentForeground = highlighter.Foreground;
                    currentBackground = highlighter.Background;
                }
                else
                {
                    currentForeground = _defualtForeground;
                    currentBackground = _defualtBackground;
                }

                prevIndex = change.index;
            }

            var finalBit = text.Substring(prevIndex, text.Length - prevIndex);
            WriteLine(finalBit, currentForeground, currentBackground);
        }

        private static List<(int index, bool push, IHighlighter highlighter, int depth)>
            GeneratePushPopSequenceFromSections(IEnumerable<HighlightSection> sections)
        {
            var sortedSections = sections.OrderBy(s => s.StartIndex);

            const string baseContext = "base";
            const string allContext = "all";

            var changes = new List<(int index, bool push, IHighlighter highlighter, int depth)>();
            var sectionStack = new Stack<HighlightSection>();
            foreach (var section in sortedSections)
            {
                var context = sectionStack.Any() ? sectionStack.Peek().Highlighter.Name : baseContext;

                var validContexts = section.Highlighter.ValidIn;
                if (validContexts.Contains(context) || validContexts.Contains(allContext))
                {
                    sectionStack.Push(section);
                    var depth = sectionStack.Count;
                    changes.Add((section.StartIndex, true, section.Highlighter, depth));
                    changes.Add((section.EndIndex, false, section.Highlighter, depth));
                }
            }

            return changes;
        }

        /// <summary>
        ///     Writes a line of text to console.
        /// </summary>
        /// <remarks>
        ///     This is the only direct reference to Console.Write* because I might eventually add soft-wrapping or other
        ///     formatting options, whcihc would all be applied through here.
        /// </remarks>
        /// <param name="text">Text to write.</param>
        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        /// <summary>
        ///     Terminates application. Prints error message and tells the user to press any key to quit.
        /// </summary>
        [ContractAnnotation("=> halt")]
        public void ErrorQuit(string errorMessage)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(errorMessage);
            Console.WriteLine("Press any key to quit.");
            Console.ReadKey();
            Environment.Exit(1);
        }

        public void WriteLine(string text, ConsoleColor foreground, ConsoleColor background)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            WriteLine(text);
        }
    }
}