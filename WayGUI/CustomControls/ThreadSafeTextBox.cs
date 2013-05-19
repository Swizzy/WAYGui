namespace WayGUI.CustomControls {
    using System.Drawing;
    using System.Windows.Forms;

    public class ThreadSafeTextBox : TextBox
    {  
        private delegate string GetStringDelegate();
        private delegate Color GetColorDelegate();
        private delegate bool GetBoolDelegate();

        private string GetText()
        {
            if (!InvokeRequired)
                return base.Text;
            GetStringDelegate del = GetText;
            return Invoke(del) as string;
        }

        private bool GetEnabled()
        {
            if (!InvokeRequired)
                return base.Enabled;
            GetBoolDelegate del = GetEnabled;
            return Invoke(del) is bool && (bool) Invoke(del);
        }

        private Color GetBackColor()
        {
            if (!InvokeRequired)
                return base.BackColor;
            GetColorDelegate del = GetBackColor;
            return Invoke(del) is Color ? (Color) Invoke(del) : new Color();
        }

        private Color GetForeColor()
        {
            if (!InvokeRequired)
                return base.ForeColor;
            GetColorDelegate del = GetForeColor;
            return Invoke(del) is Color ? (Color)Invoke(del) : new Color();
        }

        public override string Text {
            get {
                return GetText();
            }
            set {
                if (InvokeRequired)
                    Invoke(new MethodInvoker(() => base.Text = value));
                base.Text = value;
            }
        }

        public new bool Enabled {
            get {
                return GetEnabled();
            }
            set {
                if (InvokeRequired)
                    Invoke(new MethodInvoker(() => base.Enabled = value));
                base.Enabled = value;
            }
        }

        public override Color BackColor {
            get {
                return GetBackColor();
            }
            set {
                if (InvokeRequired)
                    Invoke(new MethodInvoker(() => { base.BackColor = value; }));
                else base.BackColor = value;
            }
        }

        public override Color ForeColor {
            get {
                return GetForeColor();
            }
            set {
                if (InvokeRequired)
                    Invoke(new MethodInvoker(() => { base.ForeColor = value; }));
                base.ForeColor = value;
                
            }
        }

        public new void AppendText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;
            if (InvokeRequired)
                Invoke(new MethodInvoker(() => base.AppendText(text)));
            else base.AppendText(text);
        }

        public new void Clear()
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(() => base.Clear()));
            else base.Clear();
        }

    }
}