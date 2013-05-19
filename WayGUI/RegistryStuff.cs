namespace WayGUI
{
    using System.IO;
    using Microsoft.Win32;

    static class  RegistryStuff
    {
        internal static bool GetPythonPath(out string path) {
            var ret = false;
            if (GetStringSetting(out path, "PythonPath")) {
                if (File.Exists(path))
                    return true;
            }
            var key = Registry.ClassesRoot.OpenSubKey("Python.File", RegistryKeyPermissionCheck.ReadSubTree);
            if (key != null) {
                key = key.OpenSubKey("shell");
                if (key != null) {
                    key = key.OpenSubKey("open");
                    if (key != null) {
                        key = key.OpenSubKey("command");
                        if (key != null) {
                            var val = key.GetValue("", "");
                            if (val is string && !string.IsNullOrEmpty(val as string)) {
                                path = val as string;
                                path = path.Substring(1);
                                path = path.Substring(0, path.IndexOf("\"", System.StringComparison.Ordinal));
                                if (File.Exists(path))
                                    ret = true;
                            }
                        }
                    }
                }
            }
            return ret && SetStringSetting(path, "PythonPath");
        }

        internal static bool GetScriptDirectory(out string path) {
            if (GetStringSetting(out path, "ScriptDirectory") && Directory.Exists(path))
                return (File.Exists(string.Format("{0}\\NORway.py", path)) && File.Exists(string.Format("{0}\\NANDWay.py", path)));
            return false;
        }

        internal static bool SaveScriptDirectory(string path) {
            return SetStringSetting(path, "ScriptDirectory");
        }

        private static bool SetStringSetting(string val, string name) {
            var skey = Registry.CurrentUser.OpenSubKey("Software", RegistryKeyPermissionCheck.ReadWriteSubTree);
            if (skey != null)
            {
                skey = skey.CreateSubKey("Swizzy");
                if (skey != null)
                {
                    skey = skey.CreateSubKey("WayGUI");
                    if (skey != null)
                    {
                        skey.SetValue(name, val);
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool GetStringSetting(out string val, string name) {
            val = "";
            var skey = Registry.CurrentUser.OpenSubKey("Software", RegistryKeyPermissionCheck.ReadWriteSubTree);
            if (skey == null)
                return false;
            skey = skey.OpenSubKey("Swizzy");
            if (skey == null)
                return false;
            skey = skey.OpenSubKey("WayGUI");
            if (skey == null)
                return false;
            var tmp = skey.GetValue(name, "");
            if (!(tmp is string) || string.IsNullOrEmpty(tmp as string))
                return false;
            val = tmp as string;
            return true;
        }
    }
}
