using Azure.IoT.Models.DeviceTwinPatch;
using Microsoft.Azure.Devices;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Azure.IoT.SimulatedDeviceManager
{
    public partial class ManagerForm : Form
    {
        private readonly RegistryManager registryManager;

        public ManagerForm()
        {
            InitializeComponent();

            // Portal - IoT Hub - Shared Access Policies - service
            var registryConnectionString = Environment.GetEnvironmentVariable("registryConnectionString");
            if (registryConnectionString == null)
            {
                MessageBox.Show("Please set the `registryConnectionString` environment variable");
                Environment.Exit(-1);
            }
            registryManager = RegistryManager.CreateFromConnectionString(registryConnectionString);

            SetToolTips();
            UpdateListOfDevices();
        }

        private void SetToolTips()
        {
            toolTip.SetToolTip(btn_refresh, "Refreshes the list of devices");
            toolTip.SetToolTip(btn_updateDeviceTwin, "Updates the device twin of the selected devices");
            toolTip.SetToolTip(btn_newVersion, "Publishes a new device configuration to the IoT Hub, causing all device twins to update with a new version number");
        }

        private void UpdateListOfDevices()
        {
            clb_devices.Items.Clear();
            var query = registryManager.CreateQuery("select * from devices");
            while (query.HasMoreResults)
            {
                var page = query.GetNextAsTwinAsync().GetAwaiter().GetResult();
                foreach (var twin in page)
                {
                    clb_devices.Items.Add(twin.DeviceId);
                }
            }
        }

        private async void OnUpdateDeviceTwinButtonClick(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;
            var collection = clb_devices.CheckedItems;
            foreach (var item in clb_devices.CheckedItems)
            {
                var weatherStation = await registryManager.GetTwinAsync(item.ToString());
                weatherStation.Properties.Desired[nameof(WeatherDataPatch)] = new WeatherDataPatch()
                {
                    ReportingRateSeconds = Convert.ToInt32(nud_reportingRate.Value),
                };

                await registryManager.UpdateTwinAsync(item.ToString(), weatherStation, weatherStation.ETag);
            }
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
                ["properties.desired." + nameof(NewVersion)] = new NewVersion()
                {
                    FirmwareVersion = versionNumber,
                    FirmwarePackageUri = $"https://example.com/{versionNumber}.zip",
                    FirmwarePackageChecksum = "123ABC",
                },
            };

            // Currently targets all devices within the IoT Hub
            config.TargetCondition = "*";
            config.Priority = 1;

            await registryManager.AddConfigurationAsync(config);
            lbl_versionNumber.Text = versionNumber;

            ((Button)sender).Enabled = true;
        }

        private void OnRefreshButtonClick(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;
            UpdateListOfDevices();
            ((Button)sender).Enabled = true;
        }
    }
}
