package manager.azure;

public class NewVersion {

    public String FirmwareVersion;
    public String FirmwarePackageUri;
    public String FirmwarePackageChecksum;

    public NewVersion() {
        // nothing
    }

    public NewVersion(String firmwareVersion, String firmwarePackageUri, String firmwarePackageChecksum) {
        FirmwareVersion = firmwareVersion;
        FirmwarePackageUri = firmwarePackageUri;
        FirmwarePackageChecksum = firmwarePackageChecksum;
    }

}
