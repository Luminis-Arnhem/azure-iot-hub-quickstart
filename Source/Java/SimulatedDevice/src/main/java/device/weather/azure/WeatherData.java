package device.weather.azure;

public class WeatherData {

    private double Temperature;
    private double Humidity;

    public WeatherData(double temperature, double humidity) {
        Temperature = temperature;
        Humidity = humidity;
    }

    public double getTemperature() {
        return Temperature;
    }

    public double getHumidity() {
        return Humidity;
    }

    @Override
    public String toString() {
        return "WeatherData{" +
                "Temperature=" + Temperature +
                ", Humidity=" + Humidity +
                '}';
    }
}
