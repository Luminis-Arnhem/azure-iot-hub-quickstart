using Azure.IoT.Models.DeviceTwinPatch;
using Microsoft.Azure.Devices;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

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

            this.registryManager = RegistryManager.CreateFromConnectionString(registryConnectionString);
            var query = this.registryManager.CreateQuery("select * from devices");
            while (query.HasMoreResults)
            {
                var page = query.GetNextAsTwinAsync().GetAwaiter().GetResult();
                foreach (var twin in page)
                {
                    checkedListBox1.Items.Add(twin.DeviceId);
                }
            }
        }

        private async void OnUpdateDeviceTwinButtonClick(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;
            var collection = checkedListBox1.CheckedItems;
            foreach (var item in checkedListBox1.CheckedItems)
            {
                var weatherStation = await this.registryManager.GetTwinAsync(item.ToString());
                weatherStation.Properties.Desired[nameof(WeatherDataPatch)] = new WeatherDataPatch()
                {
                    ReportingRateSeconds = Convert.ToInt32(this.nud_reportingRate.Value),
                };

                await this.registryManager.UpdateTwinAsync(item.ToString(), weatherStation, weatherStation.ETag);
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

            await this.registryManager.AddConfigurationAsync(config);
            this.lbl_versionNumber.Text = versionNumber;

            ((Button)sender).Enabled = true;
        }
    }
}
