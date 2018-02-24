namespace T3h.MultiTail
{
    using System.Collections.Generic;

    /// <summary>
    ///     Settings for the application
    /// </summary>
    internal class Settings
    {
        /// <summary>
        ///     The initial number of lines at the end of the file to read.
        /// </summary>
        public int InitalLinesReadFromEnd { get; set; }

        /// <summary>
        ///     Overrides <see cref="InitalLinesReadFromEnd" /> with the current hight of the console window.
        /// </summary>
        public bool InitiallyReadOneWindow { get; set; }

        /// <summary>
        ///     Files that are to be tailed.
        /// </summary>
        public List<FileSetting> Files { get; set; } = new List<FileSetting>();

        /// <summary>
        ///     The update interval in ms. This is a backup incase FileSystemWatcher mysteriously fails.
        /// </summary>
        public int UpdateInterval { get; set; }
    }
}