package device.weather.azure;

import com.microsoft.azure.sdk.iot.device.*;
import com.microsoft.azure.sdk.iot.device.DeviceTwin.Pair;
import com.microsoft.azure.sdk.iot.device.DeviceTwin.Property;
import com.microsoft.azure.sdk.iot.device.DeviceTwin.PropertyCallBack;
import com.microsoft.azure.sdk.iot.device.DeviceTwin.TwinPropertyCallBack;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import java.io.IOException;
import java.net.URISyntaxException;
import java.util.HashMap;
import java.util.Map;

public class HubClient {

    private final String connectionString;

    private final IotHubClientProtocol protocol;

    private final Logger logger;

    private DeviceClient client;

    private HubClientEventResponseListener eventResponseDelegate;

    private DesiredPropertyCallback desiredPropertyCallback;


    public HubClient(String connectionString, IotHubClientProtocol protocol) {
        this.connectionString = connectionString;
        this.protocol = protocol;
        this.logger = LogManager.getLogger(getClass());
    }

    public void connect() throws HubClientException {
        disconnect();
        try {
            client = new DeviceClient(connectionString, protocol);
            client.open();
            registerOnDesiredProperties();
        } catch (URISyntaxException e) {
            throw new HubClientException("Error creating connection", e);
        } catch (IOException e) {
            throw new HubClientException("Couldn't open Device Client", e);
        }
    }

    public void disconnect() throws HubClientException {
        try {
            if (client != null) {
                client.closeNow();
                client = null;
            }
        } catch (IOException e) {
            throw new HubClientException("Couldn't close Device Client", e);
        }
    }

    public void sendEvent(Message message, Object context) throws HubClientException {
        if (client == null) {
            throw new HubClientException("Device Client not open");
        }
//        logger.debug(String.format("Sending event; %s, %s", message, context));
        client.sendEventAsync(message, this::handleEventSync, context);

    }

    private void registerOnDesiredProperties() {
        try {
            client.startDeviceTwin(new DeviceTwinStatusCallback(), null, new PropertyCallback(), null);
            logger.info("Azure; Subscribed to direct methods and polling for reported properties. Waiting...");
            Map<Property, Pair<TwinPropertyCallBack, Object>> desiredProperties = new HashMap<Property, Pair<TwinPropertyCallBack, Object>>() {
                {
                    put(new Property("WeatherDataPatch", null), new Pair<TwinPropertyCallBack, Object>((property, o) -> {
                        if (desiredPropertyCallback != null) {
                            desiredPropertyCallback.onTelemetryConfigChanged(property);
                        }
                    }, null));

                    put(new Property("NewVersion", null), new Pair<TwinPropertyCallBack, Object>((property, o) -> {
                        if (desiredPropertyCallback != null) {
                            desiredPropertyCallback.onTelemetryConfigChanged(property);
                        }
                    }, null));
                }
            };
            client.subscribeToTwinDesiredProperties(desiredProperties);
        } catch (Exception e) {
            logger.error("Azure; On exception, shutting down \n" + " Cause: " + e.getCause() + " \n" + e.getMessage());
            try {
                if (client != null) {
                    client.closeNow();
                }
            } catch (IOException e1) {
                e1.printStackTrace();
            }
            logger.debug("Azure; Shutting down...");
        }
    }

    public void setEventResponseDelegate(HubClientEventResponseListener eventResponseDelegate) {
        this.eventResponseDelegate = eventResponseDelegate;
    }

    public void setDesiredPropertyCallback(DesiredPropertyCallback desiredPropertyCallback) {
        this.desiredPropertyCallback = desiredPropertyCallback;
    }

    private void handleEventSync(IotHubStatusCode iotHubStatusCode, Object ctx) {
        if (this.eventResponseDelegate != null) {
            this.eventResponseDelegate.onEventResponse(iotHubStatusCode, ctx);
        }
    }

    protected static class DeviceTwinStatusCallback implements IotHubEventCallback {
        public void execute(IotHubStatusCode status, Object context) {
            System.out.println(String.format("IoT Hub responded to device twin operation with status: %s; %s", status.name(), context));
        }
    }

    protected static class PropertyCallback implements PropertyCallBack<String, String> {
        public void PropertyCall(String propertyKey, String propertyValue, Object context) {
            System.out.println(String.format("Azure; PropertyKey:%s, PropertyValue:%s", propertyKey, propertyKey));
        }
    }
}
