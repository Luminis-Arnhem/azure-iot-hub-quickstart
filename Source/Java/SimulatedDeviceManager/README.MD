# Simulated Device Manager

## Configuration
When creating the IoT Hub using the setups.ps1 script, the Shared Access Policy Connection String is written to the output.
Use this Connection String in the application.properties file as the value for the 'connectionString' property.

In case you have missed it, you can find the Connection String following the next steps:
- Goto the Azure Portal and visit the created IoT Hub
- Click the link 'Shared access policies' in the left sidebar
- Click the policy name 'iothubowner'
- Copy the value of the 'Primary Conection String'

## Run
```shell script
$ mvn compile
$ mvn package
$ java -jar target/SimulatedDeviceManager-1.0-jar-with-dependencies.jar
```
