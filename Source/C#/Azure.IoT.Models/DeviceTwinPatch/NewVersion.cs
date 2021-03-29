namespace Azure.IoT.Models.DeviceTwinPatch
{
    public class NewVersion
    {
        public string FirmwareVersion { get; set; }
        public string FirmwarePackageUri { get; set; }
        public string FirmwarePackageChecksum { get; set; }
    }
}
