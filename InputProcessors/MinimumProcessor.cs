namespace TemperatureApp.InputProcessors {
    public class MinimumProcessor : InputProcessor {
        public MinimumProcessor() : base("Find Minimum") { }

        public override void ProcessGlobal(FolderNavigator navigator, FileInput[] inputs, bool isHumidity = false) {
            if (inputs.Length == 0) return;

            double min = inputs.First().Values.Values.First();

            foreach (FileInput input in inputs) {
                foreach (double value in input.Values.Values) {
                    if (value < min) min = value;
                }
            }

            navigator.WriteResult("globalmin", $"global min: {min}", isHumidity);
        }

        public override void ProcessLocal(FolderNavigator navigator, FileInput[] inputs, bool isHumidity = false) {
            if (inputs.Length == 0) return;
            string[] compiled = new string[inputs.Length];
            string measurement = isHumidity ? "humidity" : "temperature";


            for (int i = 0; i < inputs.Length; i++) {
                FileInput input = inputs[i];
                string date = input.Date.ToString("MM/dd/yyyy");

                double min = input.Values.Values.First();

                foreach (double value in input.Values.Values) {
                    if (value < min) min = value;
                }

                compiled[i] = $"id:{input.Id} measurement: {measurement} - location {input.Location} - date {date} (MM/dd/yyyy), min: {min}";
            }

            navigator.WriteResult("min", string.Join('\n', compiled), isHumidity);
        }
    }
}
