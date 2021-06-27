# Azure IoT Hub

- Create a IoT hub using this documentation: https://docs.microsoft.com/en-gb/azure/iot-hub/iot-hub-create-through-portal
- Create a simulation device in Java: https://github.com/Azure/azure-iot-sdk-java 

## Running simulated device
```shell script
$ mvn compile
$ mvn package
$ java -jar target/device.SimulatedDeviceManager-1.0-jar-with-dependencies.jar
```
