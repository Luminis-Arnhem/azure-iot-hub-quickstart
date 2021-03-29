// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// This application uses the Azure IoT Hub device SDK for .NET
// For samples see: https://github.com/Azure/azure-iot-sdk-csharp/tree/master/iothub/device/samples

using Microsoft.Azure.Devices.Client;
using System;
using System.Threading.Tasks;

namespace Azure.IoT.SimulatedDevice
{
    class Program
    {
        
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Simulated device. Ctrl-C to exit.\n");

            // The device connection string to authenticate the device with your IoT hub.
            var deviceConnectionString = Environment.GetEnvironmentVariable("DeviceConnectionString");

            // Connect to the IoT hub using the MQTT protocol
            var deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString, TransportType.Mqtt);
            var simulatedDevice = new SimulatedWeatherStation(deviceClient);

            await simulatedDevice.SendDeviceToCloudMessagesAsync();
            Console.ReadLine();
        }
    }
}
