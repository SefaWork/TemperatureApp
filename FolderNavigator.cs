using System.Text;

namespace TemperatureApp {
    public class FolderNavigator {

        private static readonly HashSet<string> TEMPERATURE_FOLDER_NAMES = [
            "temperature",
            "temperatures",
            "sıcaklık"
        ];

        private static readonly HashSet<string> HUMIDITY_FOLDER_NAMES = [
            "humidity",
            "humidities",
            "nem"
        ];

        private static readonly HashSet<string> RESULT_FOLDER_NAMES = [
            "result",
            "results",
            "olcumler",
            "ölçümler"
        ];

        private string rootFolder;
        private string temperatureFolder;
        private string humidityFolder;
        private string resultFolder;

        public string RootFolder { get => rootFolder; }
        public string TemperatureFolder { get => temperatureFolder; }
        public string HumidityFolder { get => humidityFolder; }
        public string ResultFolder { get => resultFolder; }

        public FolderNavigator() {
            rootFolder = "";
            temperatureFolder = "";
            humidityFolder = "";
            resultFolder = "";
        }

        private void FindSubdirectories() {
            this.temperatureFolder = "";
            this.humidityFolder = "";
            this.resultFolder = "";

            if (this.rootFolder == "") return;

            string[] subdirectories = Directory.GetDirectories(this.rootFolder);

            byte found = 0;
            bool temperatureFound = false, humidityFound = false, resultFound = false;

            foreach(string subdirectory in subdirectories) {
                string folderName = subdirectory.Split('\\').Last().ToLower();
                if(!temperatureFound && TEMPERATURE_FOLDER_NAMES.Contains(folderName)) {
                    this.temperatureFolder = subdirectory;
                    temperatureFound = true;

                    if (++found == 3) break;
                } else if(!humidityFound && HUMIDITY_FOLDER_NAMES.Contains(folderName)) {
                    this.humidityFolder = subdirectory;
                    humidityFound = true;
                    found++;

                    if (++found == 3) break;
                } else if(!resultFound && RESULT_FOLDER_NAMES.Contains(folderName)) {
                    this.resultFolder = subdirectory;
                    resultFound = true;
                    found++;

                    if (++found == 3) break;
                }
            }
        }

        public List<string> GetMissingFolders() {
            if (this.rootFolder == "") return [];
            List<string> missing = [];

            if (this.temperatureFolder == String.Empty) missing.Add("temperature");
            if (this.humidityFolder == String.Empty) missing.Add("humidity");
            if (this.resultFolder == String.Empty) missing.Add("result");

            return missing;
        }

        public FileInput[] GetTemperatureInputs() {
            if (this.temperatureFolder == "") return [];
            string[] filePaths = Directory.GetFiles(this.temperatureFolder);

            FileInput[] allInputs = new FileInput[filePaths.Length];
            for(int i = 0; i < filePaths.Length; i++) {
                string filePath = filePaths[i];
                string fileName = Path.GetFileName(filePath);

                if (!fileName.EndsWith(".txt")) throw new Exception($"Temperature subfolder should only contain text documents. Illegal file: file://{filePath}");

                FileInput input;
                try {
                    input = FileInput.FromFilePath(filePath);
                    if (input.IsHumidity) throw new Exception($"Humidity information should not be inside of temperatures directory.");
                } catch(Exception e) {
                    // Decorate error and rethrow.
                    throw new Exception($"{e.Message} Illegal file: file://{filePath}", e);
                }

                allInputs[i] = input;
            }

            return allInputs;
        }

        public FileInput[] GetHumidityInputs() {
            if (this.humidityFolder == "") return [];
            string[] filePaths = Directory.GetFiles(this.humidityFolder);

            FileInput[] allInputs = new FileInput[filePaths.Length];
            for (int i = 0; i < filePaths.Length; i++) {
                string filePath = filePaths[i];
                string fileName = Path.GetFileName(filePath);

                if (!fileName.EndsWith(".txt")) {
                    throw new Exception($"Humidity subfolder should only contain text documents. Illegal file: file://{filePath}");
                }

                FileInput input;
                try {
                    input = FileInput.FromFilePath(filePath);
                    if (!input.IsHumidity) throw new Exception($"Temperature information should not be inside of humidity directory.");
                } catch (Exception e) {
                    // Decorate error and rethrow.
                    throw new Exception($"{e.Message} Illegal file: file://{filePath}", e);
                }

                allInputs[i] = input;
            }

            return allInputs;
        }

        public bool SetRootFolder(string? folderPath) {
            if(!string.IsNullOrWhiteSpace(folderPath)) {
                if(Directory.Exists(folderPath)) {
                    this.rootFolder = folderPath;
                    this.FindSubdirectories();
                    return true;
                } else {
                    this.rootFolder = "";
                    this.FindSubdirectories();
                }
            }

            return false;
        }

        public void CreateMissingFolders() {
            if (this.rootFolder == "") return;

            // Make sure subdirectories are up-to-date.
            this.FindSubdirectories();

            // Temperature subfolder doesn't exist, create one.
            if (this.temperatureFolder == "") {
                DirectoryInfo info = Directory.CreateDirectory(Path.Combine(this.rootFolder, "temperature"));
                this.temperatureFolder = info.FullName;
            }

            // Humidity subfolder doesn't exist, create one.
            if (this.humidityFolder == "") {
                DirectoryInfo info = Directory.CreateDirectory(Path.Combine(this.rootFolder, "humidity"));
                this.humidityFolder = info.FullName;
            }

            // Result subfolder doesn't exist, create one.
            if (this.resultFolder == "") {
                DirectoryInfo info = Directory.CreateDirectory(Path.Combine(this.rootFolder, "result"));
                this.humidityFolder = info.FullName;
            }
        }

        public void WriteResult(string fileName, string content, bool isHumidity) {
            string subfolderName;

            // Output folder name should match input folder name.
            if(isHumidity) {
                subfolderName = this.humidityFolder.Split('\\').Last();
            } else {
                subfolderName = this.temperatureFolder.Split('\\').Last();
            }

            if (string.IsNullOrWhiteSpace(subfolderName)) throw new Exception("Path does not exist.");

            string folderPath = $"{this.resultFolder}\\{subfolderName}";
            Directory.CreateDirectory(folderPath);

            FileStream stream = File.Open($"{folderPath}\\{fileName}.txt", FileMode.Create);
            byte[] buffer = Encoding.UTF8.GetBytes(content);

            stream.Write(buffer, 0, buffer.Length);
            stream.Close();
        }
    }
}
