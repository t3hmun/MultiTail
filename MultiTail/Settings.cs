namespace T3h.MultiTail
{
    using System.Collections.Generic;

    /// <summary>
    ///     Settings for the application
    /// </summary>
    internal class Settings
    {
        public int ReadTo { get; set; }
        public List<FileSetting> File { get; set; } = new List<FileSetting>();
    }
}