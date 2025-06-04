using TemperatureApp.InputProcessors;

namespace TemperatureApp {
    static class ProcessorRegister {
        private static List<InputProcessor> REGISTER = [];

        public static InputProcessor? GetProcessor(string text) {
            int found = -1;

            for(int i=0; i < REGISTER.Count; i++) {
                InputProcessor processor = REGISTER[i];
                if(processor.Text == text) {
                    found = i;
                    break;
                }
            }

            return found > -1? REGISTER[found] : null;
        }

        public static int GetProcessorIndex(InputProcessor processor) {
            return REGISTER.IndexOf(processor);
        }

        public static string[] GetProcessorTextList() {
            return [.. REGISTER.ConvertAll((processor) => processor.Text)];
        }

        static ProcessorRegister() {
            // List of registers.

            REGISTER.Add(new AverageProcessor());
            REGISTER.Add(new MaximumProcessor());
            REGISTER.Add(new MinimumProcessor());
            REGISTER.Add(new StandardDeviationProcessor());
            REGISTER.Add(new FrequencyProcessor());
            REGISTER.Add(new MedianProcessor());
        }
    }
}
