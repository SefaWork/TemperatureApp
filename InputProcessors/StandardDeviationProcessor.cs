namespace TemperatureApp.InputProcessors {
    public class StandardDeviationProcessor : InputProcessor {
        public StandardDeviationProcessor() : base("Find Standard Deviation") { }

        public override void ProcessGlobal(FolderNavigator navigator, FileInput[] inputs, bool isHumidity = false) {
            if (inputs.Length == 0) return;

            List<double> allValues = [];
            double sum = 0d;

            foreach (FileInput input in inputs) {
                foreach (double value in input.Values.Values) {
                    allValues.Add(value);
                    sum += value;
                }
            }

            double mean = sum / allValues.Count;
            sum = 0d;

            foreach(double value in allValues) {
                sum += Math.Pow(value - mean, 2);
            }

            double standardDeviation = Math.Sqrt(sum / allValues.Count);

            navigator.WriteResult("globalStandardDeviation", $"global standard deviation: {standardDeviation}", isHumidity);
        }

        public override void ProcessLocal(FolderNavigator navigator, FileInput[] inputs, bool isHumidity = false) {
            if (inputs.Length == 0) return;
            string[] compiled = new string[inputs.Length];
            string measurement = isHumidity ? "humidity" : "temperature";


            for (int i = 0; i < inputs.Length; i++) {
                FileInput input = inputs[i];
                string date = input.Date.ToString("MM/dd/yyyy");

                int count = input.Values.Values.Count;
                double sum = 0d;

                // First, find mean.
                foreach (double value in input.Values.Values) {
                    sum += value;
                }

                double mean = sum / count;

                // Second, find variation from mean.

                foreach (double value in input.Values.Values) {
                    sum += Math.Pow(value - mean, 2);
                }

                // Lastly, divide by n and take square root.
                double standardDeviation = Math.Sqrt(sum / count);

                compiled[i] = $"id:{input.Id} measurement: {measurement} - location {input.Location} - date {date} (MM/dd/yyyy), standard deviation: {standardDeviation}";
            }

            navigator.WriteResult("standardDeviation", string.Join('\n', compiled), isHumidity);
        }
    }
}
