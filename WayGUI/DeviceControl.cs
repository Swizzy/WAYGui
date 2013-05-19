namespace WayGUI
{
    using System;
    using System.Management;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;
    using WayGUI.Properties;

    static class DeviceControl
    {
        public static void SetNORWayPort() {
            ThreadPool.QueueUserWorkItem(s => GetPorts());
        }

        private static void GetPorts() {
            if (!Program.Mainform.InvokeRequired) {
                try
                {
                    var searcher = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM MSSerial_PortName");
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        var instanceName = queryObj["InstanceName"].ToString();
                        if (!instanceName.Contains("VID_16C0"))
                            continue;
                        if (!instanceName.Contains("PID_047A"))
                            continue;
                        Program.Mainform.comports.SelectedIndex = Program.Mainform.comports.FindStringExact(queryObj["PortName"].ToString());
                        return;
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show(string.Format(Resources.UnableToSearchForNORWayNANDWayPort, ex.Message));
                }
            }
            else {
                Program.Mainform.Invoke((MethodInvoker)GetPorts);
            }
        }

        internal static bool CheckIfWayDeviceArrived(ref Message m) {
#if DEBUG
            Program.Mainform.AddOutput("Device change occured (checking params)...\r\n");
#endif
            if (m.WParam.ToInt32() != 0x8000 || m.LParam == IntPtr.Zero)
                return false;
#if DEBUG
            Program.Mainform.AddOutput("Device arrival detected...\r\n");
#endif
            var hdr = (DevBroadcastDeviceInterfaceBuffer)Marshal.PtrToStructure(m.LParam, typeof(DevBroadcastDeviceInterfaceBuffer));
#if DEBUG
            if (hdr.dbch_devicetype == 3)
                Program.Mainform.AddOutput("Device has Serial Port...\r\n");
#endif
            return hdr.dbch_devicetype == 3;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct DevBroadcastDeviceInterfaceBuffer
        {
            public DevBroadcastDeviceInterfaceBuffer(Int32 deviceType)
            {
                dbch_size = Marshal.SizeOf(typeof(DevBroadcastDeviceInterfaceBuffer));
                dbch_devicetype = deviceType;
                dbch_reserved = 0;
            }
            // ReSharper disable FieldCanBeMadeReadOnly.Local
            [FieldOffset(0)] private readonly Int32 dbch_size;
            [FieldOffset(4)] public Int32 dbch_devicetype;
            [FieldOffset(8)] private Int32 dbch_reserved;
            // ReSharper restore FieldCanBeMadeReadOnly.Local
        }

        static IntPtr _msgHandle = IntPtr.Zero;
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, IntPtr notificationFilter, uint flags);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool UnregisterDeviceNotification(IntPtr handle);
        
        public static void SetUpReceive(IntPtr handle) {
            var mem = IntPtr.Zero;
            try {
                mem = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(DevBroadcastDeviceInterfaceBuffer)));
                Marshal.StructureToPtr(new DevBroadcastDeviceInterfaceBuffer(5), mem, false);
                _msgHandle = RegisterDeviceNotification(handle, mem, 0);
            }
            catch (Exception) { }
            finally {
                Marshal.FreeHGlobal(mem);
            }
        }

        public static void RemoveReceive() {
            if (_msgHandle != IntPtr.Zero)
                UnregisterDeviceNotification(_msgHandle);
        }
    }
}