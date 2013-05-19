namespace WayGUI
{
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Ports;
    using System.Reflection;
    using WayGUI.Properties;

    public sealed partial class MainForm : Form {
        private string[] _ports;
        private static bool _protect = true, _initialized;
        private static bool _initNAND0, _initNAND1;
        private bool _allowErase;

        [Flags] private enum MemoryDevice {
            NONE = 0,
            NOR = 1,
            NAND0 = 2,
            NAND1 = 4,
            NANDAuto = NAND0 | NAND1
        }

        private class BWArgs {
            [Flags] internal enum Modes {
                Release = 0,
                Erase = 1,
                Dump = 2,
                Write = 4,
                Verify = 8,
                Diff = 16,
                Word = 32,
                WordUnlockBypassMode = 64
            }

            internal Modes Mode;
            internal readonly string Port;
            internal string Src;
            internal string Path;
            internal int DumpCount;
            internal int Offset;
            internal int Length;
            internal int Address;
            internal readonly MemoryDevice Device = MemoryDevice.NONE;

            public BWArgs(Modes mode, MemoryDevice dev, string port) {
                Mode = mode;
                Device = dev;
                Port = port;
            }
        }

        public MainForm() {
            InitializeComponent();
            var ver = Assembly.GetExecutingAssembly().GetName().Version;
#if DEBUG
            Text = string.Format(Text, ver.Major, ver.Minor, "DEBUG Build");
#elif BETA
            Text = string.Format(Text, ver.Major, ver.Minor, ver.Revision > 0 ? string.Format("BETA{0}", ver.Revision) : "BETA");
#elif ALPHA
            Text = string.Format(Text, ver.Major, ver.Minor, ver.Revision > 0 ? string.Format("ALPHA{0}", ver.Revision) : "ALPHA");
#else
            Text = string.Format(Text, ver.Major, ver.Minor, "");
#endif

#if DEBUG || BETA || ALPHA
            donateToolStripMenuItem.Visible = false;
#else
            donateToolStripMenuItem.Visible = true;
#endif
            PythonHandler.Error += ProcOut;
            PythonHandler.Output += ProcOut;
            Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            DeviceControl.SetUpReceive(Handle);
        }

        private void ProcOut(object sender, PythonHandler.EventArg<string> arg) {
            if (Regex.IsMatch(arg.Data, "^[0-9]* KB / [0-9]+ KB"))
                return;
            AddOutput(arg.Data);
        }
        public void AddOutput(string text) {
            OutputBox.AppendText(text);
            OutputBox.Select(OutputBox.Text.Length, 0);
            OutputBox.ScrollToCaret();
        }

        private void SetAppState(bool busy) {
            KillPython.Enabled = busy;
            operationsbox.Enabled = !busy;
            readbtn.Enabled = !busy && _initialized;
            write.Enabled = !busy && _initialized;
            releasebtn.Enabled = !busy && _initialized;
            settingsbox.Enabled = !busy;
        }

        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);
            switch (m.Msg) {
                case 0x219:
                    if (DeviceControl.CheckIfWayDeviceArrived(ref m))
                        UpdatePorts();
                    else
                        UpdatePorts(false);
                    break;
            }
        }

        private void UpdatePorts(bool scan = true) {
            _ports = SerialPort.GetPortNames();
            if (comports.Items.Count == _ports.Length)
                return;
            comports.DataSource = _ports;
            if (comports.Items.Count > 0 && scan) {
                DeviceControl.SetNORWayPort();
                if (comports.SelectedIndex < comports.Items.Count)
                    comports.SelectedIndex = 0;
            }
            else if (scan)
                comports.Text = "";
        }

        private void Form1Load(object sender, EventArgs e) {
            UpdatePorts();
            memory.Items.AddRange(new object[] {
                                                   new ComboBoxItem<MemoryDevice>(MemoryDevice.NOR,
                                                                                  "NOR"),
                                                   new ComboBoxItem<MemoryDevice>(
                                                       MemoryDevice.NAND0, "NAND 0"),
                                                   new ComboBoxItem<MemoryDevice>(
                                                       MemoryDevice.NAND1, "NAND 1"),
                                                   new ComboBoxItem<MemoryDevice>(
                                                       MemoryDevice.NANDAuto, "NAND 0 & 1 (0 -> 1)")
                                               });
            memory.SelectedIndex = 0;
        }

        private void MemorySelectedIndexChanged(object sender, EventArgs e) {
            var mem = ((ComboBoxItem<MemoryDevice>) memory.SelectedItem).Value;
            erasenorbtn.Enabled = _allowErase && mem == MemoryDevice.NOR && _initialized;
            wordwrite.Enabled = mem == MemoryDevice.NOR;
            wordwriteubm.Enabled = mem == MemoryDevice.NOR;
            noradressbox.Enabled = mem == MemoryDevice.NOR;
            verifynor.Enabled = mem == MemoryDevice.NOR && _initialized;
            diffwrite.Enabled = mem == MemoryDevice.NAND0 || mem == MemoryDevice.NAND1 ||
                                mem == MemoryDevice.NANDAuto;
            offsetbox.Enabled = mem == MemoryDevice.NAND0 || mem == MemoryDevice.NAND1 ||
                                mem == MemoryDevice.NANDAuto;
            NANDInfo.Enabled = (mem == MemoryDevice.NAND0 || mem == MemoryDevice.NAND1 ||
                                mem == MemoryDevice.NANDAuto) && _initialized;
        }

        private void ColinklabelClick(object sender, EventArgs e) {
            Process.Start(Resources.LinkConsoleOpen);
        }

        private static string GetWriteArgs(BWArgs args) {
            var sendargs = "";
            if ((args.Mode & BWArgs.Modes.Verify) == BWArgs.Modes.Verify)
                sendargs = "v";
            switch (args.Device)
            {
                case MemoryDevice.NAND1:
                case MemoryDevice.NAND0:
                    if ((args.Mode & BWArgs.Modes.Diff) == BWArgs.Modes.Diff)
                    {
                        var ofd = new OpenFileDialog
                        {
                            Title = Resources.SelectDiffFileToUse,
                            FileName = string.Format("{0}.diff", Path.GetFileNameWithoutExtension(args.Src)),
                            Filter = Resources.FilterDiff
                        };
                        if (ofd.ShowDialog() != DialogResult.OK)
                        {
                            if (args.Offset >= 0)
                            {
                                if (args.Length > 0)
                                    return string.Format("{0}write \"{1}\" 0x{2:X} 0x{3:X}", sendargs, args.Src, args.Offset, args.Length);
                                return args.Offset > 0 ? string.Format("{0}write \"{1}\" 0x{2:X}", sendargs, args.Src, args.Offset) : string.Format("{0}write \"{1}\"", sendargs, args.Src);
                            }
                        }
                        else
                            return string.Format("{0}writediff \"{1}\" \"{2}\"", sendargs, args.Src, ofd.FileName);
                    }
                    break;
                case MemoryDevice.NOR:
                    if ((args.Mode & BWArgs.Modes.WordUnlockBypassMode) == BWArgs.Modes.WordUnlockBypassMode)
                        sendargs = string.Format("{0}writewordubm \"{1}\"", sendargs, args.Src);
                    else if ((args.Mode & BWArgs.Modes.Word) == BWArgs.Modes.Word)
                        sendargs = string.Format("{0}writeword \"{1}\"", sendargs, args.Src);
                    else
                        sendargs = string.Format("{0}write \"{1}\"", sendargs, args.Src);
                    if (args.Address > 0)
                        sendargs += string.Format(" 0x{0:X}", args.Address);
                    break;
            }
            return sendargs;
        }

        private void BWDoWork(object sender, DoWorkEventArgs e) {
            if (!File.Exists(PythonHandler.PythonPath)) {
                e.Result = Resources.NotFoundPython;
                return;
            }
            if (!(e.Argument is BWArgs))
                return;
            var args = e.Argument as BWArgs;
            var baseArgs = string.Format(args.Device == MemoryDevice.NOR ? "NORway.py {0} " : "NANDWay.py  {0} ", args.Port);

            #region Check Scripts Exists

            switch (args.Device) {
                case MemoryDevice.NOR:
                    if (!File.Exists(PythonHandler.WorkingDirectory + "\\NORway.py")) {
                        e.Result = Resources.NotFoundNORway;
                        return;
                    }
                    break;
                case MemoryDevice.NAND0:
                case MemoryDevice.NAND1:
                case MemoryDevice.NANDAuto:
                    if (!File.Exists(PythonHandler.WorkingDirectory + "\\NANDWay.py")) {
                        e.Result = Resources.NotFoundNANDWay;
                        return;
                    }
                    break;
            }

            #endregion Check Scripts Exists

            var sendargs = "";
            var runit = false;
            switch (args.Mode) {

                #region Release

                case BWArgs.Modes.Release:
                    switch (args.Device) {
                        case MemoryDevice.NOR:
                            sendargs = baseArgs + "release";
                            runit = true;
                            break;
                        case MemoryDevice.NAND0:
                            sendargs = baseArgs + "0 release";
                            runit = true;
                            break;
                        case MemoryDevice.NAND1:
                            sendargs = baseArgs + "1 release";
                            runit = true;
                            break;
                        case MemoryDevice.NANDAuto:
                            var res = new int[2];
                            res[0] = PythonHandler.StartProcess(baseArgs + "0 release");
                            res[1] = PythonHandler.StartProcess(baseArgs + "1 release");
                            e.Result = res;
                            if (res[0] == 0 && res[1] == 0)
                                _initialized = false;
                            break;
                        default:
                            MessageBox.Show(Resources.ErrorHorrible, Resources.ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                    if (runit) {
                        e.Result = PythonHandler.StartProcess(sendargs);
                        if ((int) e.Result == 0)
                            _initialized = false;
                    }
                    break;

                #endregion Release

                #region Dump

                case BWArgs.Modes.Dump:
                    if (args.DumpCount == 1) {
                        switch (args.Device)
                        {
                            #region NOR

                            case MemoryDevice.NOR:
                                sendargs = string.Format("{0}dump \"{1}\"", baseArgs, args.Src);
                                e.Result = PythonHandler.StartProcess(sendargs);
                                return;
                                
                            #endregion

                            #region NAND0

                            case MemoryDevice.NAND0:
                                sendargs = string.Format("{0}0 dump \"{1}\"", baseArgs, args.Src);
                                if (args.Offset != 0) {
                                    sendargs += string.Format(" {0:X}", args.Offset);
                                    if (args.Length != 0)
                                        sendargs += string.Format(" {0:X}", args.Length);
                                }
                                e.Result = PythonHandler.StartProcess(sendargs);
                                return;

                            #endregion NAND0
                                
                            #region NAND1
                            
                            case MemoryDevice.NAND1:
                                sendargs = string.Format("{0}1 dump \"{1}\"", baseArgs, args.Src);
                                if (args.Offset != 0)
                                {
                                    sendargs += string.Format(" {0:X}", args.Offset);
                                    if (args.Length != 0)
                                        sendargs += string.Format(" {0:X}", args.Length);
                                }
                                e.Result = PythonHandler.StartProcess(sendargs);
                                return;

                            #endregion NAND1

                            #region NAND Auto

                            case MemoryDevice.NANDAuto:
                                sendargs = baseArgs + "{0} dump \"" + args.Src + "\"";
                                if (args.Offset != 0)
                                {
                                    sendargs += string.Format(" {0:X}", args.Offset);
                                    if (args.Length != 0)
                                        sendargs += string.Format(" {0:X}", args.Length);
                                }
                                var res = new int[2];
                                res[0] = PythonHandler.StartProcess(string.Format(sendargs, "0"));
                                res[1] = PythonHandler.StartProcess(string.Format(sendargs, "1"));
                                e.Result = res;
                                return;

                            #endregion NAND Auto
                        }
                    }
                    else {
                        var res = new int[args.DumpCount];
                        if (args.Device == MemoryDevice.NANDAuto)
                            res = new int[args.DumpCount * 2];
                        switch (args.Device)
                        {
                            #region NOR

                            case MemoryDevice.NOR:
                                sendargs = string.Format("{0}dump \"{1}\\dump{{0}}.bin\"", baseArgs, args.Path);
                                runit = true;
                                break;

                            #endregion NOR

                            #region NAND0
                            
                            case MemoryDevice.NAND0:
                                sendargs = string.Format("{0} 0 dump \"{1}\\dump{{0}}.bin\"", baseArgs, args.Path);
                                if (args.Offset != 0)
                                {
                                    sendargs += string.Format(" {0:X}", args.Offset);
                                    if (args.Length != 0)
                                        sendargs += string.Format(" {0:X}", args.Length);
                                }
                                runit = true;
                                break;

                            #endregion NAND0

                            #region NAND1

                            case MemoryDevice.NAND1:
                                sendargs = string.Format("{0} 0 dump \"{1}\\dump{{0}}.bin\"", baseArgs, args.Path);
                                if (args.Offset != 0)
                                {
                                    sendargs += string.Format(" {0:X}", args.Offset);
                                    if (args.Length != 0)
                                        sendargs += string.Format(" {0:X}", args.Length);
                                }
                                runit = true;
                                break;
                                
                            #endregion NAND1

                            #region NANDAuto

                            case MemoryDevice.NANDAuto:
                                sendargs = string.Format("{0} {{0}} dump \"{1}\\dump{{1}}.bin\"", baseArgs, args.Path);
                                if (args.Offset != 0)
                                {
                                    sendargs += string.Format(" {0:X}", args.Offset);
                                    if (args.Length != 0)
                                        sendargs += string.Format(" {0:X}", args.Length);
                                }
                                break;

                            #endregion NANDAuto
                        }
                        for (var index = 0; index < args.DumpCount; index++)
                        {
                            if (runit)
                                res[index] = PythonHandler.StartProcess(sendargs);
                            else {
                                res[index] = PythonHandler.StartProcess(string.Format(sendargs, "0", index));
                                res[res.Length / 2 + index] = PythonHandler.StartProcess(string.Format(sendargs, "1", index));
                            }
                        }
                    }
                    break;

                #endregion Dump

                #region Erase

                case BWArgs.Modes.Erase:
                    switch (args.Length)
                    {
                        case 0:
                            e.Result = PythonHandler.StartProcess(string.Format("{0}erasechip", baseArgs));
                            break;
                        default:
                            var res = new int[args.Length];
                            for (var index = 0; index < args.Length; index++)
                                res[index] = PythonHandler.StartProcess(string.Format("{0}erase 0x{1:X}", baseArgs, (index*0x20000) + args.Address));
                            break;
                    }
                    break;

                #endregion Erase

                #region Verify

                case BWArgs.Modes.Verify:
                    switch (args.Length)
                    {
                        case 0:
                            e.Result = PythonHandler.StartProcess(string.Format("{0}verify \"{1}\" 0x{2:X}", baseArgs, args.Src, args.Address));
                            break;
                        default:
                            var res = new int[args.Length];
                            for (var index = 0; index < args.Length; index++)
                                res[index] = PythonHandler.StartProcess(string.Format("{0}verify \"{1}\" 0x{2:X}", baseArgs, args.Src, (index*0x20000) + args.Address));
                            break;
                    }
                    break;

                #endregion Verify

                default:

                #region Write

                    if ((args.Mode & BWArgs.Modes.Write) == BWArgs.Modes.Write) {
                        switch (args.Device) {
                            case MemoryDevice.NOR:
                                sendargs = baseArgs;
                                break;
                            case MemoryDevice.NAND0:
                                sendargs = baseArgs + "0 ";
                                break;
                            case MemoryDevice.NAND1:
                                sendargs = baseArgs + "1 ";
                                break;
                            case MemoryDevice.NANDAuto:
                                e.Result = Resources.ErrorAutoWriteNANDNotSupported;
                                return;
                        }
                        e.Result = PythonHandler.StartProcess(sendargs + GetWriteArgs(args));
                    }

                #endregion Write

                    else
                        e.Result = Resources.ErrorHorrible;
                    break;
            }
        }

        private void BWRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            SetAppState(false);
            if (e.Result is string)
                OutputBox.AppendText(e.Result as string + Environment.NewLine);
            else if (e.Result is int) {
                OutputBox.AppendText((int) e.Result != 0 ? 
                    string.Format("ERROR: Something went wrong with python: {0}{1}", (int) e.Result, Environment.NewLine) : string.Format("Success!{0}", Environment.NewLine));
            }
        }

        private bool GetBWArgs(BWArgs.Modes mode, out BWArgs args) {
            args = null;
            if (comports.SelectedIndex < 0) {
                MessageBox.Show(Resources.SelectPort);
                return false;
            }
            if (memory.SelectedIndex < 0) {
                MessageBox.Show(Resources.SelectMemory);
                return false;
            }
            args = new BWArgs(mode, ((ComboBoxItem<MemoryDevice>) memory.SelectedItem).Value, comports.SelectedItem as string);
            return true;
        }

        private void ReadbtnClick(object sender, EventArgs e) {
            BWArgs args;
            if (!GetBWArgs(BWArgs.Modes.Dump, out args))
                return;
            args.DumpCount = (int) dumpcount.Value;
            if (args.Device == MemoryDevice.NAND0 || args.Device == MemoryDevice.NAND1 ||
                args.Device == MemoryDevice.NANDAuto) {
                args.Offset = (int) offsetbox.Value;
                args.Length = (int) lengthbox.Value;
            }
            if (args.DumpCount > 1) {
                var fsd = new FolderSelectDialog { Title = Resources.SelectSaveDumps };
                if (fsd.ShowDialog())
                    args.Path = fsd.FileName;
                else
                    return;
            }
            else {
                var sfd = new SaveFileDialog {
                                                 Title = Resources.SelectSaveDump,
                                                 CreatePrompt = false,
                                                 FileName = "dump.bin",
                                                 AddExtension = true,
                                                 DefaultExt = "bin",
                                                 Filter = Resources.FilterBin
                                             };
                if (sfd.ShowDialog() == DialogResult.OK)
                    args.Src = sfd.FileName;
                else
                    return;
            }
            SetAppState(true);
            bw.RunWorkerAsync(args);
        }

        private void WriteClick(object sender, EventArgs e) {
            BWArgs args;
            var mode = BWArgs.Modes.Write;
            if (writeverify.Checked)
                mode |= BWArgs.Modes.Verify;
            if (!GetBWArgs(mode, out args))
                return;
            switch (args.Device) {
                case MemoryDevice.NANDAuto:
                case MemoryDevice.NAND1:
                case MemoryDevice.NAND0:
                    args.Offset = (int) offsetbox.Value;
                    args.Length = (int) lengthbox.Value;
                    if (diffwrite.Checked)
                        args.Mode |= BWArgs.Modes.Diff;
                    break;
                case MemoryDevice.NOR:                    
                    args.Address = FixAddress((int)noradressbox.Value);
                    if (wordwrite.Checked)
                        args.Mode |= BWArgs.Modes.Word;
                    if (wordwriteubm.Checked)
                        args.Mode |= BWArgs.Modes.WordUnlockBypassMode;
                    break;
            }
            var ofd = new OpenFileDialog
            {
                Title = Resources.SelectFileToWrite,
                FileName = "dump.bin",
                AddExtension = true,
                DefaultExt = "bin",
                Filter = Resources.FilterBin
            };
            if (ofd.ShowDialog() == DialogResult.OK)
                args.Src = ofd.FileName;
            else
                return;
            SetAppState(true);
            bw.RunWorkerAsync(args);
        }

        private void ErasenorbtnClick(object sender, EventArgs e) {
            BWArgs args;
            if (!GetBWArgs(BWArgs.Modes.Erase, out args))
                return;
            args.Address = (int) noradressbox.Value;
            args.Address = FixAddress((int)noradressbox.Value);
            args.Length = (int) lengthbox.Value;
            SetAppState(true);
            bw.RunWorkerAsync(args);
        }

        private void InitBtnClick(object sender, EventArgs e) {
            if (comports.SelectedIndex < 0)
            {
                MessageBox.Show(Resources.SelectPort);
                return;
            }
            var port = comports.SelectedItem as string;
            if (memory.SelectedIndex < 0)
            {
                MessageBox.Show(Resources.SelectMemory);
                return;
            }
            var mem = ((ComboBoxItem<MemoryDevice>)memory.SelectedItem).Value;
            string args;
            switch (mem) {
                case MemoryDevice.NOR:
                    args = string.Format("NORway.py {0}", port);
                    break;
                case MemoryDevice.NAND0:
                    args = string.Format("NANDWay.py {0} 0", port);
                    break;
                case MemoryDevice.NAND1:
                    args = string.Format("NANDWay.py {0} 1", port);
                    break;
                case MemoryDevice.NANDAuto:
                    args = string.Format("NANDWay.py {0}", port);
                    var res = new int[2];
                    res[0] = PythonHandler.StartProcess(string.Format("{0} 0", args));
                    res[1] = PythonHandler.StartProcess(string.Format("{0} 1", args));
                    _initNAND0 = false;
                    _initNAND1 = false;
                    if (res[0] == 0)
                        _initNAND0 = true;
                    if (res[1] == 0)
                        _initNAND1 = true;
                    else if (res[0] == 0 && res[1] != 0)
                        MessageBox.Show(string.Format(Resources.ErrorInitNAND1FailedNAND0OK, res[1]));
                    else if (res[0] != 0 && res[1] == 0)
                        MessageBox.Show(string.Format(Resources.ErrorInitNAND0FailedNAND1OK, res[0]));
                    else
                        MessageBox.Show(string.Format(Resources.ErrorInitNANDFailed, res[0], res[1]));
                    _initialized = _initNAND0 && _initNAND1;
                    return;
                default:
                    return;
            }
            SetAppState(true);
            var ret = PythonHandler.StartProcess(args);
            switch (ret) {
                case 0:
                    _initialized = true;
                    if (mem == MemoryDevice.NAND0)
                        _initNAND0 = true;
                    if (mem == MemoryDevice.NAND1)
                        _initNAND1 = true;
                    break;
                default:
                    MessageBox.Show(string.Format(Resources.ErrorInitFailed, ret));
                    _initialized = false;
                    break;
            }
            SetAppState(false);
        }

        private void ReleasebtnClick(object sender, EventArgs e) {
            BWArgs args;
            if (!GetBWArgs(BWArgs.Modes.Release, out args))
                return;
            bw.RunWorkerAsync(args);
        }

        private void SwitchProtectionMode(object sender, EventArgs e) {
            switchprotectmode.Text = !_protect
                                         ? Resources.ProtectionDisable
                                         : Resources.ProtectionEnable;
            _protect = !_protect;
        }

        private void SwitchEraseAllowed(object sender, EventArgs e) {
            switchEraseMode.Text = !_allowErase ? Resources.EraseDisable : Resources.EraseEnable;
            _allowErase = !_allowErase;
            MemorySelectedIndexChanged(sender, e);
        }

        private static int FixAddress(int address) {
            var overload = address % 0x20000;
            if (overload > 0 && _protect) {
                if (((address * 0x10)) % 0x20000 == 0) {
                    Program.Mainform.OutputBox.AppendText(string.Format("{2}Address corrected from: 0x{0:X} to 0x{1:X}{2}", address, address * 0x10, Environment.NewLine));
                    return address*0x10;
                }
                if (((address * 0x100) % 0x20000) == 0)
                {
                    Program.Mainform.OutputBox.AppendText(string.Format("{2}Address corrected from: 0x{0:X} to 0x{1:X}{2}", address, address * 0x100, Environment.NewLine));
                    return address * 0x100;
                }
                Program.Mainform.OutputBox.AppendText(string.Format("{2}Address corrected from: 0x{0:X} to 0x{1:X}{2}", address, address - overload, Environment.NewLine));
                return address - overload;
            }
            return address;
        }

        private void SelectPythonPathToolStripMenuItemClick(object sender, EventArgs e) { Program.GetPythonPath(); }

        private void SelectNoRwaypyNANDWaypyPathToolStripMenuItemClick(object sender, EventArgs e) { Program.GetScriptDir(); }

        private void NANDInfoClick(object sender, EventArgs e) {
            if (comports.SelectedIndex < 0) {
                MessageBox.Show(Resources.SelectPort);
                return;
            }
            var port = comports.SelectedItem as string;
            if (memory.SelectedIndex < 0) {
                MessageBox.Show(Resources.SelectMemory);
                return;
            }
            var mem = ((ComboBoxItem<MemoryDevice>) memory.SelectedItem).Value;
            SetAppState(true);
            var res = new int[2];
            if (mem == MemoryDevice.NAND0 || mem == MemoryDevice.NANDAuto)
                res[0] = PythonHandler.StartProcess(string.Format("NANDWay.py {0} 0 info", port));
            if ((mem == MemoryDevice.NAND1 || mem == MemoryDevice.NANDAuto) && res[0] == 0)
                res[1] = PythonHandler.StartProcess(string.Format("NANDWay.py {0} 1 info", port));
            if (res[0] != 0)
                MessageBox.Show(string.Format(Resources.ErrorGettingNandInfo, res[0], 0));
            if (res[1] != 0)
                MessageBox.Show(string.Format(Resources.ErrorGettingNandInfo, res[1], 1));
            SetAppState(false);
        }

        private void VerifyNORClick(object sender, EventArgs e) {
            BWArgs args;
            if (!GetBWArgs(BWArgs.Modes.Verify, out args))
                return;
            args.Address = FixAddress((int)noradressbox.Value);
            args.Length = (int)lengthbox.Value;
            var ofd = new OpenFileDialog
            {
                Title = Resources.SelectVerifyFile,
                FileName = "dump.bin",
                AddExtension = true,
                DefaultExt = "bin",
                Filter = Resources.FilterBin
            };
            if (ofd.ShowDialog() == DialogResult.OK)
                args.Src = ofd.FileName;
            else
                return;
            SetAppState(true);
            bw.RunWorkerAsync(args);
        }

        private void LogstripOpening(object sender, CancelEventArgs e)
        {
            e.Cancel = OutputBox.Text.Length == 0;
        }

        private void SaveLogToolStripMenuItemClick(object sender, EventArgs e) {
            var sfd = new SaveFileDialog {
                                             Title = Resources.SelectSaveLog,
                                             FileName = "Way GUI.log",
                                             AddExtension = true,
                                             DefaultExt = "log",
                                             Filter = Resources.FilterLog
                                         };
            if (sfd.ShowDialog() != DialogResult.OK)
                return;
            File.WriteAllLines(sfd.FileName, OutputBox.Lines);
        }

        private void KillPythonClick(object sender, EventArgs e) { PythonHandler.Kill(); }

        private void MainFormFormClosing(object sender, FormClosingEventArgs e) { DeviceControl.RemoveReceive(); }

        private void DonateToolStripMenuItemClick(object sender, EventArgs e)
        {
            Process.Start(Resources.LinkDonate);
        }

        private void HclinkClick(object sender, EventArgs e) { Process.Start(Resources.LinkHomebrewConnection); }
    }
}