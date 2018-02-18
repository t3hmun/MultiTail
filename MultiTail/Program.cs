namespace T3h.MultiTail
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Newtonsoft.Json;

    internal static class Program
    {
        private const string BaseConfig = "default.mtconf";
        private const string ConfigExtension = "mtconf";

        public static void Main()
        {
            var win = new Window();

            var settings = ParseArgs(win);
        }

        private static void Tail(FileSetting file)
        {
            using (var fs = new FileStream(file.Filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
            }
        }


        private static Settings ParseArgs(Window win)
        {
            var fullBaseConfPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, BaseConfig);
            if (!File.Exists(fullBaseConfPath)) win.ErrorQuit($"Base config '{fullBaseConfPath}' missing.");

            var baseSettings = File.ReadAllText(fullBaseConfPath);
            var settings = JsonConvert.DeserializeObject<Settings>(baseSettings);

            OverlayCalculatedDefaultSettings(settings, win);

            var arg = Environment.CommandLine;
            var argFile = arg.Trim().Trim('"');

            if (string.IsNullOrEmpty(argFile))
                win.ErrorQuit($"No file argument supplied. A '.{ConfigExtension}' or file to tail must be supplied.");

            if (!File.Exists(argFile)) win.ErrorQuit($"Supplied file '{argFile}' does not exist.");

            if (argFile.EndsWith(ConfigExtension))
            {
                var userSettings = File.ReadAllText(fullBaseConfPath);
                JsonConvert.PopulateObject(userSettings, settings);
            }
            else
            {
                settings.File.Add(new FileSetting(argFile));
            }

            return settings;
        }

        private static void OverlayCalculatedDefaultSettings(Settings settings, Window win)
        {
            if (settings.ReadTo <= 0) settings.ReadTo = win.Height;
            if (settings.File == null) settings.File = new List<FileSetting>();
        }
    }
}