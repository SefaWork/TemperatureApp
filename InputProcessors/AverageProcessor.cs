namespace TemperatureApp.InputProcessors {
    public class AverageProcessor : InputProcessor {
        public AverageProcessor() : base("Find Average") {}

        public override void ProcessGlobal(FolderNavigator navigator, FileInput[] inputs, bool isHumidity = false) {
            if (inputs.Length == 0) return;

            int count = 0;
            double sum = 0d;

            foreach (FileInput input in inputs) {
                foreach (double value in input.Values.Values) {
                    sum += value;
                    count++;
                }
            }

            navigator.WriteResult("globalaverage", $"global average: {sum / count}", isHumidity);
        }

        public override void ProcessLocal(FolderNavigator navigator, FileInput[] inputs, bool isHumidity = false) {
            if (inputs.Length == 0) return;
            string[] compiled = new string[inputs.Length];
            string measurement = isHumidity ? "humidity" : "temperature";


            for (int i = 0; i < inputs.Length; i++) {
                FileInput input = inputs[i];
                string date = input.Date.ToString("MM/dd/yyyy");

                double sum = 0;

                foreach (double value in input.Values.Values) {
                    sum += value;
                }

                compiled[i] = $"id:{input.Id} measurement: {measurement} - location {input.Location} - date {date} (MM/dd/yyyy), average: {sum / input.Values.Count}";
            }

            navigator.WriteResult("average", string.Join('\n', compiled), isHumidity);
        }
    }
}
