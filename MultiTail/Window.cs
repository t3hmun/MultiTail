namespace T3h.MultiTail
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    ///     Class that writes to the console in a pretty way.
    /// </summary>
    internal class Window
    {
        private int _width;

        public Window()
        {
            Height = Console.WindowHeight;
            _width = Console.WindowWidth;
        }

        public int Height { get; }

        public void Write(string text)
        {
            Console.WriteLine(text);
        }

        /// <summary>
        ///     Terminates application. Prints error message and tells the user to press any key to quit.
        /// </summary>
        [ContractAnnotation("=> halt")]
        public void ErrorQuit(string errorMessage)
        {
            Console.WriteLine(errorMessage);
            Console.WriteLine("Press any key to quit.");
            Console.ReadKey();
            Environment.Exit(1);
        }
    }
}