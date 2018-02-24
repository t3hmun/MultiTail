namespace T3h.MultiTail
{
    using System.IO;

    /// <summary>
    ///     A file that is tailed.
    /// </summary>
    internal class FileSetting
    {
        public FileSetting(string filename, string name = null)
        {
            var fi = new FileInfo(filename);
            Filename = fi.FullName;
            Name = name ?? fi.Name;
            Dir = fi.Directory.FullName;
        }

        /// <summary>
        ///     The full filename with path.
        /// </summary>
        public string Filename { get; }

        /// <summary>
        ///     Just the filename, no path.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     The full directory with the path.
        /// </summary>
        public string Dir { get; }
    }
}