namespace TemperatureApp.InputProcessors {
    /// <summary>
    /// Processor for finding frequency.
    /// </summary>
    public class FrequencyProcessor : InputProcessor {
        public FrequencyProcessor() : base("Find Frequency") {}

        public override void ProcessGlobal(FolderNavigator navigator, FileInput[] inputs, bool isHumidity = false) {
            if (inputs.Length == 0) return;
            string measurement = isHumidity ? "humidity" : "temperature";

            Dictionary<double, double> values = [];

            for (int i = 0; i < inputs.Length; i++) {
                FileInput input = inputs[i];
                foreach (double value in input.Values.Values) {
                    if (values.ContainsKey(value)) {
                        values[value]++;
                    } else {
                        values.Add(value, 1);
                    }
                }
            }

            string compiled = $"global frequencies:{Environment.NewLine}";

            foreach (double key in values.Keys) {
                double value = values[key];
                compiled += $"{key} measured {value} times" + Environment.NewLine;
            }

            navigator.WriteResult("globalfrequency", compiled, isHumidity);
        }

        public override void ProcessLocal(FolderNavigator navigator, FileInput[] inputs, bool isHumidity = false) {
            if (inputs.Length == 0) return;
            string[] compiled = new string[inputs.Length];
            string measurement = isHumidity ? "humidity" : "temperature";

            for(int i=0; i < inputs.Length; i++) {
                FileInput input = inputs[i];
                Dictionary<double, double> values = [];
                foreach(double value in input.Values.Values) {
                    if(values.ContainsKey(value)) {
                        values[value]++;
                    } else {
                        values.Add(value, 1);
                    }
                }

                string date = input.Date.ToString("MM/dd/yyyy");
                string innerCompilation = $"id:{input.Id} measurement: {measurement} - location {input.Location} - date {date} (MM/dd/yyyy){Environment.NewLine}";

                foreach (double key in values.Keys) {
                    double value = values[key];
                    innerCompilation += $"{key} measured {value} times" + Environment.NewLine;
                }

                compiled[i] = innerCompilation;
            }

            string fullText = string.Join("------------------\n", compiled);
            navigator.WriteResult("frequency", fullText, isHumidity);
        }
    }
}
