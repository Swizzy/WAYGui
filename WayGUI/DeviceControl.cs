namespace WayGUI {
    using System;
    using System.Management;
    using System.Threading;
    using System.Windows.Forms;
    using WayGUI.Properties;

    internal static class DeviceControl {
        public static void SetNORWayPort() {
            if(Program.IsAdmin())
                ThreadPool.QueueUserWorkItem(s => GetPorts());
        }

        private static void GetPorts() {
            if(!Program.Mainform.InvokeRequired) {
                try {
                    var searcher = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM MSSerial_PortName");
                    foreach(ManagementObject queryObj in searcher.Get()) {
                        var instanceName = queryObj["InstanceName"].ToString();
                        if(!instanceName.Contains("VID_16C0"))
                            continue;
                        if(!instanceName.Contains("PID_047A"))
                            continue;
                        Program.Mainform.comports.SelectedIndex = Program.Mainform.comports.FindStringExact(queryObj["PortName"].ToString());
                        return;
                    }
                }
                catch(Exception ex) {
                    MessageBox.Show(string.Format(Resources.UnableToSearchForNORWayNANDWayPort, ex.Message));
                }
            }
            else
                Program.Mainform.Invoke((MethodInvoker) GetPorts);
        }
    }
}