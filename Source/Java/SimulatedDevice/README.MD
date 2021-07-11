# Simulated Device

## Configuration 
When creating a new device using the setups.ps1 script, the Primary Connection String is written to the output.
Use this Primary Connection String in the application.properties file as the value for the 'connectionString' property. 

In case you have missed it, you can find the Primary Connection String following the next steps:
- Goto the Azure Portal and visit the created IoT Hub
- Click the link 'IoT Devices' in the left sidebar
- Click on the device for which you want the connection string
- Copy the Primary (Secondary) Connection String


## Run
```shell script
$ mvn compile
$ mvn package
$ java -jar target/SimulatedDevice-1.0-jar-with-dependencies.jar
```
