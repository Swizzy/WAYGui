namespace WayGUI {
    internal class ComboBoxItem<T> {
        public readonly T Value;
        private readonly string _displayName;

        public ComboBoxItem(T value, string displayName) {
            Value = value;
            _displayName = displayName;
        }

        public override string ToString() {
            return _displayName;
        }
    }
}