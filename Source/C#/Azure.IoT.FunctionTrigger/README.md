# Azure Function trigger

## Running the project locally
Open the solution in Visual Studio and select the Function Trigger project as the startup project. The project requires a bit of manual configuration.

- Go to the Azure Portal and visit the created IoT Hub
- Click the link 'Built-in Endpoints' in the left sidebar
- In the "Event Hub compatible endpoint" section, select `service` as the Shared access policy
- Copy the Event Hub-compatible endpoint string and paste it into `IotHubServiceEndpoint` in `local.settings.json`.

Then, press run. The project requires .NET Core 3.1

## Viewing telemetry data

- Goto to the Azure Portal and visit the Function App service
- Click on Function App that you just created
- Click on the 'Functions' link under heading 'Functions' in the left sidebar
- Click the function 'ProcessWeatherTelemetry'
- Click on the link 'Monitor' in the left sidebar
- Click on the 'Logs' tab at the top of the screen
- Select the 'App Insights Logs'
