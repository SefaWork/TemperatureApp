using TemperatureApp.InputProcessors;

namespace TemperatureApp {
    /// <summary>
    /// Keeps a record of all processors.
    /// </summary>
    static class ProcessorRegister {
        private static List<InputProcessor> REGISTER = [];

        /// <summary>
        /// Finds a processor.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets index of found processor.
        /// </summary>
        /// <param name="processor"></param>
        /// <returns></returns>
        public static int GetProcessorIndex(InputProcessor processor) {
            return REGISTER.IndexOf(processor);
        }

        /// <summary>
        /// Returns names of all processors.
        /// </summary>
        /// <returns></returns>
        public static string[] GetProcessorTextList() {
            return [.. REGISTER.ConvertAll((processor) => processor.Text)];
        }

        /// <summary>
        /// Static initializer for registering processors.
        /// </summary>
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
