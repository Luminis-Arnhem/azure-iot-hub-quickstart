Clear-Host []

Write-Host "  .___     ___________   ___ ___      ___.
  |   | ___\__    ___/  /   |   \ __ _\_ |__
  |   |/  _ \|    |    /    ~    \  |  \ __ \
  |   (  <_> )    |    \    Y    /  |  / \_\ \
  |___|\____/|____|     \___|_  /|____/|___  /
                              \/           \/" -ForegroundColor Magenta

Write-Host "          Azure IoT Hub Quickstart" -ForegroundColor Magenta
Write-Host ""

## Exit codes
$LoginFailure = 1
$NoSubscriptionsFound = 2

$FunctionAppStorageCreationError = 3
$FunctionAppCreationError = 4
$FunctionAppConfigurationCreationError = 5

## Custom functions
function Add-Separator {
    Write-Host "----------------------" -ForegroundColor DarkGray
}

function Add-MenuItem ([String]$Key, [String]$Label) {
    $obj = New-Object -TypeName psobject
    $obj | Add-Member -MemberType NoteProperty -Name key -Value $Key
    $obj | Add-Member -MemberType NoteProperty -Name value -Value $Label
    return $obj
}

function Show-MenuItems ([psobject[]]$Items) {
    For ($i = 0; $i -lt $Items.length; $i++) {
        $item = $Items[$i]
        Write-Host "[$($item.key)]:" $item.value -ForegroundColor Yellow
    }
}

function Show-Menu ([String]$Title, [String]$Question, [psobject[]]$Items, [String]$DefaultValue) {
    Add-Separator
    Write-Host $Title -ForegroundColor Magenta
    Show-MenuItems -Items $Items
    do {
        $selectedIndex = $null
        $selection = Read-Host $Question "[$DefaultValue] Default; [?] Help"
        if ($selection -eq "?") {
            Show-MenuItems -Items $Items

        } else {
            if ($selection -eq "") {
                $selection = $DefaultValue
            }
            For ($i = 0; $i -lt $Items.length; $i++) {
                $item = $Items[$i]
                if ($item.key -eq $selection) {
                    $selectedIndex = $i
                    break
                }
            }
            if ($selectedIndex -eq $null) {
                Write-Host "Incorrect value!" -ForegroundColor Red
            }
        }
     }
     until ($selectedIndex -ne $null)
     return $selectedIndex
}

function Select-Subscription ($Subscriptions) {
    $title = "Select subscription"
    $prompt = "Select the correct subscription"

    $subscriptionChoices = @()
    For ($i = 0; $i -lt $Subscriptions.length; $i++) {
        $subscription = $Subscriptions[$i]
        $subscriptionChoices += Add-MenuItem -Key $i -Label $subscription.name
    }
    $selectedIndex = Show-Menu -Title $title -Question $prompt -Items $subscriptionChoices -DefaultValue 0
    return $Subscriptions[$selectedIndex]
}

function Add-ResourceGroup {
    do {
        do {
            $newResourceGroupName = Read-Host "Please type the name of the new resource group"
        }
        until ($newResourceGroupName -ne "")
        $resourceLocations = az account list-locations | ConvertFrom-Json

        $title = "Select resource location"
        $prompt = "Select the location for your resource group"

        $locationChoices = @()
        For ($i = 0; $i -lt $resourceLocations.length; $i++) {
            $location = $resourceLocations[$i]
            $locationChoices += Add-MenuItem -Key $i -Label $location.regionalDisplayName
        }

        $selectedIndex = Show-Menu -Title $title -Question $prompt -Items $locationChoices -DefaultValue 0
        $selectedLocation = $resourceLocations[$selectedIndex]
        $selectedLocationName = $selectedLocation.displayName
        Write-Host "Selected resource location:" $selectedLocation.regionalDisplayName -ForegroundColor Green

        Write-Host "Creating resource group" $newResourceGroupName "at location" $selectedLocation.regionalDisplayName -ForegroundColor Cyan
        $group = az group create --name $newResourceGroupName --location $selectedLocationName | ConvertFrom-Json
        if (!$group) {
            Write-Host "Error creating resource group, make sure the name is unique for you as a customer!"
        }
    }
    until ($group)
    return $group
}

function Select-ResourceGroup {
    $resourceGroups = az group list --subscription $selectedSubscription.id | ConvertFrom-Json

    if ($resourceGroups.length -eq 0) {
        Add-Separator
        Write-Host "No existing resource groups found. Start creation..." -ForegroundColor Red
        $resourceGroup = Add-ResourceGroup
    }
    else {
        $title = "Select resource group"
        $prompt = "Select the correct resource group"

        $groupChoices = @()
        $groupChoices += Add-MenuItem -Key "n" -Label "New resource group"
        For ($i = 0; $i -lt $resourceGroups.length; $i++) {
            $group = $resourceGroups[$i]
            $groupChoices += Add-MenuItem -Key $i -Label $group.name
        }
        $groupIndex = Show-Menu -Title $title -Question $prompt -Items $groupChoices -DefaultValue "n"
        $resourceGroup = $null
        if ($groupIndex -eq 0) {
            $resourceGroup = Add-ResourceGroup
        } else {
            $resourceGroup = $resourceGroups[$groupIndex - 1]
        }
    }
    return $resourceGroup
}

function Add-IotHub {
    Add-Separator
    $matchPattern = "(^[0-9a-z]{3,18}$)"
    do {
        do {
            Write-Host "Supply a name for the IoT Hub which is globally unique for all Azure customers." -ForegroundColor Blue
            $iotHubName = Read-Host "What will your IoT Hub be called?"
            if ($iotHubName -cNotMatch $matchPattern) {
                Write-Host "The name must be between 3 and 18 characters (only numeric and lowercase alphanumeric)" -ForegroundColor Red
            }
        }
        until ($iotHubName -cmatch $matchPattern)

        Write-Host "Creating IoT Hub" $iotHubName "for resource group" $resourceGroup.name -ForegroundColor Cyan
        $iotHub = az iot hub create --name $iotHubName --resource-group $resourceGroup.name --sku "F1" --partition-count 2 | ConvertFrom-Json
        if (!$iotHub) {
            Write-Host "Error creating IoT Hub, the name should be globally unique for all Azure customers!" -ForegroundColor Red
        }
    }
    until ($iotHub)
    return $iotHub
}

function Add-Device ([String]$IotHubName) {
    Add-Separator
    do {
        do {
            $deviceName = Read-Host "What will your starter IoT device be called?"
        }
        until ($deviceName -ne "")
        Write-Host "Creating device" $deviceName "for Iot Hub" $IotHubName -ForegroundColor Cyan
        $device = az iot hub device-identity create -n $IotHubName -d $deviceName | ConvertFrom-Json
        if (!$device) {
            Write-Host "Error creating device, the name should be unique for the IotHub!" -ForegroundColor Red
        }
    }
    until ($device)

    Write-Host "Your primary connection string for this device is:"
    Write-Host "HostName=$iotHubName.azure-devices.net;DeviceId=$deviceName;SharedAccessKey=$($device.authentication.symmetricKey.primaryKey)"
    Write-Host "You can look this up later in the Azure Portal"

    return $device
}

function Add-FunctionApp([String]$IotHubName, [String]$ResourceGroupName, [String]$ResourceGroupLocation, [String]$IoTHubEndpoint) {
    Add-Separator
    Write-Host "Now creating the necessary FunctionApp" -ForegroundColor Magenta

    # Storage account name must be between 3 and 24 characters in length and use numbers and lower-case letters only.
    Write-Host "Creating storage account for the function app..."
#     az storage account create --name "$($IotHubName)sta" --resource-group $ResourceGroupName --sku Standard_LRS

    ## This won't work, fails even if the storage account is successfully created
    $storageAccount = az storage account create --name "$($IotHubName)sta" --resource-group $ResourceGroupName --sku Standard_LRS | ConvertFrom-Json
    if (!$storageAccount) {
        Write-Host "Error creating storage account!" -ForegroundColor Red
        Write-Host $storageAccount
        exit $FunctionAppStorageCreationError
    }

    $storageAccountName = $storageAccount.name

    Write-Host "Creating the function app..."
    $functionApp = az functionapp create --name "$($IotHubName)fn" --os-type Windows --resource-group $ResourceGroupName --storage-account $storageAccountName --consumption-plan-location $ResourceGroupLocation --functions-version 3 | ConvertFrom-Json
    if (!$functionApp) {
        Write-Host "Error creating function app configuration!" -ForegroundColor Red
        exit $FunctionAppCreationError
    }
    $functionAppName = $functionApp.name

    Write-Host "Updating Azure Function configuration to connect with the IoT Hub..."
    $config = az functionapp config appsettings set --name $functionAppName --resource-group $ResourceGroupName --settings "IoTHubServiceEndpoint=$IoTHubEndpoint"
    if (!$config) {
        Write-Host "Error creating function app configuration!" -ForegroundColor Red
        exit $FunctionAppConfigurationCreationError
    }

    Write-Host "Building the function application..."
    [System.IO.Directory]::SetCurrentDirectory(((Get-Location -PSProvider FileSystem).ProviderPath))
    $sourceFolder = ".\Source\C#\Azure.IoT.FunctionTrigger\"
    Write-Host "Source folder:" $sourceFolder -ForegroundColor Yellow

    $projectPath = Join-Path -Path $sourceFolder -ChildPath "Azure.IoT.FunctionTrigger.csproj"
    Write-Host "Project path:" $projectPath -ForegroundColor Yellow
    dotnet publish $projectPath -c Release

    $publishFolder = Join-Path -Path $sourceFolder -ChildPath "\bin\Release\netcoreapp3.1\publish\"
    Write-Host "Publish folder:" $publishFolder -ForegroundColor Yellow

    Write-Host "Publishing the function application..."
    $publishFile = "publish.zip"
    $assembledPath = Join-Path -Path $sourceFolder -ChildPath "\assembled"
    Write-Host "Assembled path:" $assembledPath -ForegroundColor Cyan
    if (!(Test-path $assembledPath)) { New-Item -ItemType Directory -Force -Path $assembledPath }

    $assembledFilePath = Join-Path -Path $assembledPath -ChildPath $publishFile
    Write-Host "Assembled filepath:" $assembledFilePath -ForegroundColor Cyan
    if(Test-path $assembledFilePath) { Remove-item $assembledFilePath }
    Add-Type -assembly "system.io.compression.filesystem"
    [io.compression.zipfile]::CreateFromDirectory($publishFolder, $assembledFilePath)

    Write-Host "Deploying the function application..."
    az functionapp deployment source config-zip -g $ResourceGroupName -n $functionAppName --src $assembledFilePath

    Write-Host "Deployment of all components done!"
}

### Main Script

## Checks if you are logged in, otherwise calls login command
try {
    $currentToken = $(az account get-access-token) | ConvertFrom-Json
    if ([datetime]$currentToken.expiresOn -le [datetime]::Now) {
        throw
    }
    Write-Host 'Already logged in' -ForegroundColor Blue
    $accountSubscriptions = az account list | ConvertFrom-Json
}
catch {
    Write-Host 'Executing Azure login' -ForegroundColor Yellow
    $accountSubscriptions = az login | ConvertFrom-Json
    if ($LASTEXITCODE -ne 0) {
        Write-Host 'Failed to login to Azure' -ForegroundColor Red
        exit $LoginFailure
    }
}

if ($accountSubscriptions.length -eq 0) {
    Write-Host 'No subscriptions found, exiting...' -ForegroundColor Red
    exit $NoSubscriptionsFound
}

## Select subscription
$selectedSubscription = Select-Subscription -Subscriptions $accountSubscriptions
Write-Host "You have selected subscription:" $selectedSubscription.name -ForegroundColor Green

## Select resource group
$resourceGroup = Select-ResourceGroup
$resourceGroupLocation = $resourceGroup.location
Write-Host "You have selected resource group" $resourceGroup.name "at location" $resourceGroupLocation -ForegroundColor Green

## add IotHub
$iotHub = Add-IotHub
$iotHubName = $iotHub.name

$serviceSharedAccessPolicy = az iot hub policy show --hub-name $iotHubName --name service | ConvertFrom-Json
$sharedAccessServicePrimaryKey = $serviceSharedAccessPolicy.primaryKey
$endpoint = $iotHub.properties.eventHubEndpoints.events.endpoint;
$entityPath = $iotHub.properties.eventHubEndpoints.events.path;
$IoTHubEndpoint = "Endpoint=$endpoint;SharedAccessKeyName=service;SharedAccessKey=$sharedAccessServicePrimaryKey;EntityPath=$entityPath"

## Create Device
do {
    $createdDevice = Add-Device -IotHubName $iotHubName
    $anotherDevice = Read-Host "Would you like to add another device [y/N]?"
} until ($anotherDevice -eq "" -or $anotherDevice.ToLower() -eq "n")

## Create FunctionApp
$functionApp = Add-FunctionApp -IotHubName $iotHubName -ResourceGroupName $resourceGroup.name -ResourceGroupLocation $resourceGroupLocation -IoTHubEndpoint $IoTHubEndpoint
