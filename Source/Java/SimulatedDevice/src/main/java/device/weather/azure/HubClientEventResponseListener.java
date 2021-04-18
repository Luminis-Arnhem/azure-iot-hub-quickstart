package device.weather.azure;

import com.microsoft.azure.sdk.iot.device.IotHubStatusCode;

public interface HubClientEventResponseListener {

    void onEventResponse(IotHubStatusCode statusCode, Object context);

}
