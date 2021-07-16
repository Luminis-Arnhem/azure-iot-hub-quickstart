# Simulated Device

## Configuration 
When creating a new device using the setups.ps1 script, the Primary Connection String is written to the output.
Use this Primary Connection String in the `launchSettings.json` file as the value for the `DeviceConnectionString` property. 

In case you have missed it, you can find the Primary Connection String by following the next steps:
- Go to the Azure Portal and visit the created IoT Hub
- Click the link 'IoT Devices' in the left sidebar
- Click on the device for which you want the connection string
- Copy the Primary (Secondary) Connection String

## Run
Open the solution in Visual Studio and select the Simulated Device project as the startup project. Then, press run. The project requires .NET Core 3.1
