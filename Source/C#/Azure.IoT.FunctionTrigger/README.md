# Azure Function trigger

## Running the project locally
Open the solution in Visual Studio and select the Function Trigger project as the startup project. The project requires a bit of manual configuration.

- Go to the Azure Portal and visit the created IoT Hub
- Click the link 'Built-in Endpoints' in the left sidebar
- In the "Event Hub compatible endpoint" section, select `service` as the Shared access policy
- Copy the Event Hub-compatible endpoint string and paste it into `IotHubServiceEndpoint` in `local.settings.json`.

Then, press run. The project requires .NET Core 3.1
