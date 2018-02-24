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
        public int ReadTo { get; set; }

        /// <summary>
        ///     Files that are to be tailed.
        /// </summary>
        public List<FileSetting> File { get; set; } = new List<FileSetting>();

        /// <summary>
        ///     The update interval in ms.
        /// </summary>
        public int UpdateInterval { get; set; }
    }
}