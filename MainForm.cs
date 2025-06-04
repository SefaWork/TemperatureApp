using System.Diagnostics;

namespace TemperatureApp
{
    public partial class MainForm : Form
    {
        private FolderNavigator navigator;

        public MainForm()
        {
            navigator = new();
            InitializeComponent();

            string[] list = ProcessorRegister.GetProcessorTextList();
            ControlListBox.Items.AddRange(list);
            GlobalListBox.Items.AddRange([..Enumerable.Repeat("Global", list.Length)]);
        }

        private void UpdateFolder(string path) {
            if(string.IsNullOrWhiteSpace(path)) {
                this.navigator.SetRootFolder(null);
                FolderLocation.ForeColor = Color.Gray;
                FolderLocation.Text = this.navigator.RootFolder;
            } else {
                this.navigator.SetRootFolder(path);
                FolderLocation.ForeColor = Color.Black;
                FolderLocation.Text = this.navigator.RootFolder;

                List<string> missing = this.navigator.GetMissingFolders();

                if (missing.Count > 0) {
                    DialogResult result = MessageBox.Show(
                        $"The following subfolders are missing:\n\n{string.Join('\n', missing)}\n\nCreate these subfolders now?",
                        "Missing Subfolders",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes) {
                        try {
                            this.navigator.CreateMissingFolders();
                        } catch {
                            MessageBox.Show("Failed to create folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                missing.Clear();
            }
        }

        private void BrowseFolder() {
            using (FolderBrowserDialog browser = new()) {
                DialogResult result = browser.ShowDialog();
                if (result == DialogResult.OK) {
                    UpdateFolder(browser.SelectedPath);
                }
            }
        }

        private void DisplayException(string exceptionMessage) {
            this.ResultsMessage.Clear();
            this.ResultsMessage.SelectionColor = Color.Red;
            this.ResultsMessage.AppendText($"Error | {exceptionMessage}");
        }

        private void DisplaySuccess(string successMessage) {
            this.ResultsMessage.Clear();
            this.ResultsMessage.SelectionColor = Color.Green;
            this.ResultsMessage.AppendText($"Success | {successMessage}");
        }

        private void CalculateButton_Click(object sender, EventArgs e) {
            if(this.navigator.RootFolder == "") {
                this.BrowseFolder();

                if(this.navigator.RootFolder == "") {
                    DisplayException("Root folder is not selected.");
                    return;
                }
            }

            FileInput[] temperatureFiles;
            FileInput[] humidityFiles;

            try {
                temperatureFiles = this.navigator.GetTemperatureInputs();
                humidityFiles = this.navigator.GetHumidityInputs();
            } catch (Exception err) {
                DisplayException(err.Message);
                return;
            }

            if(temperatureFiles.Length + humidityFiles.Length == 0) {
                DisplayException("There are no files to process.");
                return;
            }

            var checkedItems = this.ControlListBox.CheckedItems;
            if(checkedItems.Count == 0) {
                DisplayException("You did not select anything.");
                return;
            }

            foreach(string checkedItem in checkedItems) {
                InputProcessor processor = ProcessorRegister.GetProcessor(checkedItem) ?? throw new Exception("Processor does not exist.");
                int index = ProcessorRegister.GetProcessorIndex(processor);
                
                if(this.GlobalListBox.CheckedIndices.IndexOf(index) == -1) {
                    processor.ProcessLocal(this.navigator, temperatureFiles, false);
                    processor.ProcessLocal(this.navigator, humidityFiles, true);
                } else {
                    processor.ProcessGlobal(this.navigator, temperatureFiles, false);
                    processor.ProcessGlobal(this.navigator, humidityFiles, true);
                }
            }

            DisplaySuccess($"Results can be found in file://{navigator.ResultFolder}\\");
        }
        
        private void ResultsMessage_LinkClicked(object sender, System.Windows.Forms.LinkClickedEventArgs e) {
            if (e.LinkText == null) return;
            string[] split = e.LinkText.Split("file://");
            if (split.Length < 1) return;
            string path = string.Join('\\', split[1].Split('\\')[..^1]);

            Process.Start("explorer.exe", @path);
        }

        private void SelectFolderButton_Click(object sender, EventArgs e) {
            BrowseFolder();
        }
    }
}
