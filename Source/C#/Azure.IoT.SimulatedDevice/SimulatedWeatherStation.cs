using Azure.IoT.Models;
using Azure.IoT.Models.DeviceTwinPatch;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Azure.IoT.SimulatedDevice
{
    public class SimulatedWeatherStation
    {
        private readonly DeviceClient deviceClient;

        private int telemetryReportingRateSeconds;

        public SimulatedWeatherStation(DeviceClient deviceClient)
        {
            this.deviceClient = deviceClient;
            this.deviceClient.SetDesiredPropertyUpdateCallbackAsync(OnDesiredPropertyChangedAsync, null)
                .ConfigureAwait(false).GetAwaiter().GetResult();

            this.ReadTwinToSetInitialReportingRate();
        }

        // Async method to send simulated telemetry
        public async Task SendDeviceToCloudMessagesAsync()
        {
            // Initial telemetry values
            while (true)
            {
                // Create JSON message
                var temperatureTelemetry = new WeatherData()
                {
                    Temperature = this.ReadTemperatureSensor(),
                    Humidity = this.ReadHumiditySensor(),
                };

                var messageString = JsonSerializer.Serialize(temperatureTelemetry);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));

                // Add a custom application property to the message.
                // An IoT hub can filter on these properties without access to the message body.
                message.Properties.Add("temperatureAlert", (temperatureTelemetry.Temperature > 30) ? "true" : "false");

                // Send the telemetry message
                await deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                await Task.Delay(telemetryReportingRateSeconds * 1000);
            }
        }

        private void ReadTwinToSetInitialReportingRate()
        {
            var twin = this.deviceClient.GetTwinAsync()
                            .ConfigureAwait(false).GetAwaiter().GetResult();

            if (twin.Properties.Desired.Contains(nameof(WeatherDataPatch)))
            {
                this.ReadTwinToSetReportingRate(twin.Properties.Desired[nameof(WeatherDataPatch)]);
            }
            else
            {
                this.telemetryReportingRateSeconds = 5;
                Console.WriteLine($"\tWarning: device twin property not found. Setting reporting rate to {this.telemetryReportingRateSeconds} seconds.");
            }
        }

        private void ReadTwinToSetReportingRate(dynamic twin)
        {
            var weatherDataTwin = twin.ToObject<WeatherDataPatch>() as WeatherDataPatch;
            this.telemetryReportingRateSeconds = weatherDataTwin.ReportingRateSeconds;

            Console.WriteLine($"\tNow changing telemetry reporting rate to {weatherDataTwin.ReportingRateSeconds} seconds.");
            this.telemetryReportingRateSeconds = weatherDataTwin.ReportingRateSeconds;
        }

        private async Task OnDesiredPropertyChangedAsync(TwinCollection desiredProperties, object userContext)
        {
            // TODO: it can't check if only a part of the device twin is updated.
            if (desiredProperties.Contains(nameof(WeatherDataPatch)))
            {
                this.ReadTwinToSetReportingRate(desiredProperties[nameof(WeatherDataPatch)]);

                Console.WriteLine("\tSending current UTC time as reported property");
                var reportedProperties = new TwinCollection();

                reportedProperties[nameof(LastUpdatedPatch)] = new LastUpdatedPatch()
                {
                    LastUpdated = DateTime.UtcNow,
                };
                await this.deviceClient.UpdateReportedPropertiesAsync(reportedProperties).ConfigureAwait(false);
            }

            if (desiredProperties.Contains(nameof(NewVersion)))
            {
                Console.WriteLine("\tReceived a firmware update. Data:");
                Console.WriteLine(desiredProperties[nameof(NewVersion)].ToString());
            }
        }

        private double ReadTemperatureSensor()
        {
            var rand = new Random();
            var minTemperature = 20d;
            return minTemperature + rand.NextDouble() * 15;
        }

        private double ReadHumiditySensor()
        {
            var rand = new Random();
            var minHumidity = 60d;
            return minHumidity + rand.NextDouble() * 20;
        }
    }
}
