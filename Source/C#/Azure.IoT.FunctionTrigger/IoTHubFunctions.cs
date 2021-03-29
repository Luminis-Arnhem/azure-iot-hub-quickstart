using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Text;
using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

namespace Azure.IoT.FunctionTrigger
{
    public class IoTHubFunctions
    {
        private readonly ILogger<IoTHubFunctions> logger;

        public IoTHubFunctions(ILogger<IoTHubFunctions> logger)
        {
            this.logger = logger;
        }

        // Obtain the IoTHubConnectionString as follows:
        // In Azure Portal, navigate to your IoT Hub. Then, navigate to "Built-in endpoints". 
        // Under "Event Hub compatible endpoint", select the "service" shared access policy and copy its endpoint string.
        [FunctionName(nameof(ProcessWeatherTelemetry))]
        public void ProcessWeatherTelemetry([IoTHubTrigger("messages/events", Connection = "IoTHubServiceEndpoint")]EventData message)
        {
            var weatherDataJson = Encoding.UTF8.GetString(message.Body.Array);
            // var weatherData = JsonSerializer.Deserialize<WeatherData>(weatherDataJson);

            this.logger.LogInformation($"C# IoT Hub trigger function processed a message: {weatherDataJson}");
            if(message.Properties.TryGetValue("temperatureAlert", out object value) && (string)value == "true")
            {
                this.logger.LogWarning($"Warning: Temperature exceeded 30 degrees celcius!");
            }
        }
    }
}