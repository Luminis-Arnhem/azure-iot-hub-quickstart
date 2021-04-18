package device.weather.azure;

import com.microsoft.azure.sdk.iot.device.DeviceTwin.Property;

public interface DesiredPropertyCallback {
    void onTelemetryConfigChanged(Property property);
}
