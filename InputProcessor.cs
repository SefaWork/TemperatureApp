namespace TemperatureApp {
    public abstract class InputProcessor {
        private string text;
        public string Text { get => text; }

        public InputProcessor(string text) {
            this.text = text;
        }

        public abstract void ProcessLocal(FolderNavigator navigator, FileInput[] inputs, bool isHumidity = false);

        public abstract void ProcessGlobal(FolderNavigator navigator, FileInput[] inputs, bool isHumidity = false);
    }
}
