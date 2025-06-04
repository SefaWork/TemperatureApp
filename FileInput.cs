using System.Globalization;
using System.Text;

namespace TemperatureApp {
    /// <summary>
    /// Reads a file and stores relevant information for use later.
    /// </summary>
    public class FileInput {

        //------------ These fields are used for localization. ----------
        private static readonly HashSet<string> MEASUREMENT_NAMES = [
            "measurement",
            "ölçüm"
        ];

        private static readonly HashSet<string> LOCATION_NAMES = [
            "location",
            "yer"
        ];

        private static readonly Dictionary<string, string> DATE_NAMES = new() {
            {"date", "en-US"},
            {"tarih", "fr-FR"}
        };

        private static readonly HashSet<string> TEMPERATURE_NAMES = [
            "temperature",
            "sıcaklık"
        ];

        private static readonly HashSet<string> HUMIDITY_NAMES = [
            "humidity",
            "nem"
        ];
        //------------------------------------------------------------

        private int id;
        private string location;
        private DateTime date;
        private bool isHumidity;
        private Dictionary<TimeOnly, double> values;

        public int Id { get => id; }
        public string Location { get => location; }
        public DateTime Date { get => date; }
        public bool IsHumidity { get => isHumidity; }
        public bool IsTemperature { get => !isHumidity; }
        public Dictionary<TimeOnly, double> Values { get => values; }

        private FileInput(int id) {
            this.id = id;
            this.location = "";
            this.date = DateTime.Now;
            this.isHumidity = false;
            this.values = [];
        }

        /// <summary>
        /// Loads value lines.
        /// </summary>
        /// <param name="lines"></param>
        /// <exception cref="Exception"></exception>
        private void LoadValues(string[] lines) {
            foreach (string line in lines) {
                string[] split = line.Split(',', StringSplitOptions.TrimEntries);
                if (split.Length < 2) continue;

                string timestamp = split[0], value = split[1];
                double parsed = double.Parse(value);
                TimeOnly parsedTime = TimeOnly.ParseExact(timestamp, "HH:mm:ss", CultureInfo.InvariantCulture);

                if (this.values.ContainsKey(parsedTime)) throw new Exception($"Duplicate timestamp {parsedTime:HH:mm:ss}");

                this.values.Add(parsedTime, parsed);
            }

            if (this.values.Count == 0) throw new Exception("No values declared.");
        }

        /// <summary>
        /// Creates a FileInput instance from the first line in a file.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static FileInput FromDeclarationLine(string line) {
            // Tokenize line.
            string[] tokens = line.Split('-', StringSplitOptions.TrimEntries);

            // Get ID token.
            string idToken = tokens[0].Split(' ')[0];

            // Subtract length of ID from first token.
            tokens[0] = tokens[0][idToken.Length..];

            string[] firstTokenSplit = idToken.Split(':');
            if (firstTokenSplit[0].ToLower() != "id") throw new Exception("File is expected to start with id. Make sure to not put spaces when defining id.");

            int parsedId = int.Parse(firstTokenSplit[1]);
            if (parsedId < 0) throw new Exception("File id cannot be negative.");

            FileInput input = new(parsedId);
            bool measurementFound = false, dateFound = false, locationFound = false;
            
            // Iterate over each token.
            foreach(string token in tokens) {
                string[] split = token.Split(':', StringSplitOptions.TrimEntries);
                if (split.Length < 2) continue;
                
                string declaration = split[0].ToLower();
                string value = split[1];

                if(MEASUREMENT_NAMES.Contains(declaration)) {
                    if (measurementFound) throw new Exception("Duplicate measurement declaration.");
                    measurementFound = true;

                    string lowerValue = value.ToLower();

                    if (TEMPERATURE_NAMES.Contains(lowerValue)) {
                        input.isHumidity = false;
                    } else if (HUMIDITY_NAMES.Contains(lowerValue)) {
                        input.isHumidity = true;
                    } else throw new Exception("Unrecognized measurement type.");
                }

                if(DATE_NAMES.TryGetValue(declaration, out string? culture)) {
                    if (dateFound) throw new Exception("Duplicate date declaration.");
                    dateFound = true;
                    DateTime parsedDate = DateTime.Parse(value, new CultureInfo(culture));
                    input.date = parsedDate;
                }

                if(LOCATION_NAMES.Contains(declaration)) {
                    if (locationFound) throw new Exception("Duplicate location declaration.");
                    locationFound = true;
                    input.location = value;
                }
            }

            if (!dateFound) throw new Exception("Missing declaration for date.");
            if (!locationFound) throw new Exception("Missing declaration for location.");
            if (!measurementFound) throw new Exception("Missing declaration for measurement.");

            return input;
        }

        /// <summary>
        /// Creates FileInput from the first line in a file and loads values into it from the rest of the lines.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static FileInput FromFilePath(string filePath) {
            FileStream file = File.Open(filePath, FileMode.Open);

            byte[] bytes = new byte[file.Length];
            file.Read(bytes);

            file.Close();

            string fileContents = Encoding.UTF8.GetString(bytes);
            string[] lines = fileContents.Split('\n');
            string firstLine = lines[0];

            FileInput input = FromDeclarationLine(firstLine);
            input.LoadValues(lines[1..]);

            return input;
        }
    }
}
