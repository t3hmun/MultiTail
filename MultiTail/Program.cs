namespace T3h.MultiTail
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using Newtonsoft.Json;

    internal static class Program
    {
        private const string BaseConfig = "default.json";
        private const string ConfigExtension = "mtconf";

        public static void Main()
        {
            var win = new Window();

            var settings = ParseArgs(win);

            var streams = new List<FileStream>();
            foreach (var fileSetting in settings.File)
            {
                var fs = new FileStream(file.Filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                streams.Add(fs);
            }

            var quit = false;
            var slept = 0;

            while (!quit)
            {
                var update = false;
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Q) Environment.Exit(0);

                    if (key.Key == ConsoleKey.Enter) update = true;
                }
                else
                {
                    update = slept > settings.UpdateInterval;
                    slept = 0;
                }

                if (update)
                    foreach (var stream in streams)
                    {
                    }

                Thread.Sleep(50);
                slept += 50;
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


        private class Tail
        {
            public Tail(FileStream stream, int readTo, IWriter writer)
            {
                Stream = stream;
            }

            public int LastPosition { get; set; }
            public FileStream Stream { get; }

            public void Read()
            {
            }
        }
    }
}