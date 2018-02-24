namespace T3h.MultiTail
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using Newtonsoft.Json;

    internal static class Program
    {
        private const string BaseConfig = "defaultSettings.json";
        private const string ConfigExtension = "mtconf";

        public static void Main()
        {
            var win = new Window();

            var settings = LoadSettings(win);

            var tails = new List<Tail>();
            foreach (var fileSetting in settings.File)
                tails.Add(new Tail(fileSetting, settings.ReadTo, line => win.WriteLine(line)));

            var quit = false;
            var slept = 0;
            const int sleepyTime = 250;

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
                    foreach (var tail in tails)
                        tail.Update();
                Thread.Sleep(sleepyTime);
                slept += sleepyTime;
            }
        }


        private static Settings LoadSettings(Window win)
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