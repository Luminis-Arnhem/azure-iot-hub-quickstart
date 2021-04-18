package device;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import device.weather.SimulatedWeatherStation;

import java.io.IOException;
import static java.lang.System.exit;

public class SimulatedDevice {

    public static void main(String[] args) {
        final Logger logger = LogManager.getLogger(SimulatedDevice.class);
        logger.info("Starting the Simulated Device");
        try {
            new SimulatedWeatherStation().execute();
        } catch (IOException e) {
            logger.error("Failure reading device.properties", e);
            exit(-1);
        }
    }

}
