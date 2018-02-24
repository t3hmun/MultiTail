namespace T3h.MultiTail
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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
            foreach (var fileSetting in settings.Files)
                tails.Add(new Tail(fileSetting, settings.InitalLinesReadFromEnd, line => win.WriteLine(line)));

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
                    if (slept > settings.UpdateInterval)
                    {
                        update = true;
                        slept = 0;
                    }
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


            var args = Environment.GetCommandLineArgs();
            var files = args.Skip(1).ToArray();


            if (!files.Any())
                win.ErrorQuit($"No file argument supplied. A '.{ConfigExtension}' or file to tail must be supplied.");

            foreach (var file in files)
            {
                if (!File.Exists(file)) win.ErrorQuit($"Supplied file '{file}' does not exist.");

                if (file.EndsWith(ConfigExtension))
                {
                    var userSettings = File.ReadAllText(fullBaseConfPath);
                    JsonConvert.PopulateObject(userSettings, settings);
                }
                else
                {
                    settings.Files.Add(new FileSetting(file));
                }
            }


            return settings;
        }

        private static void OverlayCalculatedDefaultSettings(Settings settings, Window win)
        {
            if (settings.InitiallyReadOneWindow) settings.InitalLinesReadFromEnd = win.Height;
            if (settings.Files == null) settings.Files = new List<FileSetting>();
        }
    }
}