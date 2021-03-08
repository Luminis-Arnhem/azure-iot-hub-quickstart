# Azure IoT Hub Quickstart

# Developer info

## Connection strings
The following information is for retrieving connection strings.

### Function Trigger
IoTHubServiceEndpoint: In Azure Portal, navigate to your IoT Hub. Then, navigate to "Built-in endpoints". Under "Event Hub compatible endpoint", select the "service" shared access policy and copy its endpoint string.

### (Simulated) Device
DeviceConnectionString: In Azure Portal, navigate to your IoT Hub. Then, navigate to "IoT devices". Select the device you want the connection string for, then copy either the primary or the secondary connection string.

### (Simulated) Device Manager
RegistryConnectionString: In Azure Portal, navigate to your IoT Hub. Then, navigate to "Shared access policies". Select the "service" policy, then copy either the primary or the secondary connection string.