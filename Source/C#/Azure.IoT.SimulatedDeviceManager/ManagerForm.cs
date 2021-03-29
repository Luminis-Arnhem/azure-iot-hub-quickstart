using Azure.IoT.Models.DeviceTwinPatch;
using Microsoft.Azure.Devices;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Azure.IoT.SimulatedDeviceManager
{
    public partial class ManagerForm : Form
    {
        private const string deviceId = "TelemetryPrototype";
        private readonly RegistryManager registryManager;

        public ManagerForm()
        {
            InitializeComponent();

            // Portal - IoT Hub - Shared Access Policies - service
            var registryConnectionString = Environment.GetEnvironmentVariable("registryConnectionString");
            if(registryConnectionString == null)
            {
                MessageBox.Show("Please set the `registryConnectionString` environment variable");
                Environment.Exit(-1);
            }

            this.registryManager = RegistryManager.CreateFromConnectionString(registryConnectionString);
        }

        private async void OnUpdateDeviceTwinButtonClick(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;
            var weatherStation = await this.registryManager.GetTwinAsync(deviceId);
            weatherStation.Properties.Desired[nameof(WeatherDataPatch)] = new WeatherDataPatch()
            {
                ReportingRateSeconds = Convert.ToInt32(this.nud_reportingRate.Value),
            };

            await this.registryManager.UpdateTwinAsync(deviceId, weatherStation, weatherStation.ETag);
            ((Button)sender).Enabled = true;
        }

        private async void OnOverTheAirUpdateButtonClick(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;
            var versionNumber = Guid.NewGuid().ToString();

            var config = new Configuration(versionNumber)
            {
                Content = new ConfigurationContent(),
            };

            config.Content.DeviceContent = new Dictionary<string, object>()
            {
                ["properties.desired." + nameof(NewVersion)] =  new NewVersion()
                {
                    FirmwareVersion = versionNumber,
                    FirmwarePackageUri = $"https://example.com/{versionNumber}.zip",
                    FirmwarePackageChecksum = "123ABC",
                },
            };

            // Currently targets all devices within the IoT Hub
            config.TargetCondition = "*";
            config.Priority = 1;

            await this.registryManager.AddConfigurationAsync(config);
            this.lbl_versionNumber.Text = versionNumber;

            ((Button)sender).Enabled = true;
        }
    }
}
