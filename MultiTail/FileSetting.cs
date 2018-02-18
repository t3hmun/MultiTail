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
        }

        public string Filename { get; }
        public string Name { get; }
    }
}