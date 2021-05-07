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
    $resourceGroupLocation = $resourceGroup.location

    $iotHubName = Read-Host "What will your IoT Hub be called?"
    $iotHub = az iot hub create --name $iotHubName --resource-group $resourceGroup.name --sku "F1" --partition-count 2 | ConvertFrom-Json
    
    #TODO: "Built-in endpoints" in Azure Portal. this one is not correct. Get the built-in endpoint of Shared Access Policy "service"
    # $serviceSharedAccessPolicy = az iot hub policy show --hub-name $iotHubName --name service | ConvertFrom-Json
    # $sharedAccessServicePrimaryKey = $serviceSharedAccessPolicy.primaryKey
    # $sharedAccessServiceConnectionString = "HostName=$iotHubName.azure-devices.net;SharedAccessKeyName=service;SharedAccessKey=$sharedAccessServicePrimaryKey"

    $deviceId = Read-Host "What will your starter IoT device be called?"
    $createdDevice = az iot hub device-identity create -n $iotHubName -d $deviceId | ConvertFrom-Json

    Write-Host "Your primary connection string for this device is:"
    Write-Host "HostName=$iotHubName.azure-devices.net;DeviceId=$deviceId;SharedAccessKey=$($createdDevice.authentication.symmetricKey.primaryKey)"
    Write-Host "You can look this up later in the Azure Portal"

    Write-Host "Now creating the necessary FunctionApp"

    # Storage account name must be between 3 and 24 characters in length and use numbers and lower-case letters only.
    Write-Host "Creating storage account for the function app..."
    az storage account create --name "$($iotHubName)sta" --resource-group $resourceGroup.name --sku Standard_LRS

    Write-Host "Creating the function app..."
    $functionApp = az functionapp create --name "$($iotHubName)fn" --os-type Windows --resource-group $resourceGroup.name --storage-account "$($iotHubName)sta" --consumption-plan-location $resourceGroupLocation --functions-version 3 | ConvertFrom-Json

    Write-Host "Updating Azure Function configuration to connect with the IoT Hub..."
    az functionapp config appsettings set --name $functionApp.name --resource-group $resourceGroup.name --settings "IoTHubServiceEndpoint=$sharedAccessServiceConnectionString"

    Write-Host "Building the function application..."
    dotnet publish ".\Source\C#\Azure.IoT.FunctionTrigger\Azure.IoT.FunctionTrigger.csproj" -c Release
    $publishFolder = "Source\C#\Azure.IoT.FunctionTrigger\bin\Release\netcoreapp3.1\publish"

    Write-Host "Publishing the function application..."
    $publishZip = "publish.zip"
    if(Test-path $publishZip) {Remove-item $publishZip}
    Add-Type -assembly "system.io.compression.filesystem"
    [io.compression.zipfile]::CreateFromDirectory($publishFolder, $publishZip)

    Write-Host "Deploying the function application..."
    az functionapp deployment source config-zip -g $resourceGroup.name -n $functionApp.name --src $publishZip
}

# TODO: add selection (C#, Java, Python)
# TODO: Fine tuning iot hub name input (max 18 chars, no hyphens)
# TODO: gitignore publish.zip
# TODO: general error handling/retry mechanism
