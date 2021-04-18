package device.weather.sensors.impl;

import device.weather.sensors.TemparatureSensor;

import java.util.Random;

public class TemperatureSensorImpl implements TemparatureSensor {

    @Override
    public double getTemperature() {
        final double minTemperature = 15d;
        return minTemperature + new Random().nextDouble() * 15;
    }

}
