Write-Host "Azure IoT Hub Quickstart"
$SUBSCRIPTIONS = az login | ConvertFrom-Json

Write-Host "Listing your subscriptions..."
# TODO: Extract as function?
For ($i = 0; $i -lt $SUBSCRIPTIONS.length; $i++) {
    $name = $SUBSCRIPTIONS[$i].name;
    Write-Host "Press $i for $name" 
}
$index = Read-Host "Please select your subscription"
Write-Host "You have selected" $SUBSCRIPTIONS[$index].name

$selectedSubscription = $SUBSCRIPTIONS[$index]
$resourceGroups = az group list --subscription $selectedSubscription.id | ConvertFrom-Json

if ($resourceGroups.length -eq 0) {
    # TODO: no resource groups, create new
    # pick preferred location
}
else {
    Write-Host "Available resource groups:"
    For ($i = 0; $i -lt $resourceGroups.length; $i++) {
        $name = $resourceGroups[$i].name;
        Write-Host "Press $i for $name" 
    }
    $resourceIndex = Read-Host "Please select your resource group"
    # TODO: "n" for new

    Write-Host "You have selected" $resourceGroups[$resourceIndex].name
    $resourceGroup = $resourceGroups[$resourceIndex]

    $iotHubName = Read-Host "What will your IoT Hub be called?"
    $iotHub = az iot hub create --name $iotHubName --resource-group $resourceGroup.name --sku "F1" --partition-count 2 | ConvertFrom-Json

    # TODO: get "service" shared access policy endpoint

    $deviceId = Read-Host "What will your starter IoT device be called?"
    $createdDevice = az iot hub device-identity create -n $iotHubName -d $deviceId

    Write-Host "Your primary connection string for this device is:"
    Write-Host "HostName=$iothubname.azure-devices.net;DeviceId=$deviceId;SharedAccessKey=$($createdDevice.authentication.symmetricKey.primaryKey)"
    Write-Host "You can look this up later in the Azure Portal"
}

# TODO: Create Azure Function
# TODO: Deploy Azure Function project. add selection (C#, Java, Python)