namespace WayGUI {
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Windows.Forms;

    internal static class PythonHandler {
        internal static string PythonPath = "";
        internal static string WorkingDirectory = "";

        internal static EventHandler<EventArg<string>> Output;
        internal static EventHandler<EventArg<string>> Error;

        private static Process _proc;

        private static void SendOutput(string text) {
            if(Output != null)
                Output(null, new EventArg<string>(text));
        }

        private static void ReadOutput() {
            using(var sr = _proc.StandardOutput) {
                var line = "";
                while(line != null) {
                    line = sr.ReadLine();
                    if(string.IsNullOrEmpty(line))
                        continue;
                    SendOutput(string.Format("{0}{1}", line, Environment.NewLine));
                }
            }
        }

        private static void ReadError() {
            using(var sr = _proc.StandardError) {
                var line = "";
                while(line != null) {
                    line = sr.ReadLine();
                    if(string.IsNullOrEmpty(line))
                        continue;
                    SendError(string.Format("{0}{1}", line, Environment.NewLine));
                }
            }
        }

        private static void SendError(string text) {
            if(Error != null)
                Error(null, new EventArg<string>(text));
        }

        private static void KeepFormFromCrashing(int delaymilliseconds = 100) {
            Application.DoEvents();
            Thread.Sleep(delaymilliseconds);
        }

        internal static int StartProcess(string arguments) {
            _proc = new Process {
                                StartInfo = {
                                            UseShellExecute = false, CreateNoWindow = true, RedirectStandardError = true, RedirectStandardOutput = true, FileName = PythonPath, WorkingDirectory = WorkingDirectory, Arguments = arguments
                                            }
                                };
            SendOutput(string.Format("Python started with:{1}{0}{1}", _proc.StartInfo.Arguments, Environment.NewLine));
            _proc.Start();
            var outreader = new Thread(ReadOutput);
            outreader.Start();
            var errreader = new Thread(ReadError);
            errreader.Start();
            while(!_proc.HasExited)
                KeepFormFromCrashing();
            if(outreader.IsAlive) {
                while(outreader.IsAlive)
                    KeepFormFromCrashing();
            }
            if(errreader.IsAlive) {
                while(errreader.IsAlive)
                    KeepFormFromCrashing();
            }
            return _proc.ExitCode;
        }

        internal static void Kill() {
            if(_proc != null && !_proc.HasExited)
                _proc.Kill();
        }

        #region Nested type: EventArg

        internal sealed class EventArg<T> : EventArgs {
            private readonly T _data;

            public EventArg(T data) {
                _data = data;
            }

            public T Data {
                get { return _data; }
            }
        }

        #endregion
    }
}