namespace WayGUI.CustomControls {
    using System.Drawing;
    using System.Windows.Forms;

    public class ThreadSafeRichTextBox : RichTextBox {
        public override string Text {
            get { return GetText(); }
            set {
                if(InvokeRequired)
                    Invoke(new MethodInvoker(() => base.Text = value));
                base.Text = value;
            }
        }

        public new bool Enabled {
            get { return GetEnabled(); }
            set {
                if(InvokeRequired)
                    Invoke(new MethodInvoker(() => base.Enabled = value));
                base.Enabled = value;
            }
        }

        public override Color BackColor {
            get { return GetBackColor(); }
            set {
                if(InvokeRequired) {
                    Invoke(new MethodInvoker(() => {
                                                 base.BackColor = value;
                                             }));
                }
                else
                    base.BackColor = value;
            }
        }

        public override Color ForeColor {
            get { return GetForeColor(); }
            set {
                if(InvokeRequired) {
                    Invoke(new MethodInvoker(() => {
                                                 base.ForeColor = value;
                                             }));
                }
                base.ForeColor = value;
            }
        }

        public new void Select() {
            if(InvokeRequired) {
                Invoke(new MethodInvoker(() => base.Select()));
                return;
            }
            base.Select();
        }

        public new void Select(int start, int length) {
            if(InvokeRequired) {
                Invoke(new MethodInvoker(() => base.Select(start, length)));
                return;
            }
            base.Select(start, length);
        }

        public new void Select(bool directed, bool forward) {
            if(InvokeRequired) {
                Invoke(new MethodInvoker(() => base.Select(directed, forward)));
                return;
            }
            base.Select(directed, forward);
        }

        public new void SelectAll() {
            if(InvokeRequired) {
                Invoke(new MethodInvoker(() => base.SelectAll()));
                return;
            }
            base.SelectAll();
        }

        public new void ScrollToCaret() {
            if(InvokeRequired) {
                Invoke(new MethodInvoker(() => base.ScrollToCaret()));
                return;
            }
            base.ScrollToCaret();
        }

        public new void AppendText(string text) {
            if(string.IsNullOrEmpty(text))
                return;
            if(InvokeRequired)
                Invoke(new MethodInvoker(() => base.AppendText(text)));
            else
                base.AppendText(text);
        }

        public new void Clear() {
            if(InvokeRequired)
                Invoke(new MethodInvoker(() => base.Clear()));
            else
                base.Clear();
        }

        private string GetText() {
            if(!InvokeRequired)
                return base.Text;
            GetStringDelegate del = GetText;
            return Invoke(del) as string;
        }

        private bool GetEnabled() {
            if(!InvokeRequired)
                return base.Enabled;
            GetBoolDelegate del = GetEnabled;
            return Invoke(del) is bool && (bool) Invoke(del);
        }

        private Color GetBackColor() {
            if(!InvokeRequired)
                return base.BackColor;
            GetColorDelegate del = GetBackColor;
            return Invoke(del) is Color ? (Color) Invoke(del) : new Color();
        }

        private Color GetForeColor() {
            if(!InvokeRequired)
                return base.ForeColor;
            GetColorDelegate del = GetForeColor;
            return Invoke(del) is Color ? (Color) Invoke(del) : new Color();
        }

        #region Nested type: GetBoolDelegate

        private delegate bool GetBoolDelegate();

        #endregion

        #region Nested type: GetColorDelegate

        private delegate Color GetColorDelegate();

        #endregion

        #region Nested type: GetStringDelegate

        private delegate string GetStringDelegate();

        #endregion
    }
}