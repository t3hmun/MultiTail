namespace T3h.MultiTail
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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

            var tail = ReadTailLinesWithLastLineIncomplete(initialReadAmountFromEnd);

            for (var i = 0; i < tail.Length - 1; i++)
            {
                var lineToWrite = tail[i];
                _writeLine(lineToWrite);
            }

            string partialLine = tail.LastOrDefault();

            _fsw = new FileSystemWatcher
            {
                Path = settings.Dir,
                Filter = settings.Filename
            };

            _fsw.Changed += (obj, args) => { _waiter.Set(); };
            _fsw.EnableRaisingEvents = true;

            _thread = new Thread(() => ReadUntilKill(partialLine));
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

        private string[] ReadTailLinesWithLastLineIncomplete(int linesFromEndToKeep)
        {
            // Seeking position in a file that could be any text encoding isn't trivial.
            // StreamReader.ReadLine is wrong, it wont tell you if the last line ended (has a \n).
            // So I've chosen to read the whole file in alternating buffers, its good enough.
            // Also we have no idea where in the file the last x lines start.
            
            // 1048576 is a mebibyte so 16 mebibytes of ram (16 bit char).
            // So 2 buffers will allocate 32mib of RAM before starting to read what will probably be a 1kib log file.
            // Seems legit.
            // This baroque contraption should quickly ignore most of a 1gb monster log.
            // But no so efficiently that I have to think about how encoding works (and then do it wrong anyway).
            const int bufferLen = 1048576;
            
            int charsRead;
            var bufferA = new char[bufferLen];
            var bufferB = new char[bufferLen];
            char[] penultimatePiece;
            var lastPiece = bufferB;
            long totalLen = 0; // an int will only store 1gib of chars.
            do
            {
                charsRead = _sr.ReadBlock(bufferA, 0, bufferLen);
                totalLen += charsRead;
                penultimatePiece = lastPiece;
                lastPiece = bufferA;

                if (charsRead != bufferLen) continue;
                
                charsRead = _sr.ReadBlock(bufferB, 0, bufferLen);
                totalLen += charsRead;
                penultimatePiece = lastPiece;
                lastPiece = bufferB;

            } while (charsRead == bufferLen);

            var vaguelySensibleNumOfChars = 16384; //32 kibibytes, this many chars fills my monitor, close enough.
            if (totalLen < vaguelySensibleNumOfChars) vaguelySensibleNumOfChars = (int)totalLen;

            var text = charsRead > bufferLen
                ? new string(penultimatePiece.Concat(lastPiece.Take(charsRead))
                    .Take(vaguelySensibleNumOfChars)
                    .ToArray())
                : new string(lastPiece.Take(vaguelySensibleNumOfChars).ToArray());

            var lines = text.Split('\n');
            var lineCount = lines.Length;
            var start = lineCount < linesFromEndToKeep ? 0 : lineCount - linesFromEndToKeep;
            return lines.Skip(start).ToArray();
        }

        public void Update()
        {
            _waiter.Set();
        }

        private void ReadUntilKill(string partialLine)
        {
            // We don't use readline because it reads incomplete lines without telling us that there was no \n.
            while (!_kill)
            {
                var text = _sr.ReadToEnd();
                if (text == "")
                {
                    _waiter.WaitOne();
                }
                else if (!text.Contains('\n'))
                {
                    //If it doesn't contain a newline then it isn't a new line.
                    partialLine += text; //null+="..." seems to work fine, who knew?
                }

                text = text.Replace("\r", "");
                    var lines = text.Split('\n');

                    var line = _sr.ReadLine();
                    if (line == null)
                    {
                        _waiter.WaitOne();
                    }
                    else
                    {
                        _writeLine(line);
                    }
                }
            }
        }
    }
}