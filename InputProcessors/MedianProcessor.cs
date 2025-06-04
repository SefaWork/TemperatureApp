namespace TemperatureApp.InputProcessors {
    /// <summary>
    /// Processor for finding median.
    /// </summary>
    public class MedianProcessor : InputProcessor {
        public MedianProcessor() : base("Find Median") { }

        public override void ProcessGlobal(FolderNavigator navigator, FileInput[] inputs, bool isHumidity = false) {
            if (inputs.Length == 0) return;

            List<double> values = [];

            foreach(FileInput input in inputs) {
                foreach(double value in input.Values.Values) {
                    values.Add(value);
                }
            }

            values.Sort((a,b) => a > b? 1 : -1);
            int count = values.Count;
            int halvedCount = (int)(count / 2);

            double median;

            if(values.Count % 2 == 0) {
                // 6 / 2 and 6 / 2 + 1
                median = (values[halvedCount] + values[halvedCount + 1]) / 2;
            } else {
                median = values[halvedCount + 1];
            }

            navigator.WriteResult("globalmedian", $"median: {median}", isHumidity);
        }

        public override void ProcessLocal(FolderNavigator navigator, FileInput[] inputs, bool isHumidity = false) {
            if (inputs.Length == 0) return;
            string[] compiled = new string[inputs.Length];
            string measurement = isHumidity ? "humidity" : "temperature";

            for (int i = 0; i < inputs.Length; i++) {
                FileInput input = inputs[i];
                string date = input.Date.ToString("MM/dd/yyyy");

                List<double> values = [];
                foreach(double value in input.Values.Values) {
                    values.Add(value);
                }

                values.Sort((a, b) => a > b ? 1 : -1);
                int count = values.Count;
                int halvedCount = (int)(count / 2);

                double median;

                if (values.Count % 2 == 0) {
                    // 6 / 2 and 6 / 2 + 1
                    median = (values[halvedCount - 1] + values[halvedCount]) / 2;
                } else {
                    median = values[halvedCount];
                }

                compiled[i] = $"id:{input.Id} measurement: {measurement} - location {input.Location} - date {date} (MM/dd/yyyy), median: {median}";
            }

            navigator.WriteResult("median", string.Join('\n', compiled), isHumidity);
        }
    }
}
