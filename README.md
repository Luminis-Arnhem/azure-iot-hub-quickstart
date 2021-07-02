# Azure IoT Hub Quickstart
Azure IoT Hub Quickstart is a quickstart project to help you get started with Azure IoT Hub. The project contains a script for deploying a basic Azure IoT Hub infrastructure to Microsoft Azure. The project also contains an example simulated device, a serverless function for data processing, as well as an Azure IoT Hub device manager application.

## Prerequisites
Before getting started, you should have the following tools installed:
- Azure CLI
- PowerShell or PowerShell Core
- .NET & .NET Core 3.1, or Java
- A .NET IDE of your choice
- (Optional) a Java IDE of your choice

You should also have an Azure Subscription available for use.

## Getting started
To get started, run the following command in your terminal:
```sh
.\setup.ps1
```
This script takes you through some prompts to deploy a basic cloud infrastructure in Azure.

## Simulated devices
The repo contains simulated devices which report sensor data to the IoT Hub. There is a .NET and Java version of the simulated device, both of which do the same thing. Simply run the project after deploying the IoT Hub infrastructure and taking note of the device connection string.

## Azure Function
The Azure Function in this repo reads incoming data from the IoT Hub, and outputs it to the logger. These logs can be read in Azure Application Insights or from the logs window of that Azure Function. The project automatically gets deployed to Microsoft Azure while you run the deployment script.

## Device Manager
The device manager is an example GUI application which connects to the Azure IoT Hub to manage certain devices, such as updating their device twins. There is a .NET and Java version of the device manager, both of which do the same thing. Simply run the project after deploying the IoT Hub infrastructure and taking note of the shared access policy connection string.