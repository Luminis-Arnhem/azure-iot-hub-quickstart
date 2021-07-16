# Simulated Device Manager

## Configuration
When creating the IoT Hub using the setups.ps1 script, the Shared Access Policy Connection String is written to the output.
Use this Connection String in the `launchSettings.json` file as the value for the `registryConnectionString` property.

In case you have missed it, you can find the Connection String by following the next steps:
- Go to the Azure Portal and visit the created IoT Hub
- Click the link 'Shared access policies' on the left sidebar
- Select the name 'iothubowner'
- Copy the value of the 'Primary Connection String'

## Run
Open the project in Visual Studio and press Run. The project requires .NET Core 3.1. It runs on Windows machines only.
