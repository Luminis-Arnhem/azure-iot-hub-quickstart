package device.weather;

import device.weather.azure.*;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.microsoft.azure.sdk.iot.deps.twin.TwinCollection;
import com.microsoft.azure.sdk.iot.device.DeviceTwin.Property;
import com.microsoft.azure.sdk.iot.device.IotHubClientProtocol;
import com.microsoft.azure.sdk.iot.device.IotHubStatusCode;
import com.microsoft.azure.sdk.iot.device.Message;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import device.properties.PropertyFileReader;
import device.weather.sensors.HumiditySensor;
import device.weather.sensors.TemparatureSensor;
import device.weather.sensors.impl.HumiditySensorImpl;
import device.weather.sensors.impl.TemperatureSensorImpl;

import java.io.IOException;
import java.util.concurrent.Executors;
import java.util.concurrent.ScheduledExecutorService;
import java.util.concurrent.TimeUnit;

public class SimulatedWeatherStation implements HubClientEventResponseListener, DesiredPropertyCallback {

    private final ScheduledExecutorService executor;
    private final ObjectMapper objectMapper = new ObjectMapper();
    private final HubClient hubClient;

    private final TemparatureSensor temparatureSensor = new TemperatureSensorImpl();
    private final HumiditySensor humiditySensor = new HumiditySensorImpl();

    private int interval = 10;

    private static Logger logger;

    public SimulatedWeatherStation() throws IOException {
        logger = LogManager.getLogger(getClass());

        final PropertyFileReader properties = new PropertyFileReader();
        this.executor = Executors.newScheduledThreadPool(1);
        this.hubClient = new HubClient(
                properties.getProperty("HostName"),
                properties.getProperty("DeviceId"),
                properties.getProperty("SharedAccessKey"),
                IotHubClientProtocol.valueOf(properties.getProperty("protocol", "MQTT"))
        );
    }

    public void execute() {
        try {
            hubClient.connect();
            hubClient.setEventResponseDelegate(this);
            hubClient.setDesiredPropertyCallback(this);
            executor.execute(sendTemperatureData());
        } catch (HubClientException e) {
            e.printStackTrace();
        }
    }

    private Runnable sendTemperatureData() {
        return () -> {
            final WeatherData weatherData = new WeatherData(temparatureSensor.getTemperature(), humiditySensor.getHumidity());
            logger.info(String.format("Sending weather data; %s", weatherData));
            try {
                final Message message = new Message(objectMapper.writeValueAsString(weatherData));
                message.setProperty("temperatureAlert", "true");
                hubClient.sendEvent(message, message);
            } catch (JsonProcessingException e) {
                logger.error("Couldn't serialize weather data", e);
            } catch (HubClientException e) {
                logger.error("Couldn't send message to IoT Hub", e);
            }
        };
    }

    @Override
    public void onEventResponse(IotHubStatusCode statusCode, Object context) {
        if (statusCode.equals(statusCode.OK) || statusCode.equals(statusCode.OK_EMPTY)) {
            logger.debug(String.format("Schedules executor with %s seconds", interval));
            executor.schedule(sendTemperatureData(), interval, TimeUnit.SECONDS);
        } else if (statusCode.equals(statusCode.THROTTLED) || statusCode.equals(statusCode.SERVER_BUSY)) {
            logger.warn(String.format("Schedules executor is throttled", interval));
            executor.schedule(sendTemperatureData(), 120, TimeUnit.SECONDS);
        } else {
            logger.error(String.format("Unexpected server response status code: %s", statusCode));
            executor.schedule(sendTemperatureData(), 300, TimeUnit.SECONDS);
        }
    }

    @Override
    public void onTelemetryConfigChanged(Property property) {
        logger.debug(String.format("Desired Property change: %s", property));
        if (property.getKey().equals(WeatherDataPatch.class.getSimpleName())) {
            final TwinCollection patch = (TwinCollection) property.getValue();
            interval = ((Double) patch.get("ReportingRateSeconds")).intValue();
            logger.debug(String.format("Received new reporting rate: %s", interval));
        }
    }
}
