namespace T3h.MultiTail
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;

    internal class Tail : IDisposable
    {
        private readonly FileStream _fs;
        private readonly FileSystemWatcher _fsw;
        private readonly StreamReader _sr;
        private readonly Thread _thread;

        private readonly AutoResetEvent _waiter = new AutoResetEvent(true);
        private readonly Action<string> _writeLine;
        private bool _kill;


        public Tail(FileSetting settings, int initialReadAmountFromEnd, Action<string> writeLine)
        {
            _writeLine = writeLine;
            _fs = new FileStream(settings.Filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            _sr = new StreamReader(_fs);


            // The queue allows us to run through the file without loading all of it to memory.
            // Not really necessary most of the time.
            // While I could seek the underlying filw stream, finding lines gets complicated and not worth the effort.
            // Personally I don't care about this overhead.
            var queue = new Queue<string>();
            string line;
            while ((line = _sr.ReadLine()) != null)
            {
                queue.Enqueue(line);
                if (queue.Count > initialReadAmountFromEnd) queue.Dequeue();
            }

            foreach (var lineToWrite in queue) _writeLine(lineToWrite);

            _fsw = new FileSystemWatcher
            {
                Path = settings.Dir,
                Filter = settings.Filename
            };

            _fsw.Changed += (obj, args) =>
            {
                _waiter.Set();
            };
            _fsw.EnableRaisingEvents = true;
            
            _thread = new Thread(ReadUntilKill);
            _thread.Start();
            
        }

        public void Dispose()
        {
            _kill = true;
            _waiter?.Set();
            _thread?.Join(1000);
            _fsw?.Dispose();
            _sr?.Dispose();
            _fs?.Dispose();
        }

        public void Update()
        {
            _waiter.Set();
        }

        private void ReadUntilKill()
        {
            while (!_kill)
            {
                var line = _sr.ReadLine();
                if (line == null)
                    _waiter.WaitOne();
                else
                    _writeLine(line);
            }
        }
    }
}