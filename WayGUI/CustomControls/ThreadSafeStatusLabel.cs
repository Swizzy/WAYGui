namespace WayGUI.CustomControls {
    using System.Drawing;
    using System.Windows.Forms;

    public class ThreadSafeToolStripStatusLabel : ToolStripStatusLabel
    {
        private delegate string GetStringDelegate();
        private delegate Color GetColorDelegate();
        private delegate bool GetBoolDelegate();

        private string GetText()
        {
            if (Parent == null || !Parent.InvokeRequired)
                return base.Text;
            GetStringDelegate del = GetText;
            return Parent.Invoke(del) as string;
        }

        private bool GetEnabled()
        {
            if (Parent == null || !Parent.InvokeRequired)
                return base.Enabled;
            GetBoolDelegate del = GetEnabled;
            return Parent.Invoke(del) is bool && (bool)Parent.Invoke(del);
        }

        private Color GetBackColor()
        {
            if (Parent == null || !Parent.InvokeRequired)
                return base.BackColor;
            GetColorDelegate del = GetBackColor;
            return Parent.Invoke(del) is Color ? (Color)Parent.Invoke(del) : new Color();
        }

        private Color GetForeColor()
        {
            if (Parent == null || !Parent.InvokeRequired)
                return base.ForeColor;
            GetColorDelegate del = GetForeColor;
            return Parent.Invoke(del) is Color ? (Color)Parent.Invoke(del) : new Color();
        }

        public override string Text
        {
            get
            {
                return GetText();
            }
            set
            {
                if (Parent != null && Parent.InvokeRequired)
                    Parent.Invoke(new MethodInvoker(() => base.Text = value));
                base.Text = value;
            }
        }

        public new bool Enabled
        {
            get
            {
                return GetEnabled();
            }
            set
            {
                if (Parent != null && Parent.InvokeRequired)
                    Parent.Invoke(new MethodInvoker(() => base.Enabled = value));
                base.Enabled = value;
            }
        }

        public override Color BackColor
        {
            get
            {
                return GetBackColor();
            }
            set
            {
                if (Parent != null && Parent.InvokeRequired)
                    Parent.Invoke(new MethodInvoker(() => { base.BackColor = value; }));
                else base.BackColor = value;
            }
        }

        public override Color ForeColor
        {
            get
            {
                return GetForeColor();
            }
            set
            {
                if (Parent != null && Parent.InvokeRequired)
                    Parent.Invoke(new MethodInvoker(() => { base.ForeColor = value; }));
                base.ForeColor = value;

            }
        }
    }
}