namespace TemperatureApp.InputProcessors {
    public class MaximumProcessor : InputProcessor {
        public MaximumProcessor() : base("Find Maximum") {}

        public override void ProcessGlobal(FolderNavigator navigator, FileInput[] inputs, bool isHumidity = false) {
            if (inputs.Length == 0) return;

            double max = inputs.First().Values.Values.First();

            foreach (FileInput input in inputs) {
                foreach (double value in input.Values.Values) {
                    if (value > max) max = value;
                }
            }

            navigator.WriteResult("globalmax", $"global max: {max}", isHumidity);
        }

        public override void ProcessLocal(FolderNavigator navigator, FileInput[] inputs, bool isHumidity = false) {
            if (inputs.Length == 0) return;
            string[] compiled = new string[inputs.Length];
            string measurement = isHumidity ? "humidity" : "temperature";


            for (int i = 0; i < inputs.Length; i++) {
                FileInput input = inputs[i];
                string date = input.Date.ToString("MM/dd/yyyy");

                double max = input.Values.Values.First();

                foreach (double value in input.Values.Values) {
                    if (value > max) max = value;
                }

                compiled[i] = $"id:{input.Id} measurement: {measurement} - location {input.Location} - date {date} (MM/dd/yyyy), max: {max}";
            }

            navigator.WriteResult("max", string.Join('\n', compiled), isHumidity);
        }
    }
}
