package device.weather.sensors.impl;

import device.weather.sensors.HumiditySensor;

import java.util.Random;

public class HumiditySensorImpl implements HumiditySensor {

    @Override
    public double getHumidity() {
        final double minHumidity = 60d;
        return minHumidity + new Random().nextDouble() * 20;
    }

}
