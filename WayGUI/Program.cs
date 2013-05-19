using System;
using System.Windows.Forms;

namespace WayGUI
{
    using System.ComponentModel;
    using System.IO;
    using System.Security.Principal;
    using WayGUI.Properties;

    static class Program
    {
        internal static MainForm Mainform;
        
        [STAThread] static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Mainform = new MainForm();
            if (!RegistryStuff.GetPythonPath(out PythonHandler.PythonPath))
                GetPythonPath();
            if (!RegistryStuff.GetScriptDirectory(out PythonHandler.WorkingDirectory))
                GetScriptDir();
            Application.Run(Mainform);
        }

        internal static void GetPythonPath() {
            var ofd = new OpenFileDialog {
                                             AddExtension = true,
                                             DefaultExt = "exe",
                                             FileName = "python.exe",
                                             Filter = Resources.FilterExe,
                                             Title = Resources.SelectPythonPath
                                         };
            ofd.FileOk += delegate(object sender, CancelEventArgs args) {
                var fileName = Path.GetFileName(ofd.FileName);
                if (fileName == null || !fileName.Equals("python.exe", StringComparison.CurrentCultureIgnoreCase))
                    args.Cancel = true;
            };
            if (ofd.ShowDialog() == DialogResult.OK)
                PythonHandler.PythonPath = ofd.FileName;
            else {
                MessageBox.Show(Resources.SelectPythonPathInTools);
            }
        }

        internal static void GetScriptDir() {
            var ofd = new OpenFileDialog {
                                             AddExtension = true,
                                             DefaultExt = "py",
                                             FileName = "NORway.py",
                                             Filter = Resources.FilterPy,
                                             Title = Resources.SelectNORwayOrNANDWay
                                         };
            ofd.FileOk += delegate(object sender, CancelEventArgs args) {
                var path = Path.GetDirectoryName(ofd.FileName);
                if (path == null)
                    args.Cancel = true;
                if (!File.Exists(path + "\\NORway.py"))
                    args.Cancel = true;
                if (!File.Exists(path + "\\NANDway.py"))
                    args.Cancel = true;
            };
            if (ofd.ShowDialog() == DialogResult.OK) {
                PythonHandler.WorkingDirectory = Path.GetDirectoryName(ofd.FileName);
                if (!RegistryStuff.SaveScriptDirectory(PythonHandler.WorkingDirectory))
                    MessageBox.Show(Resources.ErrorWhileSavingScriptPath, Resources.ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else {
                MessageBox.Show(Resources.SelectNORwayAndNANDWayInTools);
            }
        }

        internal static bool IsAdmin() {
            var identity = WindowsIdentity.GetCurrent();
            if (identity == null)
                return false;
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
